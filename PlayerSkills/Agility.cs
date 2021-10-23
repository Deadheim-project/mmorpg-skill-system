using HarmonyLib;

namespace MMRPGSkillSystem.PlayerSkills
{
    [HarmonyPatch]
    public class Agility
    {
        public static int PassiveBowPolearmMultiplier = 10;
        public static int PassiveSpearKnifeMultiplier = 15;

        [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetDamage), typeof(int))]
        public class GetDamage
        {
            public static void Postfix(ItemDrop.ItemData __instance, ref HitData.DamageTypes __result)
            {
                int skillLevel = Level.GetSkillLevel(Skill.Agility);

                if (skillLevel < 100) return;

                var bowKnivesMultiplier = 1.10f;

                if (skillLevel >= 200) bowKnivesMultiplier += 0.2f;

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
                        __result.m_blunt *= 1.20f;
                        __result.m_slash *= 1.20f;
                        __result.m_pierce *= 1.20f;
                        __result.m_chop *= 1.20f;
                        __result.m_pickaxe *= 1.20f;
                        __result.m_fire *= 1.20f;
                        __result.m_frost *= 1.20f;
                        __result.m_lightning *= 1.20f;
                        __result.m_poison *= 1.20f;
                        __result.m_spirit *= 1.20f;
                    }
                }

                ApplyPassiveMultiplier(__instance, ref __result, skillLevel);
            }
        }

        private static void ApplyPassiveMultiplier(ItemDrop.ItemData __instance, ref HitData.DamageTypes __result, int skillLevel)
        {
            float bowPolearmMultiplier = ((skillLevel / 100) * PassiveBowPolearmMultiplier) / 1000 + 1;
            float spearKnifeMultiplier = ((skillLevel / 100) * PassiveSpearKnifeMultiplier) / 1000 + 1;

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
                    staminaUse *= 1 - 0.1f;
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
                    drain *= 1 - 0.1f;
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

                float movespeedBonus = 0.05f;   

                if (skillLevel >= 150) movespeedBonus += 0.12f;
                if (skillLevel >= 200) movespeedBonus += 0.10f;

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

                float backstabBonus = 1.1f;                
                if (skillLevel >= 200) backstabBonus += 0.2f;

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
