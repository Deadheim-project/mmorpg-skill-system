using BepInEx.Configuration;
using HarmonyLib;

namespace ValheimLevelSystem.PlayerSkills
{
    [HarmonyPatch]
    public class Agility
    {
        public static ConfigEntry<float> PassiveBowMultiplier;
        public static ConfigEntry<float> PassivePolearmMultipler;
        public static ConfigEntry<float> PassiveSpearMultiplier;
        public static ConfigEntry<float> PassiveKnifeMultiplier;
        public static ConfigEntry<float> Level50RunSpeed;
        public static ConfigEntry<float> Level50SpearDamage;
        public static ConfigEntry<float> Level50PolearmDamage;
        public static ConfigEntry<float> Level100JumAndRunStaminaReduction;
        public static ConfigEntry<float> Level100BowDamage;
        public static ConfigEntry<float> Level100KnivesDamage;
        public static ConfigEntry<float> Level150SpearDamage;
        public static ConfigEntry<float> Level150SPolearmDamage;
        public static ConfigEntry<float> Level150RunSpeed;
        public static ConfigEntry<float> Level200RunSpeed;
        public static ConfigEntry<float> Level200BowsDamage;
        public static ConfigEntry<float> Level200KnivesDamage;

        public static void InitConfigs(ConfigFile config)
        {
            PassiveBowMultiplier = config.Bind("Agility Server config", "PassiveBowMultiplier", 10f,
                    new ConfigDescription("PassiveBowMultiplier", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            PassivePolearmMultipler = config.Bind("Agility Server config", "PassivePolearmMultipler", 15f,
                    new ConfigDescription("PassivePolearmMultipler", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            PassiveKnifeMultiplier = config.Bind("Agility Server config", "PassiveKnifeMultiplier", 15f,
        new ConfigDescription("PassiveKnifeMultiplier", null, null,
        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            PassiveSpearMultiplier = config.Bind("Agility Server config", "PassiveSpearMultiplier", 15f,
        new ConfigDescription("PassiveSpearMultiplier", null, null,
        new ConfigurationManagerAttributes { IsAdminOnly = true }));


            Level50RunSpeed = config.Bind("Agility Server config", "Level50RunSpeed", 1.05f,
                    new ConfigDescription("Level50RunSpeed", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level50SpearDamage = config.Bind("Agility Server config", "Level50SpearDamage", 1.1f,
                     new ConfigDescription("Level50SpearDamage", null, null,
                     new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level50PolearmDamage = config.Bind("Agility Server config", "Level50PolearmDamage", 1.1f,
     new ConfigDescription("Level50PolearmDamage", null, null,
     new ConfigurationManagerAttributes { IsAdminOnly = true }));


            Level100BowDamage = config.Bind("Agility Server config", "Level100BowDamage", 1.1f,
                    new ConfigDescription("Level100BowDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100KnivesDamage = config.Bind("Agility Server config", "Level100KnivesDamage", 1.1f,
        new ConfigDescription("Level100KnivesDamage", null, null,
        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100JumAndRunStaminaReduction = config.Bind("Agility Server config", "Level100JumAndRunStaminaReduction", 1.1f,
                    new ConfigDescription("Level100JumAndRunStaminaReduction", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150SpearDamage = config.Bind("Agility Server config", "Level150SpearDamage", 1.2f,
                    new ConfigDescription("Level150SpearDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));


            Level150SPolearmDamage = config.Bind("Agility Server config", "Level150SPolearmDamage", 1.2f,
                    new ConfigDescription("Level150SPolearmDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));


            Level150RunSpeed = config.Bind("Agility Server config", "Level150RunSpeed", 1.12f,
                    new ConfigDescription("Level150RunSpeed", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200RunSpeed = config.Bind("Agility Server config", "Level200RunSpeed", 1.10f,
                    new ConfigDescription("Level200RunSpeed", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200BowsDamage = config.Bind("Agility Server config", "Level200BowsDamage", 1.2f,
                    new ConfigDescription("Level200BowsDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));


            Level200KnivesDamage = config.Bind("Agility Server config", "Level200KnivesDamage", 1.2f,
                    new ConfigDescription("Level200KnivesDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }

        [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetDamage), typeof(int))]
        public class GetDamage
        {
            public static void Postfix(ItemDrop.ItemData __instance, ref HitData.DamageTypes __result)
            {
                if (__instance is null) return;

                if (__instance.m_shared is null) return;

                int skillLevel = Level.GetSkillLevel(Skill.Agility);

                if (skillLevel < 1) return;

                if (Player.m_localPlayer is null) return;

                float spearMultiplier = ((skillLevel / 100f) * PassiveSpearMultiplier.Value) / 100f + 1;
                float bowMultiplier = ((skillLevel / 100f) * PassiveBowMultiplier.Value) / 100f + 1;
                float polearmMultiplier = ((skillLevel / 100f) * PassivePolearmMultipler.Value) / 100f + 1;
                float knifeMultiplier = ((skillLevel / 100f) * PassiveKnifeMultiplier.Value) / 100f + 1;

                if (skillLevel >= 50)
                {
                    spearMultiplier += Level50SpearDamage.Value - 1;
                    polearmMultiplier += Level50PolearmDamage.Value - 1;
                }

                if (skillLevel >= 100)
                {
                    bowMultiplier += Level100BowDamage.Value - 1;
                    knifeMultiplier += Level100KnivesDamage.Value - 1;
                }

                if (skillLevel >= 150)
                {
                    spearMultiplier += Level150SpearDamage.Value - 1;
                    polearmMultiplier += Level150SPolearmDamage.Value - 1;
                }

                if (skillLevel >= 200)
                {
                    knifeMultiplier += Level200BowsDamage.Value - 1;
                    polearmMultiplier += Level200KnivesDamage.Value - 1;
                }

                float multiplier = 1;

                if (__instance.m_shared.m_skillType == Skills.SkillType.Spears) multiplier = spearMultiplier;
                if (__instance.m_shared.m_skillType == Skills.SkillType.Bows) multiplier = bowMultiplier;
                if (__instance.m_shared.m_skillType == Skills.SkillType.Polearms) multiplier = polearmMultiplier;
                if (__instance.m_shared.m_skillType == Skills.SkillType.Knives) multiplier = knifeMultiplier;

                __result.m_blunt *= multiplier;
                __result.m_slash *= multiplier;
                __result.m_pierce *= multiplier;
                __result.m_chop *= multiplier;
                __result.m_pickaxe *= multiplier;
            }
        }

        [HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyJumpStaminaUsage))]
        public static class ModifyJumpStaminaUse_SEMan_ModifyJumpStaminaUsage_Patch
        {
            public static void Postfix(SEMan __instance, ref float staminaUse)
            {
                int skillLevel = Level.GetSkillLevel(Skill.Agility);

                if (skillLevel < 100) return;

                if (__instance.m_character.IsPlayer())
                {
                    staminaUse *= 1 - (Level100JumAndRunStaminaReduction.Value - 1);
                }
            }
        }

        [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetHoldStaminaDrain))]
        public static class GetHoldStaminaDrain
        {
            public static void Postfix(float __result)
            {
                int skillLevel = Level.GetSkillLevel(Skill.Agility);

                if (skillLevel < 200) return;

                __result = 0;
            }
        }


        [HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyRunStaminaDrain))]
        public static class ModifySprintStaminaUse_SEMan_ModifyRunStaminaDrain_Patch
        {
            public static void Postfix(SEMan __instance, float baseDrain, ref float drain)
            {
                int skillLevel = Level.GetSkillLevel(Skill.Agility);

                if (skillLevel < 100) return;

                if (__instance.m_character.IsPlayer())
                {
                    drain *= 1 - (Level100JumAndRunStaminaReduction.Value - 1);
                }
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.UpdateMovementModifier))]
        public static class UpdateMovementModifier
        {
            public static void Postfix(Player __instance)
            {
                int skillLevel = Level.GetSkillLevel(Skill.Agility);

                if (skillLevel < 50) return;

                float movespeedBonus = Level50RunSpeed.Value - 1;

                if (skillLevel >= 150) movespeedBonus += Level150RunSpeed.Value - 1;
                if (skillLevel >= 200) movespeedBonus += Level200RunSpeed.Value - 1;

                __instance.m_equipmentMovementModifier += movespeedBonus;
            }
        }

        public static void UpdateStatusEffect()
        {
            //int skillLevel = Level.GetSkillLevel(Skill.Agility);

            //SE_Stats stats = (SE_Stats)Player.m_localPlayer.m_seman.m_statusEffects.Find(x => x.m_name == "vls_agility");
            //if (stats != null)
            //{
            //    Player.m_localPlayer.m_seman.m_statusEffects.Remove(stats);
            //}        

            //if (skillLevel < 50) return;

            //float backstabBonus = Level50Backstab.Value;
            //if (skillLevel >= 200) backstabBonus += Level200Backstab.Value - 1;

            //SE_Stats effect = (SE_Stats)Player.m_localPlayer.m_seman.AddStatusEffect("vls_agility");

        }
    }
}
