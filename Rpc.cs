using HarmonyLib;
using MMRPGSkillSystem;
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
            if (ZNet.instance.IsServer()) return;

            string[] splited = pkg.ReadString().Split(',');
            int x = Convert.ToInt32(splited[0]);
            int y = Convert.ToInt32(splited[1]);
            string name = (splited[2]);
            int level = Convert.ToInt32(splited[3]);

            if ((double)Vector2.Distance(new Vector2(x, y), Player.m_localPlayer.transform.position) >= (double)Level.rangeToDivideExp)
            {
                return;
            }

            name = name.Replace("(Clone)", "");
            MonsterExp monster = ExpTable.MonsterExpList.Where(xx => xx.Name.ToLower() == name.ToLower()).FirstOrDefault();
            Character character = PrefabManager.Instance.GetPrefab(name).GetComponent<Character>();
 
            Level.RaiseExpWithValues(monster.ExpAmount, level, character.m_faction == Character.Faction.Boss);
        }
    }
}
