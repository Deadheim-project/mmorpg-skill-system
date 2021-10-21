using System;

namespace MMRPGSkillSystem
{
    public class SkillService
    {
        public static void SkillUp(string skill)
        {
            int availablePoints = Convert.ToInt32(Level.GetAvailablePoints());
            if (availablePoints < 1) return;

            int skillLevel = Convert.ToInt32(Level.GetSkillLevel(skill));
            string newLevel = (skillLevel + 1).ToString();
            Player.m_localPlayer.m_knownTexts["player" + skill] = newLevel;

            GUI.UpdateSkillLevelText(skill, newLevel);
            Level.RemovePoints();
        }
    }
}
