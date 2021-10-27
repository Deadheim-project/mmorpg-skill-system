using BepInEx.Configuration;
using HarmonyLib;

namespace MMRPGSkillSystem.PlayerSkills
{
    public class Focus
    {
        public static ConfigEntry<float> PassiveLifeRegen;
        public static ConfigEntry<float> PassiveStaminaRegen;
        public static ConfigEntry<float> Level50StaminaRegen;
        public static ConfigEntry<float> Level50ElementalReduction;
        public static ConfigEntry<float> Level100LifeRegen;
        public static ConfigEntry<float> Level100StaminaBonus;
        public static ConfigEntry<float> Level150BuffAndPotionsDuration;
        public static ConfigEntry<float> Level150LifeBonus;
        public static ConfigEntry<float> Level200LifeAndStaminaRegen;
        public static ConfigEntry<float> Level200ElementalReduction;

        public static int skillLevel = 1;

        public static void InitConfigs(ConfigFile config)
        {
            PassiveLifeRegen = config.Bind("Focus Server config", "PassiveLifeRegen", 1.05f,
                    new ConfigDescription("PassiveLifeRegen", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            PassiveStaminaRegen = config.Bind("Focus Server config", "PassiveStaminaRegen", 1.05f,
                    new ConfigDescription("PassiveStaminaRegen", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level50StaminaRegen = config.Bind("Focus Server config", "Level50StaminaRegen", 1.1f,
                    new ConfigDescription("Level50StaminaRegen", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level50ElementalReduction = config.Bind("Focus Server config", "Level50ElementalReduction", 1.05f,
                    new ConfigDescription("Level50ElementalReduction", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100LifeRegen = config.Bind("Focus Server config", "Level100LifeRegen", 1.1f,
                    new ConfigDescription("Level50CarLevel100LifeRegenryWeight", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100StaminaBonus = config.Bind("Focus Server config", "Level100StaminaBonus", 10f,
                    new ConfigDescription("Level100StaminaBonus", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150BuffAndPotionsDuration = config.Bind("Focus Server config", "Level150BuffAndPotionsDuration", 1.2f,
                    new ConfigDescription("Level150BuffAndPotionsDuration", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150LifeBonus = config.Bind("Focus Server config", "Level150LifeBonus", 10f,
                    new ConfigDescription("Level150LifeBonus", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200LifeAndStaminaRegen = config.Bind("Focus Server config", "Level200LifeAndStaminaRegen", 1.1f,
                    new ConfigDescription("Level200LifeAndStaminaRegen", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200ElementalReduction = config.Bind("Focus Server config", "Level200ElementalReduction", 1.15f,
                    new ConfigDescription("Level200ElementalReduction", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }

        [HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyStaminaRegen))]
        public static class ModifyStaminaRegen_SEMan_ModifyStaminaRegen_Patch
        {
            public static void Postfix(SEMan __instance, ref float staminaMultiplier)
            {
                if (__instance.m_character.IsPlayer())
                {
                    skillLevel = Level.GetSkillLevel(Skill.Focus);
                    var regenValue = (skillLevel / 100f) * (PassiveStaminaRegen.Value - 1);

                    if (skillLevel >= 50) regenValue += Level50StaminaRegen.Value - 1;
                    if (skillLevel >= 200) regenValue += Level200LifeAndStaminaRegen.Value - 1;

                    staminaMultiplier += regenValue;
                }
            }
        }

        [HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyHealthRegen))]
        public static class ModifyHealthRegen_SEMan_ModifyHealthRegen_Patch
        {
            public static void Postfix(SEMan __instance, ref float regenMultiplier)
            {
                if (__instance.m_character.IsPlayer())
                {
                    skillLevel = Level.GetSkillLevel(Skill.Focus);

                    var regenValue = (skillLevel / 100f) * (PassiveLifeRegen.Value - 1);

                    if (skillLevel >= 100) regenValue += Level100LifeRegen.Value - 1;
                    if (skillLevel >= 200) regenValue += Level200LifeAndStaminaRegen.Value - 1;

                    regenMultiplier += regenValue;
                }
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

                skillLevel = Level.GetSkillLevel(Skill.Focus);

                if (skillLevel < 50) return;

                float elementalReduction = Level50ElementalReduction.Value;

                if (skillLevel >= 100) elementalReduction += Level200ElementalReduction.Value - 1;

                hit.m_damage.m_fire *= elementalReduction;
                hit.m_damage.m_frost *= elementalReduction;
                hit.m_damage.m_lightning *= elementalReduction;
                hit.m_damage.m_poison *= elementalReduction;
                hit.m_damage.m_spirit *= elementalReduction;
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.ConsumeItem))]
        public static class ConsumeItem
        {
            private static void Prefix(Inventory inventory, ItemDrop.ItemData item)
            {
                Player localPlayer = Player.m_localPlayer;
                if (!localPlayer || localPlayer.IsDead() || (localPlayer.InCutscene() || localPlayer.IsTeleporting()))
                    return;

                skillLevel = Level.GetSkillLevel(Skill.Focus);

                if (skillLevel < 150) return;

                if (ValheimLevelSystem.PotionToIncreaseTimeNameList.Contains(item.m_dropPrefab.name))
                {
                    float ttl = item.m_shared.m_consumeStatusEffect.m_ttl;
                    item.m_shared.m_consumeStatusEffect.m_ttl = ttl + (ttl / 100f * Level150BuffAndPotionsDuration.Value);
                };
            }
        }

        [HarmonyPatch(typeof(Player), "SetGuardianPower")]
        public static class SetGuardianPower
        {
            private static void Postfix(ref Player __instance)
            {
                if (skillLevel < 150) return;

                if (__instance.m_guardianSE)
                {
                    __instance.m_guardianSE.m_ttl = __instance.m_guardianSE.m_ttl + (__instance.m_guardianSE.m_ttl / 100 * Level150BuffAndPotionsDuration.Value);
                }
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.GetTotalFoodValue))]
        public static class GetTotalFoodValue
        {
            public static void Postfix(Player __instance, ref float hp, ref float stamina)
            {
                skillLevel = Level.GetSkillLevel(Skill.Focus);
                if (skillLevel < 100) return;

                stamina += Level100StaminaBonus.Value;

                if (skillLevel < 150) return;

                hp += Level150LifeBonus.Value;
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.GetBaseFoodHP))]
        public static class GetBaseFoodHP
        {
            public static void Postfix(Player __instance, ref float __result)
            {
                skillLevel = Level.GetSkillLevel(Skill.Focus);

                if (skillLevel < 150) return;

                __result += Level150LifeBonus.Value;
            }
        }
    }
}