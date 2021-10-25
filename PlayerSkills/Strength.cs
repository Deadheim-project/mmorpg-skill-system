using BepInEx.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MMRPGSkillSystem.PlayerSkills
{
    [HarmonyPatch]
    public class Strength
    {
        public static ConfigEntry<float> PassiveTwoHandedMultiplier;
        public static ConfigEntry<float> PassiveOneHandedMultiplier;
        public static ConfigEntry<float> PassiveChopAndPickaxeMultiplier;
        public static ConfigEntry<float> Level50OneHandedDamage;
        public static ConfigEntry<float> Level50CarryWeight;
        public static ConfigEntry<float> Level100TwoHandedDamage;
        public static ConfigEntry<float> Level100ReduceAttackStamina;
        public static ConfigEntry<float> Level150SwordsDamage;
        public static ConfigEntry<float> Level150CarryWeight;
        public static ConfigEntry<float> Level150SpeedMultiplierOneHanded;
        public static ConfigEntry<float> Level200SpeedMultiplierTwoHanded;
        public static ConfigEntry<float> Level200OneTwoHandedDamage;

        public static void InitConfigs(ConfigFile config)
        {
            PassiveTwoHandedMultiplier = config.Bind("Strength Server config", "ExpMultiplierPerLevel", 10f,
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

            Level50CarryWeight = config.Bind("Strength Server config", "Level50CarryWeight", 50f,
                    new ConfigDescription("Level50CarryWeight", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100TwoHandedDamage = config.Bind("Strength Server config", "Level100TwoHandedDamage", 1.2f,
                    new ConfigDescription("Level100TwoHandedDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100ReduceAttackStamina = config.Bind("Strength Server config", "Level100ReduceAttackStamina", 1.1f,
                    new ConfigDescription("Level100ReduceAttackStamina", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150CarryWeight = config.Bind("Strength Server config", "Level150CarryWeight", 50f,
                    new ConfigDescription("Level150CarryWeight", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150SwordsDamage = config.Bind("Strength Server config", "Level150SwordsDamage", 1.2f,
                    new ConfigDescription("Level150SwordsDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150SpeedMultiplierOneHanded = config.Bind("Strength Server config", "Level150SpeedMultiplierOneHanded", 1.1f,
                    new ConfigDescription("Level150SpeedMultiplierOneHanded", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200SpeedMultiplierTwoHanded = config.Bind("Strength Server config", "Level200SpeedMultiplierTwoHanded", 1.05f,
                    new ConfigDescription("Level200SpeedMultiplierTwoHanded", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200OneTwoHandedDamage = config.Bind("Strength Server config", "Level200OneTwoHandedDamage", 1.15f,
                    new ConfigDescription("Level200OneTwoHandedDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
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

                if (skillLevel >= 150)
                {
                    if (__instance.m_shared?.m_skillType == Skills.SkillType.Swords)
                    {
                        __result.m_blunt *= Level150SwordsDamage.Value;
                        __result.m_slash *= Level150SwordsDamage.Value;
                        __result.m_pierce *= Level150SwordsDamage.Value;
                        __result.m_chop *= Level150SwordsDamage.Value;
                        __result.m_pickaxe *= Level150SwordsDamage.Value;
                        __result.m_fire *= Level150SwordsDamage.Value;
                        __result.m_frost *= Level150SwordsDamage.Value;
                        __result.m_lightning *= Level150SwordsDamage.Value;
                        __result.m_poison *= Level150SwordsDamage.Value;
                        __result.m_spirit *= Level150SwordsDamage.Value;
                    }
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
                        __result *= 1 - (Level100ReduceAttackStamina.Value - 1);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyMaxCarryWeight))]
        public static class ModifyMaxCarryWeight
        {
            public static void Postfix(SEMan __instance, ref float limit)
            {
                if (__instance.m_character.IsPlayer())
                {
                    int skillLevel = Level.GetSkillLevel(Skill.Strength);

                    if (skillLevel >= 50)
                    {
                        limit += Level50CarryWeight.Value;
                    }

                    if (skillLevel >= 150)
                    {
                        limit += Level150CarryWeight.Value;
                    }
                }
            }
        }

        public static Dictionary<long, string> lastAnimations = new Dictionary<long, string>();

        [HarmonyPatch(typeof(CharacterAnimEvent), "Speed")]
        static class CharacterAnimEvent_Speed_Patch
        {
            static void Postfix(ref Animator ___m_animator, Character ___m_character, float speedScale)
            {
                if (Player.m_localPlayer == null) return;

                int skillLevel = Level.GetSkillLevel(Skill.Strength);
                if (skillLevel < 150) return;

                if (___m_character is Player)
                    lastAnimations.Remove((___m_character as Player).GetPlayerID());
            }
        }

        [HarmonyPatch(typeof(CharacterAnimEvent), "FixedUpdate")]
        static class CharacterAnimEventFixedUpdate
        {
            static void Prefix(ref Animator ___m_animator, Character ___m_character)
            {
                if (Player.m_localPlayer == null) return;

                int skillLevel = Level.GetSkillLevel(Skill.Strength);
                if (skillLevel < 150) return;

                if (!(___m_character is Humanoid) || Player.m_localPlayer == null || (___m_character is Player && (___m_character as Player).GetPlayerID() != Player.m_localPlayer.GetPlayerID()))
                    return;

                if (___m_animator?.GetCurrentAnimatorClipInfo(0)?.Any() != true || ___m_animator.GetCurrentAnimatorClipInfo(0)[0].clip == null)
                {
                    return;
                }

                if (___m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.StartsWith("Attack"))
                {
                    var itemType = (___m_character as Humanoid).GetCurrentWeapon()?.m_shared.m_itemType;
                    if (itemType == ItemDrop.ItemData.ItemType.OneHandedWeapon)
                        ___m_animator.speed = ChangeSpeed(___m_character, ___m_animator, Level150SpeedMultiplierOneHanded.Value);

                    if (skillLevel < 200) return;
                    if (itemType == ItemDrop.ItemData.ItemType.TwoHandedWeapon)
                        ___m_animator.speed = ChangeSpeed(___m_character, ___m_animator, Level200SpeedMultiplierTwoHanded.Value);

                }
            }
        }

        public static float ChangeSpeed(Character character, Animator animator, float speed)
        {
            if (character is Player)
            {
                var player = (Player)character;

                if (Player.m_localPlayer != player) return animator.speed;

                long id = player.GetPlayerID();
                string name = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                float newSpeed = animator.speed * speed;
                if (!lastAnimations.ContainsKey(id) || lastAnimations[id] != name)
                {
                    lastAnimations[id] = name;
                    return newSpeed;
                }
            }
            return animator.speed;
        }
    }
}
