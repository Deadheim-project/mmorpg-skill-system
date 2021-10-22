using BepInEx;
using UnityEngine;
using System.Collections.Generic;
using BepInEx.Configuration;
using Jotunn.Utils;
using HarmonyLib;
using Jotunn.Managers;

namespace MMRPGSkillSystem
{
    [BepInPlugin(PluginGUID, PluginGUID, Version)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    public class MMRPGSkillSystem : BaseUnityPlugin
    {
        public const string PluginGUID = "Detalhes.MMRPGSkillSystem";
        public const string Name = "MMRPGSkillSystem";
        public const string Version = "1.0.0";
        public static Dictionary<string, GameObject> menuItems = new Dictionary<string, GameObject>();

        public static ConfigEntry<int> ExpRate;
        public static ConfigEntry<int> StartingPoints;
        public static ConfigEntry<int> MaxLevel;
        public static ConfigEntry<int> PointsPerLevel;
        public static ConfigEntry<int> BaseExpPerLevel;
        public static ConfigEntry<float> ExpMultiplierPerLevel;
        public static ConfigEntry<KeyCode> KeyboardShortcut;

        public static ConfigEntry<int> Tier1Exp;
        public static ConfigEntry<int> Tier2Exp;
        public static ConfigEntry<int> Tier3Exp;
        public static ConfigEntry<int> Tier4Exp;
        public static ConfigEntry<int> Tier5Exp;
        public static ConfigEntry<int> Tier6Exp;
        public static ConfigEntry<int> Tier7Exp;
        public static ConfigEntry<int> Tier8Exp;
        public static ConfigEntry<int> Tier9Exp;
        public static ConfigEntry<int> Tier10Exp;
        public static ConfigEntry<int> TierBossExp;

        public static ConfigEntry<string> Tier1Creatures;
        public static ConfigEntry<string> Tier2Creatures;
        public static ConfigEntry<string> Tier3Creatures;
        public static ConfigEntry<string> Tier4Creatures;
        public static ConfigEntry<string> Tier5Creatures;
        public static ConfigEntry<string> Tier6Creatures;
        public static ConfigEntry<string> Tier7Creatures;
        public static ConfigEntry<string> Tier8Creatures;
        public static ConfigEntry<string> Tier9Creatures;
        public static ConfigEntry<string> Tier10Creatures;
        public static ConfigEntry<string> TierBossCreatures;

        public static List<string> playerSkills = new List<string>(new string[] { "Strength", "Agility", "Intelligence", "Constitution", "Focus" });

        Harmony harmony = new Harmony(PluginGUID);

        public static GameObject Menu;

        public void Awake()
        {
            Config.SaveOnConfigSet = true;

            InitConfigs();

            SynchronizationManager.OnConfigurationSynchronized += (obj, attr) =>
            {
                if (attr.InitialSynchronization)
                {
                    Jotunn.Logger.LogMessage("Initial Config sync event received");
                }
                else
                {
                    Jotunn.Logger.LogMessage("Config sync event received");
                }
            };

            harmony.PatchAll();
            ExpTable.InitMonsterExpList();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyboardShortcut.Value))
            {
                GUI.ToggleMenu();
            }
        }

        public void InitConfigs()
        {
            ExpRate = Config.Bind("Server config", "ExpRate", 1,
                new ConfigDescription("ExpMultilplier",
                    new AcceptableValueRange<int>(1, 100), null,
                         new ConfigurationManagerAttributes { IsAdminOnly = true }));

            StartingPoints = Config.Bind("Server config", "StartingPoints", 15,
               new ConfigDescription("StartingPoints",
                   new AcceptableValueRange<int>(1, 100), null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));


            MaxLevel = Config.Bind("Server config", "MaxLevel", 1,
                new ConfigDescription("MaxLevel",
                    new AcceptableValueRange<int>(1, 1000), null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            BaseExpPerLevel = Config.Bind("Server config", "BaseExpPerLevel", 1000,
                new ConfigDescription("BaseExpPerLevel",
                    new AcceptableValueRange<int>(1, int.MaxValue), null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));


            ExpMultiplierPerLevel = Config.Bind("Server config", "ExpMultiplierPerLevel", 1.05f,
                new ConfigDescription("ExpMultiplierPerLevel",
                    new AcceptableValueRange<float>(1, 100), null,
                         new ConfigurationManagerAttributes { IsAdminOnly = true }));

            PointsPerLevel = Config.Bind("Server config", "PointsPerLevel", 5,
                new ConfigDescription("PointsPerLevel",
                    new AcceptableValueRange<int>(1, int.MaxValue), null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            KeyboardShortcut = Config.Bind("Server Config", "KeyboardShortcutConfig",
                KeyCode.Equals,
                    new ConfigDescription("Server side KeyboardShortcut", null, null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier1Exp = Config.Bind("Server config", "Tier1Exp", 50,
                new ConfigDescription("Tier1Exp",
                    new AcceptableValueRange<int>(1, int.MaxValue), null,
                         new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier1Creatures = Config.Bind("Server config", "Tier1Creatures", "Eikthyr,Boar,Crow,Deer,Neck,Gull,Seagal,Greyling",
                new ConfigDescription("Tier1Creatures", null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier2Exp = Config.Bind("Server config", "Tier2Exp", 100,
                new ConfigDescription("Tier2Exp",
                    new AcceptableValueRange<int>(1, int.MaxValue), null,
                         new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier2Creatures = Config.Bind("Server config", "Tier2Creatures", "gd_king,Greydwarf,Greydwarf_Elite,Greydwarf_Shaman,Skeleton,Skeleton_Poison,TentaRoot,Skeleton_NoArcher",
                new ConfigDescription("Tier2Creatures", null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier3Exp = Config.Bind("Server config", "Tier3Exp", 200,
                new ConfigDescription("Tier3Exp",
                    new AcceptableValueRange<int>(1, int.MaxValue), null,
                         new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier3Creatures = Config.Bind("Server config", "Tier3Creatures", "Bonemass,Blob,Ghost,Leech,Wraith,Draugr,Draugr_Ranged,Surtling,Troll",
                new ConfigDescription("Tier3Creatures", null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));
            
            Tier4Exp = Config.Bind("Server config", "Tier4Exp", 300,
                new ConfigDescription("Tier4Exp",
                    new AcceptableValueRange<int>(1, int.MaxValue), null,
                         new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier4Creatures = Config.Bind("Server config", "Tier4Creatures", "Dragon,Draugr_Elite,BlobElite,Wolf,Fenring,Drake,Hatchling",
                new ConfigDescription("Tier4Creatures", null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));
            
            Tier5Exp = Config.Bind("Server config", "Tier5Exp", 400,
                new ConfigDescription("Tier5Exp",
                    new AcceptableValueRange<int>(1, int.MaxValue), null,
                         new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier5Creatures = Config.Bind("Server config", "Tier5Creatures", "GoblinKing,Fuling,Goblin,GoblinArcher,Serpent,Deathsquito,Lox",
                new ConfigDescription("Tier5Creatures", null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier6Exp = Config.Bind("Server config", "Tier6Exp", 500,
                new ConfigDescription("Tier6Exp",
                    new AcceptableValueRange<int>(1, int.MaxValue), null,
                         new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier6Creatures = Config.Bind("Server config", "Tier6Creatures", "StoneGolem,GoblinBrute,GoblinShaman",
                new ConfigDescription("Tier6Creatures", null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier7Exp = Config.Bind("Server config", "Tier7Exp", 50,
                new ConfigDescription("Tier7Exp",
                    new AcceptableValueRange<int>(1, int.MaxValue), null,
                         new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier7Creatures = Config.Bind("Server config", "Tier7Creatures", "",
                new ConfigDescription("Tier7Creatures", null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier8Exp = Config.Bind("Server config", "Tier8Exp", 50,
                new ConfigDescription("Tier8Exp",
                    new AcceptableValueRange<int>(1, int.MaxValue), null,
                         new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier8Creatures = Config.Bind("Server config", "Tier8Creatures", "",
                new ConfigDescription("Tier8Creatures", null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));
                        Tier1Exp = Config.Bind("Server config", "Tier1Exp", 50,
                new ConfigDescription("Tier1Exp",
                    new AcceptableValueRange<int>(1, int.MaxValue), null,
                         new ConfigurationManagerAttributes { IsAdminOnly = true }));
            Tier9Exp = Config.Bind("Server config", "Tier9Exp", 50,
                new ConfigDescription("Tier9Exp",
                    new AcceptableValueRange<int>(1, int.MaxValue), null,
                         new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier9Creatures = Config.Bind("Server config", "Tier9Creatures", "",
                new ConfigDescription("Tier9Creatures", null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier10Exp = Config.Bind("Server config", "Tier10Exp", 50,
                new ConfigDescription("Tier10Exp",
                    new AcceptableValueRange<int>(1, int.MaxValue), null,
                         new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Tier10Creatures = Config.Bind("Server config", "Tier10Creatures", "",
                new ConfigDescription("Tier10Creatures", null,
                        new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }
    }
}
