using BepInEx;
using HarmonyLib;
using Jotunn.Configs;
using UnityEngine;
using System.Collections.Generic;

namespace MMRPGSkillSystem
{
    [BepInPlugin("Detalhes.MMRPGSkillSystem", "MMRPGSkillSystem", "1.0.2")]
    [BepInProcess("valheim.exe")]
    public class MMRPGSkillSystem : BaseUnityPlugin
    {
        public const string PluginGUID = "Detalhes.MMRPGSkillSystem";
        public static int ExpMultiplier = 1;
        public static int MaxLevel = 100;
        public static int PointsPerLevel = 5;
        public static Dictionary<string, GameObject> menuItems = new Dictionary<string, GameObject>();

        Harmony harmony = new Harmony(PluginGUID);

        public static GameObject Menu;

        private void Awake()
        {
            harmony.PatchAll();
            Level.InitLevelRequirementList();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                GUI.ToggleMenu();
            }
        }
    }
}
