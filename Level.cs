using ValheimLevelSystem.PlayerSkills;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ValheimLevelSystem
{
    public class Level
    {
        public static List<LevelRequirement> LevelRequirementList { get; set; }
        public static int PlayerCurrentLevel { get; set; }
        public static long PlayerCurrentExp { get; set; }

        public static void InitLevelRequirementList()
        {
            LevelRequirementList = new List<LevelRequirement>();
            int baseExpLevelPerLevel = ValheimLevelSystem.BaseExpPerLevel.Value;
            float expMultiplier = ValheimLevelSystem.ExpMultiplierPerLevel.Value;
            for (int level = 1; level <= ValheimLevelSystem.MaxLevel.Value; level++)
            {
                LevelRequirement levelRequirement = new LevelRequirement
                {
                    Level = level
                };

                if (level == 1)
                {
                    levelRequirement.ExpAmount = baseExpLevelPerLevel;
                    LevelRequirementList.Add(levelRequirement);
                    continue;
                }

                var lastLevelRequirement = LevelRequirementList.Last().ExpAmount;
                levelRequirement.ExpAmount = (long)Math.Round((lastLevelRequirement + baseExpLevelPerLevel) * expMultiplier);
                LevelRequirementList.Add(levelRequirement);
            }
        }

        public static void RaiseExpWithValues(int expAmount, int level, bool boss = false)
        {         
            float exp = expAmount + ((expAmount / 100f) * (level * ValheimLevelSystem.ExpPercentageBonusPerStar.Value));

            if (boss) exp *= ValheimLevelSystem.BossExpMultiplier.Value;

            int nearPlayers = Player.GetPlayersInRangeXZ(Player.m_localPlayer.transform.position, ValheimLevelSystem.RangeToDivideExp.Value);

            exp *= ValheimLevelSystem.ExpRate.Value;
            if (nearPlayers > 0) exp /= nearPlayers;

            AddExp(Convert.ToInt32(exp));
            if (ValheimLevelSystem.ShowExpText.Value) Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, "+" + exp + " Exp");
        }

        unsafe public static void AddExp(int exp)
        {
            if (!Player.m_localPlayer.m_knownTexts.ContainsKey("playerLevel"))
            {
                Player.m_localPlayer.m_knownTexts.Add("playerLevel", "1");
                Player.m_localPlayer.m_knownTexts.Add("playerExp", exp.ToString());
            }
            else
            {
                long currentExp = long.Parse(Player.m_localPlayer.m_knownTexts["playerExp"]);
                int currentLevel = Convert.ToInt32(Player.m_localPlayer.m_knownTexts["playerLevel"]);
                long newExp = exp + currentExp;
                LevelRequirement currentLevelRequirement = LevelRequirementList.FirstOrDefault(x => x.Level == currentLevel);

                if (currentLevelRequirement is null) return;

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
            AddPoints();

            Player.m_localPlayer.Message(MessageHud.MessageType.Center, "Level: " + (currentLevel + 1));

            if (ValheimLevelSystem.ShowLevelOnName.Value)
            {
                Player.m_localPlayer.m_nview.GetZDO().Set("playerName", ValheimLevelSystem.PlayerName + " " + Level.GetLevel());
            }

            GUI.UpdatePlayerLevelText();
        }

        public static long GetMaxExpForCurrentLevel()
        {
            Player localPlayer = Player.m_localPlayer;
            if (!localPlayer || localPlayer.IsDead() || (localPlayer.InCutscene() || localPlayer.IsTeleporting()))
                return 0;

            LevelRequirement currentLevelRequirement = LevelRequirementList.FirstOrDefault(x => x.Level == long.Parse(Player.m_localPlayer.m_knownTexts["playerLevel"]));

            if (currentLevelRequirement == null) return 0;

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
                Player.m_localPlayer.m_knownTexts.Add("playerExp", "0");
            }
            return Player.m_localPlayer.m_knownTexts["playerExp"];
        }

        public static string GetAvailablePoints()
        {           

            if (!Player.m_localPlayer.m_knownTexts.ContainsKey("playerAvailablePoints"))
            {
                Player.m_localPlayer.m_knownTexts.Add("playerAvailablePoints", ValheimLevelSystem.StartingPoints.Value.ToString());
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
            availablePoints += ValheimLevelSystem.PointsPerLevel.Value;

            if (Convert.ToInt32(Level.GetLevel()) >= ValheimLevelSystem.LevelToStartGivingExtraPoint.Value) availablePoints += 1;

            Player.m_localPlayer.m_knownTexts["playerAvailablePoints"] = availablePoints.ToString();
            GUI.UpdatePlayerPointsAvailable();
        }

        public static void RemovePoints()
        {              
            int availablePoints = Convert.ToInt32(GetAvailablePoints());
            availablePoints -= 1;

            Player.m_localPlayer.m_knownTexts["playerAvailablePoints"] = availablePoints.ToString();
            GUI.UpdatePlayerPointsAvailable();

        }

        public static int GetSkillLevel(Skill skill)
        {
            string value;
            if (Player.m_localPlayer.m_knownTexts.TryGetValue("player" + skill.ToString(), out value))
            {
                return Convert.ToInt32(value);

            } else
            {
                Player.m_localPlayer.m_knownTexts.Add("player" + skill.ToString(), "1");
            }

            return Convert.ToInt32(Player.m_localPlayer.m_knownTexts["player" + skill.ToString()]); 
        }
    }

    public class LevelRequirement
    {
        public int Level { get; set; }
        public long ExpAmount { get; set; }
    }
}


