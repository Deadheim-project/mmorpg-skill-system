using BepInEx.Configuration;
using HarmonyLib;

namespace MMRPGSkillSystem.PlayerSkills
{
    [HarmonyPatch]
    public class Agility
    {
        public static ConfigEntry<float> PassiveBowPolearmMultiplier;
        public static ConfigEntry<float> PassiveSpearKnifeMultiplier;
        public static ConfigEntry<float> Level50RunSpeed;
        public static ConfigEntry<float> Level50Backstab;
        public static ConfigEntry<float> Level100JumAndRunStaminaReduction;
        public static ConfigEntry<float> Level100BowKnivesDamage;
        public static ConfigEntry<float> Level150SpearAndPolearmDamage;
        public static ConfigEntry<float> Level150RunSpeed;
        public static ConfigEntry<float> Level200RunSpeed;
        public static ConfigEntry<float> Level200Backstab;
        public static ConfigEntry<float> Level200BowKnivesDamage;

        public static void InitConfigs(ConfigFile config)
        {
            PassiveBowPolearmMultiplier = config.Bind("Agility Server config", "PassiveBowPolearmMultiplier", 10f,
                    new ConfigDescription("PassiveBowPolearmMultiplier", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            PassiveSpearKnifeMultiplier = config.Bind("Agility Server config", "PassiveSpearKnifeMultiplier", 15f,
                    new ConfigDescription("PassiveMPassiveSpearKnifeMultiplieragicDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level50RunSpeed = config.Bind("Agility Server config", "Level50RunSpeed", 1.05f,
                    new ConfigDescription("Level50RunSpeed", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level50Backstab = config.Bind("Agility Server config", "Level50Backstab", 1.15f,
                     new ConfigDescription("Level50Backstab", null, null,
                     new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100BowKnivesDamage = config.Bind("Agility Server config", "Level100BowKnivesDamage", 1.1f,
                    new ConfigDescription("Level100BowKnivesDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100JumAndRunStaminaReduction = config.Bind("Agility Server config", "Level100JumAndRunStaminaReduction", 1.1f,
                    new ConfigDescription("Level100JumAndRunStaminaReduction", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150SpearAndPolearmDamage = config.Bind("Agility Server config", "Level150SpearAndPolearmDamage", 1.2f,
                    new ConfigDescription("Level150SpearAndPolearmDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150RunSpeed = config.Bind("Agility Server config", "Level150RunSpeed", 1.12f,
                    new ConfigDescription("Level150RunSpeed", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200RunSpeed = config.Bind("Agility Server config", "Level200RunSpeed", 1.10f,
                    new ConfigDescription("Level200RunSpeed", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200BowKnivesDamage = config.Bind("Agility Server config", "Level200BowKnivesDamage", 1.2f,
                    new ConfigDescription("Level200BowKnivesDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200Backstab = config.Bind("Agility Server config", "Level200Backstab", 1.20f,
                    new ConfigDescription("Level200Backstab", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }

        [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetDamage), typeof(int))]
        public class GetDamage
        {
            public static void Postfix(ItemDrop.ItemData __instance, ref HitData.DamageTypes __result)
            {
                int skillLevel = Level.GetSkillLevel(Skill.Agility);

                if (skillLevel < 1) return;

                float spearKnifeMultiplier = ((skillLevel / 100f) * PassiveSpearKnifeMultiplier.Value) / 100f + 1;
                float bowPolearmMultiplier = ((skillLevel / 100f) * PassiveBowPolearmMultiplier.Value) / 100f + 1;

                if (__instance.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Bow || __instance.m_shared.m_skillType == Skills.SkillType.Polearms)
                {
                    __result.m_blunt *= bowPolearmMultiplier;
                    __result.m_slash *= bowPolearmMultiplier;
                    __result.m_pierce *= bowPolearmMultiplier;
                    __result.m_chop *= bowPolearmMultiplier;
                    __result.m_pickaxe *= bowPolearmMultiplier;
                    __result.m_fire *= bowPolearmMultiplier;
                    __result.m_frost *= bowPolearmMultiplier;
                    __result.m_lightning *= bowPolearmMultiplier;
                    __result.m_poison *= bowPolearmMultiplier;
                    __result.m_spirit *= bowPolearmMultiplier;
                }

                if (__instance.m_shared.m_skillType == Skills.SkillType.Polearms || __instance.m_shared.m_skillType == Skills.SkillType.Knives)
                {
                    __result.m_blunt *= spearKnifeMultiplier;
                    __result.m_slash *= spearKnifeMultiplier;
                    __result.m_pierce *= spearKnifeMultiplier;
                    __result.m_chop *= spearKnifeMultiplier;
                    __result.m_pickaxe *= spearKnifeMultiplier;
                    __result.m_fire *= spearKnifeMultiplier;
                    __result.m_frost *= spearKnifeMultiplier;
                    __result.m_lightning *= spearKnifeMultiplier;
                    __result.m_poison *= spearKnifeMultiplier;
                    __result.m_spirit *= spearKnifeMultiplier;
                }

                if (skillLevel < 100) return;

                var bowKnivesMultiplier = Level100BowKnivesDamage.Value;

                if (skillLevel >= 200) bowKnivesMultiplier += Level200BowKnivesDamage.Value - 1;

                if (__instance.m_shared.m_skillType == Skills.SkillType.Bows || __instance.m_shared.m_skillType == Skills.SkillType.Knives)
                {
                    __result.m_blunt *= bowKnivesMultiplier;
                    __result.m_slash *= bowKnivesMultiplier;
                    __result.m_pierce *= bowKnivesMultiplier;
                    __result.m_chop *= bowKnivesMultiplier;
                    __result.m_pickaxe *= bowKnivesMultiplier;
                    __result.m_fire *= bowKnivesMultiplier;
                    __result.m_frost *= bowKnivesMultiplier;
                    __result.m_lightning *= bowKnivesMultiplier;
                    __result.m_poison *= bowKnivesMultiplier;
                    __result.m_spirit *= bowKnivesMultiplier;
                }

                if (skillLevel >= 150)
                {
                    if (__instance.m_shared.m_skillType == Skills.SkillType.Spears || __instance.m_shared.m_skillType == Skills.SkillType.Polearms)
                    {
                        __result.m_blunt *= Level150SpearAndPolearmDamage.Value;
                        __result.m_slash *= Level150SpearAndPolearmDamage.Value;
                        __result.m_pierce *= Level150SpearAndPolearmDamage.Value;
                        __result.m_chop *= Level150SpearAndPolearmDamage.Value;
                        __result.m_pickaxe *= Level150SpearAndPolearmDamage.Value;
                        __result.m_fire *= Level150SpearAndPolearmDamage.Value;
                        __result.m_frost *= Level150SpearAndPolearmDamage.Value;
                        __result.m_lightning *= Level150SpearAndPolearmDamage.Value;
                        __result.m_poison *= Level150SpearAndPolearmDamage.Value;
                        __result.m_spirit *= Level150SpearAndPolearmDamage.Value;
                    }
                }  
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

        [HarmonyPatch(typeof(Attack), nameof(Attack.DoAreaAttack))]
        public static class DoAreaAttack
        {
            private static bool Prefix(Attack __instance) { return ModifyBackstabPatchHelper.DoPrefix(__instance); }
            private static void Postfix(Attack __instance) { ModifyBackstabPatchHelper.DoPostfix(__instance); }
        }

        [HarmonyPatch(typeof(Attack), nameof(Attack.DoMeleeAttack))]
        public static class DoMeleeAttack
        {
            private static bool Prefix(Attack __instance) { return ModifyBackstabPatchHelper.DoPrefix(__instance); }
            private static void Postfix(Attack __instance) { ModifyBackstabPatchHelper.DoPostfix(__instance); }
        }

        [HarmonyPatch(typeof(Attack), nameof(Attack.FireProjectileBurst))]
        public static class FireProjectileBurst
        {
            private static bool Prefix(Attack __instance) { return ModifyBackstabPatchHelper.DoPrefix(__instance); }
            private static void Postfix(Attack __instance) { ModifyBackstabPatchHelper.DoPostfix(__instance); }
        }

        public static class ModifyBackstabPatchHelper
        {
            public static bool Override;
            public static float OriginalValue;

            public static bool DoPrefix(Attack __instance)
            {
                int skillLevel = Level.GetSkillLevel(Skill.Agility);

                if (skillLevel < 50) return true;

                float backstabBonus = Level50Backstab.Value;                
                if (skillLevel >= 200) backstabBonus += Level200Backstab.Value - 1;

                Override = false;
                OriginalValue = -1;

                var weapon = __instance.m_weapon;
                if (weapon == null)
                {
                    return true;
                }

                if (__instance.m_character is Player)
                {
                    Override = true;
                    OriginalValue = weapon.m_shared.m_backstabBonus;

                    weapon.m_shared.m_backstabBonus *= (backstabBonus);
                }

                return true;
            }

            public static void DoPostfix(Attack __instance)
            {
                int skillLevel = Level.GetSkillLevel(Skill.Agility);

                if (skillLevel < 50) return;

                var weapon = __instance.m_weapon;
                if (weapon != null && Override)
                {
                    weapon.m_shared.m_backstabBonus = OriginalValue;
                }

                Override = false;
                OriginalValue = -1;
            }
        }
    }
}
