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
            if (ValheimLevelSystem.listInitiliazed) return;
            Level.InitLevelRequirementList();
            ExpTable.InitMonsterExpList();
            ValheimLevelSystem.listInitiliazed = true;
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
                foreach (var obj in ValheimLevelSystem.menuItems)
                {
                    GameObject gameObject = obj.Value;
                    Object.Destroy(gameObject);
                }

                ValheimLevelSystem.menuItems = new Dictionary<string, GameObject>();
                Object.Destroy(ValheimLevelSystem.Menu);
                ValheimLevelSystem.Menu = null;
                ValheimLevelSystem.listInitiliazed = false;
            }
        }
    }
}
