using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace ValheimLevelSystem.PlayerSkills
{
    [HarmonyPatch]
    public class Strength
    {
        public static ConfigEntry<float> PassiveTwoHandedMultiplier;
        public static ConfigEntry<float> PassiveOneHandedMultiplier;
        public static ConfigEntry<float> PassiveChopAndPickaxeMultiplier;
        public static ConfigEntry<float> Level50OneHandedDamage;
        public static ConfigEntry<float> Level50ParryWindowBonus;
        public static ConfigEntry<float> Level100TwoHandedDamage;
        public static ConfigEntry<float> Level100ReduceAttackStamina;
        public static ConfigEntry<float> Level150OneHandedDamage;
        public static ConfigEntry<float> Level150ParryWindowBonus;
        public static ConfigEntry<float> Level150ReduceAttackStamina;
        public static ConfigEntry<float> Level200ReduceAttackStamina;
        public static ConfigEntry<float> Level200OneTwoHandedDamage;

        public static void InitConfigs(ConfigFile config)
        {
            PassiveTwoHandedMultiplier = config.Bind("Strength Server config", "PassiveTwoHandedMultiplier", 10f,
                    new ConfigDescription("PassiveTwoHandedMultiplier", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            PassiveOneHandedMultiplier = config.Bind("Strength Server config", "PassiveOneHandedMultiplier", 10f,
                    new ConfigDescription("PassiveOneHandedMultiplier", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            PassiveChopAndPickaxeMultiplier = config.Bind("Strength Server config", "PassiveChopAndPickaxeMultiplier", 25f,
                    new ConfigDescription("PassiveChopAndPickaxeMultiplier", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));


            Level50OneHandedDamage = config.Bind("Strength Server config", "Level50OneHandedDamage", 1.10f,
                    new ConfigDescription("Level50OneHandedDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level50ParryWindowBonus = config.Bind("Strength Server config", "Level50ParryWindowBonus", 0.1f,
                    new ConfigDescription("Level50ParryWindowBonus", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true })); ;

            Level100TwoHandedDamage = config.Bind("Strength Server config", "Level100TwoHandedDamage", 1.2f,
                    new ConfigDescription("Level100TwoHandedDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100ReduceAttackStamina = config.Bind("Strength Server config", "Level100ReduceAttackStamina", 1.1f,
                    new ConfigDescription("Level100ReduceAttackStamina", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150ParryWindowBonus = config.Bind("Strength Server config", "Level150ParryWindowBonus", 0.15f,
                    new ConfigDescription("Level150ParryWindowBonus", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150OneHandedDamage = config.Bind("Strength Server config", "Level150OneHandedDamage", 1.2f,
                    new ConfigDescription("Level150OneHandedDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150ReduceAttackStamina = config.Bind("Strength Server config", "Level150ReduceAttackStamina", 1.1f,
                    new ConfigDescription("Level150ReduceAttackStamina", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200ReduceAttackStamina = config.Bind("Strength Server config", "Level200ReduceAttackStamina", 1.15f,
                    new ConfigDescription("Level200ReduceAttackStamina", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200OneTwoHandedDamage = config.Bind("Strength Server config", "Level200OneTwoHandedDamage", 1.15f,
                    new ConfigDescription("Level200OneTwoHandedDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }

        [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.BlockAttack))]
        private static class PàtchBlockAttack
        {
            private static readonly MethodInfo getParryWindowBonus = AccessTools.DeclaredMethod(typeof(PàtchBlockAttack), nameof(GetParryWindowBonus));

            [UsedImplicitly]
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (CodeInstruction instruction in instructions)
                {
                    if (instruction.opcode == OpCodes.Ldc_R4 && instruction.OperandIs(Humanoid.m_perfectBlockInterval))
                    {
                        yield return new CodeInstruction(OpCodes.Call, getParryWindowBonus);
                    }
                    else
                    {
                        yield return instruction;
                    }
                }
            }

            private static float GetParryWindowBonus()
            {
                int skillLevel = Level.GetSkillLevel(Skill.Strength);

                float timeToAmplify = 0f;

                if (skillLevel >= 50) timeToAmplify = Level50ParryWindowBonus.Value;
                if (skillLevel >= 100) timeToAmplify += Level150ParryWindowBonus.Value;

                return Humanoid.m_perfectBlockInterval + timeToAmplify;
            }
        }

        [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetDamage), typeof(int))]
        public class GetDamage
        {
            public static void Postfix(ItemDrop.ItemData __instance, ref HitData.DamageTypes __result)
            {
                int skillLevel = Level.GetSkillLevel(Skill.Strength);

                if (skillLevel == 1) return;

                float twoHandedMultiplier = ((skillLevel / 100f) * PassiveTwoHandedMultiplier.Value) / 100f + 1f;
                float oneHandedMultiplier = ((skillLevel / 100f) * PassiveOneHandedMultiplier.Value) / 100f + 1f;
                float chopAndPickaxeMultiplier = ((skillLevel / 100f) * PassiveChopAndPickaxeMultiplier.Value) / 100 + 1f;

                if (skillLevel >= 50) oneHandedMultiplier += (Level50OneHandedDamage.Value - 1);
                if (skillLevel >= 150) oneHandedMultiplier += (Level150OneHandedDamage.Value - 1);
                if (skillLevel >= 100) twoHandedMultiplier += (Level100TwoHandedDamage.Value - 1);

                if (skillLevel >= 200)
                {
                    oneHandedMultiplier += (Level200OneTwoHandedDamage.Value - 1);
                    twoHandedMultiplier += (Level200OneTwoHandedDamage.Value - 1);
                }

                if (__instance.m_shared.m_itemType == ItemDrop.ItemData.ItemType.TwoHandedWeapon)
                {
                    __result.m_blunt *= twoHandedMultiplier;
                    __result.m_slash *= twoHandedMultiplier;
                    __result.m_pierce *= twoHandedMultiplier;
                    __result.m_chop *= chopAndPickaxeMultiplier;
                    __result.m_pickaxe *= chopAndPickaxeMultiplier;
                    __result.m_fire *= twoHandedMultiplier;
                    __result.m_frost *= twoHandedMultiplier;
                    __result.m_lightning *= twoHandedMultiplier;
                    __result.m_poison *= twoHandedMultiplier;
                    __result.m_spirit *= twoHandedMultiplier;
                }


                if (__instance.m_shared.m_itemType == ItemDrop.ItemData.ItemType.OneHandedWeapon)
                {
                    __result.m_blunt *= oneHandedMultiplier;
                    __result.m_slash *= oneHandedMultiplier;
                    __result.m_pierce *= oneHandedMultiplier;
                    __result.m_chop *= chopAndPickaxeMultiplier;
                    __result.m_pickaxe *= chopAndPickaxeMultiplier;
                    __result.m_fire *= oneHandedMultiplier;
                    __result.m_frost *= oneHandedMultiplier;
                    __result.m_lightning *= oneHandedMultiplier;
                    __result.m_poison *= oneHandedMultiplier;
                    __result.m_spirit *= oneHandedMultiplier;
                }
            }
        }

        [HarmonyPatch(typeof(Attack), nameof(Attack.GetAttackStamina))]
        public class GetAttackStamina
        {
            public static void Postfix(Attack __instance, ref float __result)
            {
                if (__instance.m_character is Player player)
                {
                    int skillLevel = Level.GetSkillLevel(Skill.Strength);

                    if (skillLevel >= 100)
                    {
                        float bonus = Level100ReduceAttackStamina.Value;
                        if (skillLevel >= 150) bonus += Level150ReduceAttackStamina.Value - 1;
                        if (skillLevel >= 200) bonus += Level200ReduceAttackStamina.Value - 1;
                        __result *= 1 - (bonus - 1);
                    }
                }
            }
        }
    }
}
