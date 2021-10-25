using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace MMRPGSkillSystem
{
    [HarmonyPatch]
    public class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), "OnSpawned")]
        private static void OnSpawnedPostfix()
        {
            if (MMRPGSkillSystem.listInitiliazed) return;
            Level.InitLevelRequirementList();
            ExpTable.InitMonsterExpList();
            MMRPGSkillSystem.listInitiliazed = true;
        }

        [HarmonyPatch(typeof(Character), nameof(Character.ApplyDamage))]
        public static class ApplyDamage
        {
            public static void Postfix(Character __instance, HitData hit)
            {
                if (__instance.GetHealth() <= 0f && hit.GetAttacker() && hit.GetAttacker().IsPlayer())
                {
                    Level.RaiseExp(__instance);
                }
            }
        }

        [HarmonyPatch(typeof(ZNet), "Shutdown")]
        internal class Disconnect
        {
            private static void Postfix()
            {
                foreach (var obj in MMRPGSkillSystem.menuItems)
                {
                    GameObject gameObject = obj.Value;
                    Object.Destroy(gameObject);
                }

                MMRPGSkillSystem.menuItems = new Dictionary<string, GameObject>();
                Object.Destroy(MMRPGSkillSystem.Menu);
                MMRPGSkillSystem.Menu = null;
                MMRPGSkillSystem.listInitiliazed = false;
            }
        }
    }
}
