using MMRPGSkillSystem.PlayerSkills;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        public static void RaiseExp(Character creature)
        {
            MonsterExp monster = ExpTable.MonsterExpList.Where(x => x.Name == creature.gameObject.name).FirstOrDefault();

            if (monster == null)
            {
                Debug.LogError("No MonsterExp for creature: " + creature.gameObject.name);
                return;
            }

            var exp = monster.ExpAmount * (creature.m_level / 100 + 1); //10% exp bonus per star

            if (creature.m_faction == Character.Faction.Boss) exp *= 10; //10x bonus boss

            float rangeToDivideExp = 100f;
            int nearPlayers = Player.GetPlayersInRangeXZ(Player.m_localPlayer.transform.position, rangeToDivideExp);

            Debug.LogError("nearPlayers: " + nearPlayers);
            exp *= ValheimLevelSystem.ExpRate.Value;
            exp /= nearPlayers;

            AddExp(exp);
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

            Debug.LogError(Player.m_localPlayer.m_knownTexts["playerExp"]);

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
            Player localPlayer = Player.m_localPlayer;
            if (!localPlayer || localPlayer.IsDead() || (localPlayer.InCutscene() || localPlayer.IsTeleporting()))
                return 0;

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


