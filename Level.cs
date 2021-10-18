using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MMRPGSkillSystem
{
    public class Level
    {
        public static List<LevelRequirement> LevelRequirementList { get; set; }
        public static int PlayerCurrentLevel { get; set; }
        public static long PlayerCurrentExp { get; set; }

        public static void InitLevelRequirementList()
        {
            LevelRequirementList = new List<LevelRequirement>();
            int baseExpLevelPerLevel = 1000;
            float expMultiplier = 1.05f;
            for (int level = 1; level <= MMRPGSkillSystem.MaxLevel; level++)
            {
                LevelRequirement levelRequirement = new LevelRequirement();
                levelRequirement.Level = level;

                if (level == 1)
                {
                    levelRequirement.ExpAmount = baseExpLevelPerLevel;
                    LevelRequirementList.Add(levelRequirement);
                    continue;
                }

                var lastLevelRequirement = LevelRequirementList.Last().ExpAmount;
                levelRequirement.ExpAmount = (long)Math.Round((lastLevelRequirement + baseExpLevelPerLevel) * expMultiplier);
                LevelRequirementList.Add(levelRequirement);
                Debug.LogError(levelRequirement.ExpAmount);
            }
        }

        public static void RaiseExp(string creatureName)
        {
            MonsterExp monster = ExpTable.MonsterExpList.Where(x => x.Name == creatureName).FirstOrDefault();

            if (monster == null) Debug.Log("No MonsterExp for creature: " + creatureName);

            AddExp(monster.ExpAmount);
        }

        unsafe public static void AddExp(int exp)
        {
            if (!Player.m_localPlayer.m_knownTexts.ContainsKey("playerLevel"))
            {
                Player.m_localPlayer.m_knownTexts.Add("playerLevel", "1");
                Player.m_localPlayer.m_knownTexts.Add("playerExp", exp.ToString());
                Player.m_localPlayer.m_knownTexts.Add("playerPointsExpended", exp.ToString());
            }
            else
            {
                long currentExp = long.Parse(Player.m_localPlayer.m_knownTexts["playerExp"]);
                int currentLevel = Convert.ToInt32(Player.m_localPlayer.m_knownTexts["playerLevel"]);
                long newExp = exp + currentExp;
                LevelRequirement currentLevelRequirement = LevelRequirementList.First(x => x.Level == currentLevel);

                if (newExp >= currentLevelRequirement.ExpAmount)
                {
                    PlayerLevelUp(currentLevel);
                }
                else
                {
                    Player.m_localPlayer.m_knownTexts["playerExp"] = newExp.ToString();
                }
            }

            GUI.UpdateExpText();  
        }

        private static void PlayerLevelUp(int currentLevel)
        {
            Player.m_localPlayer.m_knownTexts["playerExp"] = "0";
            Player.m_localPlayer.m_knownTexts["playerLevel"] = (currentLevel + 1).ToString();

            Player.m_localPlayer.Message(MessageHud.MessageType.Center, "Level: " + (currentLevel + 1));

            GUI.UpdatePlayerLevelText();
            AddPoints();
        }

        public static long GetMaxExpForCurrentLevel()
        {
            LevelRequirement currentLevelRequirement = LevelRequirementList.First(x => x.Level == long.Parse(Player.m_localPlayer.m_knownTexts["playerLevel"]));
            return currentLevelRequirement.ExpAmount;
        }

        public static string GetLevel()
        {
            if (!Player.m_localPlayer.m_knownTexts.ContainsKey("playerLevel"))
            {
                Player.m_localPlayer.m_knownTexts.Add("playerLevel", "1");
            }
            
            return Player.m_localPlayer.m_knownTexts["playerLevel"];
        }

        public static string GetExp()
        {
            if (!Player.m_localPlayer.m_knownTexts.ContainsKey("playerExp"))
            {
                Player.m_localPlayer.m_knownTexts.Add("playerExp", "1");
            }
            return Player.m_localPlayer.m_knownTexts["playerExp"];
        }

        public static string GetAvailablePoints()
        {
            if (!Player.m_localPlayer.m_knownTexts.ContainsKey("playerAvailablePoints"))
            {
                Player.m_localPlayer.m_knownTexts.Add("playerAvailablePoints", "0");
            }
            return Player.m_localPlayer.m_knownTexts["playerAvailablePoints"];
        }

        public static void AddPoints()
        {
            if (!Player.m_localPlayer.m_knownTexts.ContainsKey("playerAvailablePoints"))
            {
                Player.m_localPlayer.m_knownTexts.Add("playerAvailablePoints", "0");
            }

            int availablePoints = Convert.ToInt32(GetAvailablePoints());
            availablePoints += MMRPGSkillSystem.PointsPerLevel;

            Player.m_localPlayer.m_knownTexts["playerAvailablePoints"] = availablePoints.ToString();
        }

        public static void RemovePoints()
        {
            if (!Player.m_localPlayer.m_knownTexts.ContainsKey("playerAvailablePoints"))
            {
                Player.m_localPlayer.m_knownTexts.Add("playerAvailablePoints", "0");
            }

            int availablePoints = Convert.ToInt32(GetAvailablePoints());
            availablePoints -= 1;

            Player.m_localPlayer.m_knownTexts["playerAvailablePoints"] = availablePoints.ToString();
        }
    }

    public class LevelRequirement
    {
        public int Level { get; set; }
        public long ExpAmount { get; set; }
    }
}


