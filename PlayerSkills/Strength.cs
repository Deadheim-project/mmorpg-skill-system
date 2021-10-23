using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MMRPGSkillSystem.PlayerSkills
{
    [HarmonyPatch]
    public class Strength
    {
        public static int PassiveTwoHandedMultiplier = 10;
        public static int PassiveOneHandedMultiplier = 5;
        public static int PassiveChopAndPickaxeMultiplier = 25;

        [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetDamage), typeof(int))]
        public class GetDamage
        {
            public static void Postfix(ItemDrop.ItemData __instance, ref HitData.DamageTypes __result)
            {
                int skillLevel = Level.GetSkillLevel(Skill.Strength);

                if (skillLevel >= 50)
                {
                    if (__instance.m_shared.m_itemType == ItemDrop.ItemData.ItemType.OneHandedWeapon)
                    {
                        __result.m_blunt *= 1.10f;
                        __result.m_slash *= 1.10f;
                        __result.m_pierce *= 1.10f;
                        __result.m_chop *= 1.10f;
                        __result.m_pickaxe *= 1.10f;
                        __result.m_fire *= 1.10f;
                        __result.m_frost *= 1.10f;
                        __result.m_lightning *= 1.10f;
                        __result.m_poison *= 1.10f;
                        __result.m_spirit *= 1.10f;
                    }
                }

                if (skillLevel >= 100)
                {
                    if (__instance.m_shared.m_itemType == ItemDrop.ItemData.ItemType.TwoHandedWeapon)
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

                if (skillLevel >= 150)
                {
                    if (__instance.m_shared?.m_skillType == Skills.SkillType.Swords)
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

                if (skillLevel >= 200)
                {
                    if (__instance.m_shared.m_itemType == ItemDrop.ItemData.ItemType.TwoHandedWeapon || __instance.m_shared.m_itemType == ItemDrop.ItemData.ItemType.OneHandedWeapon)
                    {
                        __result.m_blunt *= 1.15f;
                        __result.m_slash *= 1.15f;
                        __result.m_pierce *= 1.15f;
                        __result.m_chop *= 1.15f;
                        __result.m_pickaxe *= 1.15f;
                        __result.m_fire *= 1.15f;
                        __result.m_frost *= 1.15f;
                        __result.m_lightning *= 1.15f;
                        __result.m_poison *= 1.15f;
                        __result.m_spirit *= 1.15f;
                    }
                }

                ApplyPassiveMultiplier(__instance, ref __result, skillLevel);
            }
        }

        private static void ApplyPassiveMultiplier(ItemDrop.ItemData __instance, ref HitData.DamageTypes __result, int skillLevel)
        {
            float twoHandedMultiplier = ((skillLevel / 100) * PassiveTwoHandedMultiplier) / 100 + 1;
            float oneHandedMultiplier = ((skillLevel / 100) * PassiveOneHandedMultiplier) / 100 + 1;
            float chopAndPickaxeMultiplier = ((skillLevel / 100) * PassiveChopAndPickaxeMultiplier) / 100 + 1;

            if (__instance.m_shared.m_itemType == ItemDrop.ItemData.ItemType.OneHandedWeapon)
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
                        __result *= 0.9f;
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
                        limit += 50;
                    }

                    if (skillLevel >= 150)
                    {
                        limit += 50;
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
                    float speedMultiplierOneHanded = 1.1f;  
                    float speedMultiplierTwoHanded = 1.05f;  
                    var itemType = (___m_character as Humanoid).GetCurrentWeapon()?.m_shared.m_itemType;
                    if (itemType == ItemDrop.ItemData.ItemType.OneHandedWeapon)
                        ___m_animator.speed = ChangeSpeed(___m_character, ___m_animator, speedMultiplierOneHanded);

                    if (skillLevel < 200) return;
                    if (itemType == ItemDrop.ItemData.ItemType.TwoHandedWeapon)
                        ___m_animator.speed = ChangeSpeed(___m_character, ___m_animator, speedMultiplierTwoHanded);

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
