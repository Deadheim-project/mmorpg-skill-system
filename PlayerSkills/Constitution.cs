using BepInEx.Configuration;
using HarmonyLib;

namespace ValheimLevelSystem.PlayerSkills
{
    [HarmonyPatch]
    public class Constitution
    {
        public static ConfigEntry<float> PassiveLifeBonus;
        public static ConfigEntry<float> PassiveStaminaBonus;
        public static ConfigEntry<float> Level50FoodDuration;
        public static ConfigEntry<float> Level50PhysicalDamageReduction;
        public static ConfigEntry<float> Level100PhysicalDamageReduction;
        public static ConfigEntry<float> Level100LifeBonus;
        public static ConfigEntry<float> Level150BonusArmor;
        public static ConfigEntry<float> Level150BonusStaminaAndLife;
        public static ConfigEntry<float> Level200BonusArmor;
        public static ConfigEntry<float> Level200PhysicalDamageReduction;

        public static int skillLevel = 1;

        public static void InitConfigs(ConfigFile config)
        {
            PassiveLifeBonus = config.Bind("Constitution Server config", "PassiveLifeBonus", 1.1f,
                    new ConfigDescription("PassiveLifeBonus", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            PassiveStaminaBonus = config.Bind("Constitution Server config", "PassiveStaminaBonus", 1.1f,
                    new ConfigDescription("PassiveStaminaBonus", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level50FoodDuration = config.Bind("Constitution Server config", "Level50FoodDuration", 1.2f,
                    new ConfigDescription("Level50FoodDuration", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level50PhysicalDamageReduction = config.Bind("Constitution Server config", "Level50PhysicalDamageReduction", 1.05f,
                     new ConfigDescription("Level50PhysicalDamageReduction", null, null,
                     new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100PhysicalDamageReduction = config.Bind("Constitution Server config", "Level100PhysicalDamageReduction", 1.05f,
                    new ConfigDescription("Level100PhysicalDamageReduction", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100LifeBonus = config.Bind("Constitution Server config", "Level100JumAndRunStaminaReduction", 15f,
                    new ConfigDescription("Level100LifeBonus", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150BonusArmor = config.Bind("Constitution Server config", "Level150BonusArmor", 1.1f,
                    new ConfigDescription("Level150BonusArmor", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150BonusStaminaAndLife = config.Bind("Constitution Server config", "Level150BonusStaminaAndLife", 15f,
                    new ConfigDescription("Level150BonusStaminaAndLife", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200BonusArmor = config.Bind("Constitution Server config", "Level200BonusArmor", 1.10f,
                    new ConfigDescription("Level200BonusArmor", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200PhysicalDamageReduction = config.Bind("Constitution Server config", "Level200PhysicalDamageReduction", 1.1f,
                    new ConfigDescription("Level200PhysicalDamageReduction", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }

        [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetArmor), typeof(int))]
        public static class ModifyArmor_ItemData_GetArmor_Patch
        {
            public static void Postfix(ItemDrop.ItemData __instance, ref float __result)
            {
                skillLevel = Level.GetSkillLevel(Skill.Constitution);

                if (skillLevel < 150) return;

                var armorMultiplier = Level150BonusArmor.Value;

                if (skillLevel >= 200) armorMultiplier += Level200BonusArmor.Value - 1;

                __result *= armorMultiplier;
            }
        }

        [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
        public static class RPC_Damage
        {
            public static void Prefix(Character __instance, HitData hit)
            {
                if (!(__instance is Player player))
                {
                    return;
                }

                skillLevel = Level.GetSkillLevel(Skill.Constitution);

                if (skillLevel < 50) return;

                float physicalDamageReduction = Level50PhysicalDamageReduction.Value - 1;

                if (skillLevel >= 100) physicalDamageReduction += Level100PhysicalDamageReduction.Value - 1;
                if (skillLevel >= 200) physicalDamageReduction += Level200PhysicalDamageReduction.Value - 1;

                physicalDamageReduction = 1 - physicalDamageReduction;

                hit.m_damage.m_blunt *= physicalDamageReduction;
                hit.m_damage.m_slash *= physicalDamageReduction;
                hit.m_damage.m_pierce *= physicalDamageReduction;
                hit.m_damage.m_chop *= physicalDamageReduction;
                hit.m_damage.m_pickaxe *= physicalDamageReduction;
            }
        }


        [HarmonyPatch(typeof(Player), nameof(Player.GetTotalFoodValue))]
        public static class GetTotalFoodValue
        {
            public static void Postfix(Player __instance, ref float hp, ref float stamina)
            {
                skillLevel = Level.GetSkillLevel(Skill.Constitution);
                if (skillLevel == 1) return;

                float lifeBonus = 1 + (skillLevel / 100f) * (PassiveLifeBonus.Value - 1);
                float staminaBonus = 1 + (skillLevel / 100f) * (Constitution.PassiveStaminaBonus.Value - 1);

                stamina *= staminaBonus;
                hp *= lifeBonus;

                if (skillLevel >= 100) hp += Level100LifeBonus.Value;
                if (skillLevel >= 150)
                {
                    hp += Level150BonusStaminaAndLife.Value;
                    stamina += Level150BonusStaminaAndLife.Value;
                }
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.GetBaseFoodHP))]
        public static class GetBaseFoodHP
        {
            public static void Postfix(Player __instance, ref float __result)
            {
                skillLevel = Level.GetSkillLevel(Skill.Constitution);
                if (skillLevel == 1) return;

                float lifeBonus = 1 + (skillLevel / 100f) * (PassiveLifeBonus.Value - 1);

                __result *= lifeBonus;
                if (skillLevel >= 100) __result += Level100LifeBonus.Value;
                if (skillLevel >= 150) __result += Level150BonusStaminaAndLife.Value;
            }
        }


        [HarmonyPatch(typeof(Player), nameof(Player.ConsumeItem))]
        public static class ConsumeItem
        {
            public static float oldFoodValue;
            private static void Prefix(Inventory inventory, ItemDrop.ItemData item)
            {
                Player localPlayer = Player.m_localPlayer;
                if (!localPlayer || localPlayer.IsDead() || (localPlayer.InCutscene() || localPlayer.IsTeleporting()))
                    return;

                skillLevel = Level.GetSkillLevel(Skill.Constitution);

                if (skillLevel < 50) return;

                if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Consumable && !ValheimLevelSystem.PotionToReduceCooldownNameList.Contains(item.m_dropPrefab.name))
                {
                    oldFoodValue = item.m_shared.m_foodBurnTime;
                    item.m_shared.m_foodBurnTime *= Level50FoodDuration.Value;
                };
            }

            private static void Postfix(Inventory inventory, ItemDrop.ItemData item)
            {
                Player localPlayer = Player.m_localPlayer;
                if (!localPlayer || localPlayer.IsDead() || (localPlayer.InCutscene() || localPlayer.IsTeleporting()))
                    return;

                skillLevel = Level.GetSkillLevel(Skill.Constitution);

                if (skillLevel < 50) return;

                if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Consumable && !ValheimLevelSystem.PotionToReduceCooldownNameList.Contains(item.m_dropPrefab.name))
                {
                    item.m_shared.m_foodBurnTime = oldFoodValue;
                };
            }
        }
    }
}