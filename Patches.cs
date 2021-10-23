using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace MMRPGSkillSystem
{
    [HarmonyPatch]
    public class Patches
    {
        static bool listInitiliazed = false;
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), "OnSpawned")]
        private static void OnSpawnedPostfix()
        {
            if (listInitiliazed) return;
            Level.InitLevelRequirementList();
            ExpTable.InitMonsterExpList();
            listInitiliazed = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Character), "OnDeath")]
        private static void RaiseExpPostfix(Character __instance)
        {
            if (__instance) Level.RaiseExp(__instance);
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
                listInitiliazed = false;
            }
        }
    }
}
