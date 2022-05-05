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
        public static ConfigEntry<float> PassiveAxeMultiplier;
        public static ConfigEntry<float> PassiveSwordMultiplier;
        public static ConfigEntry<float> PassiveMaceMultiplier;
        public static ConfigEntry<float> PassiveChopAndPickaxeMultiplier;
        public static ConfigEntry<float> Level50AxeMultiplier;
        public static ConfigEntry<float> Level50SwordMultiplier;
        public static ConfigEntry<float> Level50MaceMultiplier;
        public static ConfigEntry<float> Level50ParryWindowBonus;
        public static ConfigEntry<float> Level100AxeMultiplier;
        public static ConfigEntry<float> Level100SwordMultiplier;
        public static ConfigEntry<float> Level100MaceMultiplier;
        public static ConfigEntry<float> Level100ReduceAttackStamina;
        public static ConfigEntry<float> Level150AxeMultiplier;
        public static ConfigEntry<float> Level150SwordMultiplier;
        public static ConfigEntry<float> Level150MaceMultiplier;
        public static ConfigEntry<float> Level150ParryWindowBonus;
        public static ConfigEntry<float> Level150ReduceAttackStamina;
        public static ConfigEntry<float> Level200ReduceAttackStamina;
        public static ConfigEntry<float> Level200AxeMultiplier;
        public static ConfigEntry<float> Level200SwordMultiplier;
        public static ConfigEntry<float> Level200MaceMultiplier;

        public static void InitConfigs(ConfigFile config)
        {
            PassiveAxeMultiplier = config.Bind("Strength Server config", "PassiveAxeMultiplier", 10f,
                    new ConfigDescription("PassiveAxeMultiplier", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            PassiveSwordMultiplier = config.Bind("Strength Server config", "PassiveSwordMultiplier", 10f,
                 new ConfigDescription("PassiveSwordMultiplier", null, null,
                 new ConfigurationManagerAttributes { IsAdminOnly = true }));

            PassiveMaceMultiplier = config.Bind("Strength Server config", "PassiveMaceMultiplier", 10f,
        new ConfigDescription("PassiveMaceMultiplier", null, null,
        new ConfigurationManagerAttributes { IsAdminOnly = true }));


            PassiveChopAndPickaxeMultiplier = config.Bind("Strength Server config", "PassiveChopAndPickaxeMultiplier", 25f,
                    new ConfigDescription("PassiveChopAndPickaxeMultiplier", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level50AxeMultiplier = config.Bind("Strength Server config", "Level50AxeMultiplier", 1.10f,
                    new ConfigDescription("Level50AxeMultiplier", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level50MaceMultiplier = config.Bind("Strength Server config", "Level50MaceMultiplier", 1.10f,
        new ConfigDescription("Level50MaceMultiplier", null, null,
        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level50SwordMultiplier = config.Bind("Strength Server config", "Level50SwordMultiplier", 1.10f,
        new ConfigDescription("Level50SwordMultiplier", null, null,
        new ConfigurationManagerAttributes { IsAdminOnly = true }));


            Level50ParryWindowBonus = config.Bind("Strength Server config", "Level50ParryWindowBonus", 0.1f,
        new ConfigDescription("Level50ParryWindowBonus", null, null,
        new ConfigurationManagerAttributes { IsAdminOnly = true })); ;


            Level100AxeMultiplier = config.Bind("Strength Server config", "Level100AxeMultiplier", 1.10f,
                    new ConfigDescription("Level100AxeMultiplier", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100MaceMultiplier = config.Bind("Strength Server config", "Level100MaceMultiplier", 1.10f,
        new ConfigDescription("Level100MaceMultiplier", null, null,
        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100SwordMultiplier = config.Bind("Strength Server config", "Level100SwordMultiplier", 1.10f,
        new ConfigDescription("Level100SwordMultiplier", null, null,
        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100ReduceAttackStamina = config.Bind("Strength Server config", "Level100ReduceAttackStamina", 1.1f,
                    new ConfigDescription("Level100ReduceAttackStamina", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));


            Level150ParryWindowBonus = config.Bind("Strength Server config", "Level150ParryWindowBonus", 0.15f,
                    new ConfigDescription("Level150ParryWindowBonus", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150AxeMultiplier = config.Bind("Strength Server config", "Level150AxeMultiplier", 1.2f,
                    new ConfigDescription("Level150AxeMultiplier", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150SwordMultiplier = config.Bind("Strength Server config", "Level150SwordMultiplier", 1.2f,
        new ConfigDescription("Level150SwordMultiplier", null, null,
        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150MaceMultiplier = config.Bind("Strength Server config", "Level150MaceMultiplier", 1.2f,
        new ConfigDescription("Level150MaceMultiplier", null, null,
        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150ReduceAttackStamina = config.Bind("Strength Server config", "Level150ReduceAttackStamina", 1.1f,
                    new ConfigDescription("Level150ReduceAttackStamina", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200ReduceAttackStamina = config.Bind("Strength Server config", "Level200ReduceAttackStamina", 1.15f,
                    new ConfigDescription("Level200ReduceAttackStamina", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200AxeMultiplier = config.Bind("Strength Server config", "Level200AxeMultiplier", 1.2f,
                    new ConfigDescription("Level200AxeMultiplier", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200SwordMultiplier = config.Bind("Strength Server config", "Level200SwordMultiplier", 1.2f,
        new ConfigDescription("Level200SwordMultiplier", null, null,
        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200MaceMultiplier = config.Bind("Strength Server config", "Level200MaceMultiplier", 1.2f,
        new ConfigDescription("Level200MaceMultiplier", null, null,
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

                float axeMultiplier = ((skillLevel / 100f) * PassiveAxeMultiplier.Value) / 100f + 1f;
                float swordMultiplier = ((skillLevel / 100f) * PassiveSwordMultiplier.Value) / 100f + 1f;
                float maceMultiplier = ((skillLevel / 100f) * PassiveMaceMultiplier.Value) / 100f + 1f;
                float chopAndPickaxeMultiplier = ((skillLevel / 100f) * PassiveChopAndPickaxeMultiplier.Value) / 100 + 1f;

                if (skillLevel >= 50)
                {
                    axeMultiplier += Level50AxeMultiplier.Value - 1;
                    swordMultiplier += Level50AxeMultiplier.Value - 1;
                    maceMultiplier += Level50AxeMultiplier.Value - 1;
                }

                if (skillLevel >= 100)
                {
                    axeMultiplier += Level100AxeMultiplier.Value - 1;
                    swordMultiplier += Level100AxeMultiplier.Value - 1;
                    maceMultiplier += Level100AxeMultiplier.Value - 1;
                }

                if (skillLevel >= 150)
                {
                    axeMultiplier += Level150AxeMultiplier.Value - 1;
                    swordMultiplier += Level50AxeMultiplier.Value - 1;
                    maceMultiplier += Level50AxeMultiplier.Value - 1;
                }


                if (skillLevel >= 200)
                {
                    axeMultiplier += Level200AxeMultiplier.Value - 1;
                    swordMultiplier += Level50AxeMultiplier.Value - 1;
                    maceMultiplier += Level50AxeMultiplier.Value - 1;
                }

                float multiplier = 1;

                if (__instance.m_shared.m_skillType == Skills.SkillType.Clubs) multiplier = maceMultiplier;
                if (__instance.m_shared.m_skillType == Skills.SkillType.Swords) multiplier = swordMultiplier;
                if (__instance.m_shared.m_skillType == Skills.SkillType.Axes) multiplier = axeMultiplier;

                __result.m_blunt *= multiplier;
                __result.m_slash *= multiplier;
                __result.m_pierce *= multiplier;
                __result.m_chop *= chopAndPickaxeMultiplier;
                __result.m_pickaxe *= chopAndPickaxeMultiplier;
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
