using HarmonyLib;
using System;
using UnityEngine;
using System.Linq;
using Jotunn.Managers;

namespace ValheimLevelSystem
{
    [HarmonyPatch]
    public class RPC
    {
        [HarmonyPatch(typeof(Game), "Start")]
        [HarmonyPrefix]
        public static void Prefix()
        {
            ZRoutedRpc.instance.Register("RaiseExp", new Action<long, ZPackage>(RPC_RaiseExp));
        }

        public static void RPC_RaiseExp(long sender, ZPackage pkg)
        {
            if (!Player.m_localPlayer) return;

            string[] splited = pkg.ReadString().Split(',');
            int x = Convert.ToInt32(splited[0]);
            int y = Convert.ToInt32(splited[1]);
            int z = Convert.ToInt32(splited[2]);
            string name = (splited[3]);
            int level = Convert.ToInt32(splited[4]);
            bool killedByPlayer = Convert.ToBoolean(splited[5]);

            if ((double)Vector3.Distance(new Vector3(x: x, y: y, z:z), Player.m_localPlayer.transform.position) >= (double)ValheimLevelSystem.RangeToDivideExp.Value)
            {
                return;
            }

            name = name.Replace("(Clone)", "");
            MonsterExp monster = ExpTable.MonsterExpList.Where(xx => xx.Name.ToLower() == name.ToLower()).FirstOrDefault();
            if (monster == null)
            {
                Debug.LogError("No MonsterExp for creature: " + name);
                return;
            }
            Character character = PrefabManager.Instance.GetPrefab(name).GetComponent<Character>();

            int expToGive = monster.ExpAmount;

            if (!killedByPlayer) expToGive /= ValheimLevelSystem.ExpToDivideWhenNotKilledByPlayer.Value;

            Level.RaiseExpWithValues(expToGive, level, character.m_faction == Character.Faction.Boss);
        }
    }
}
