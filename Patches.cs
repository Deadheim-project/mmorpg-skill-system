using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using System;
using ValheimLevelSystem.PlayerSkills;
using Jotunn.Managers;

namespace ValheimLevelSystem
{
    [HarmonyPatch]
    public class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), "OnSpawned")]
        private static void OnSpawnedPostfix()
        {
            Agility.UpdateStatusEffect();
                                  
            if (ValheimLevelSystem.ShowLevelOnName.Value) Player.m_localPlayer.m_nview.GetZDO().Set("playerName", ValheimLevelSystem.PlayerName + " " + Level.GetLevel());
            
            if (ValheimLevelSystem.listInitiliazed) return;
            ValheimLevelSystem.listInitiliazed = true;
        }

        [HarmonyPatch(typeof(Player), nameof(Player.OnDeath))]
        private static class OnDeath
        {
            private static void Postfix()
            {
                Level.RemoveExpOnDeath();
            }
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
                if (!(__instance.GetHealth() <= 0f)) return;
                
                bool hasToBeKilledByAPlayerToGiveExp = ValheimLevelSystem.OnlyGiveExpIfDamageComesFromPlayer.Value;

                if (hasToBeKilledByAPlayerToGiveExp)
                {
                    if (hit.GetAttacker() && hit.GetAttacker().IsPlayer())
                    {
                        var pkg = new ZPackage();
                        string msg = Convert.ToInt32(__instance.transform.position.x) + ",";
                        msg += Convert.ToInt32(__instance.transform.position.y) + ",";
                        msg += Convert.ToInt32(__instance.transform.position.z) + ",";
                        msg += __instance.gameObject.name + ",";
                        msg += __instance.GetLevel() + ",";
                        msg += true;

                        pkg.Write(msg);
                        ZRoutedRpc.instance.InvokeRoutedRPC(ZNetView.Everybody, "RaiseExp", new object[] { pkg });
                    }
                } else
                {
                    var pkg = new ZPackage();
                    string msg = Convert.ToInt32(__instance.transform.position.x) + ",";
                    msg += Convert.ToInt32(__instance.transform.position.y) + ",";
                    msg += Convert.ToInt32(__instance.transform.position.z) + ",";
                    msg += __instance.gameObject.name + ",";
                    msg += __instance.GetLevel() + ",";

                    bool killedByPlayer = hit.GetAttacker() && hit.GetAttacker().IsPlayer();
                    msg += killedByPlayer;

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
