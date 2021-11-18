using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Jotunn.Managers;
using System;

namespace MMRPGSkillSystem
{
    [HarmonyPatch]
    public class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), "OnSpawned")]
        private static void OnSpawnedPostfix()
        {
            Player.m_localPlayer.m_nview.GetZDO().Set("playerName", ValheimLevelSystem.PlayerName + " " + Level.GetLevel());
            if (ValheimLevelSystem.listInitiliazed) return;
            Level.InitLevelRequirementList();
            ExpTable.InitMonsterExpList();
            ValheimLevelSystem.listInitiliazed = true;
        }

        [HarmonyPatch(typeof(Player), "SetPlayerID")]
        internal class SetPlayerID
        {
            private static void Postfix(long playerID, string name)
            {
                try
                {
                    if (!Player.m_localPlayer) return;
                    ValheimLevelSystem.PlayerName = Player.m_localPlayer.m_nview.GetZDO().GetString("playerName");
                }
                catch
                {

                }
            }
        }

        [HarmonyPatch(typeof(Character), nameof(Character.ApplyDamage))]
        public static class ApplyDamage
        {
            public static void Postfix(Character __instance, HitData hit)
            {
                if (__instance.GetHealth() <= 0f && hit.GetAttacker() && hit.GetAttacker().IsPlayer())
                {
                    var pkg = new ZPackage();
                    string msg = Convert.ToInt32(__instance.transform.position.x) + ",";
                    msg += Convert.ToInt32(__instance.transform.position.y) + ",";
                    msg += __instance.gameObject.name + ",";
                    msg += __instance.GetLevel();
                    pkg.Write(msg);
                    ZRoutedRpc.instance.InvokeRoutedRPC(ZNetView.Everybody, "RaiseExp", new object[] { pkg });
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
                    UnityEngine.Object.Destroy(gameObject);
                }

                ValheimLevelSystem.menuItems = new Dictionary<string, GameObject>();
                UnityEngine.Object.Destroy(ValheimLevelSystem.Menu);
                ValheimLevelSystem.Menu = null;
                ValheimLevelSystem.listInitiliazed = false;
            }
        }
    }
}
