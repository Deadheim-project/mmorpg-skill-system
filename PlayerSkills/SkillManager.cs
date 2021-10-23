using System;

namespace MMRPGSkillSystem.PlayerSkills
{
    public class SkillManager
    {
        public static void SkillUp(Skill skill)
        {
            int availablePoints = Convert.ToInt32(Level.GetAvailablePoints());
            if (availablePoints < 1) return;

            int skillLevel = Level.GetSkillLevel(skill);
            string newLevel = (skillLevel + 1).ToString();
            Player.m_localPlayer.m_knownTexts["player" + skill] = newLevel;

            GUI.UpdateSkillLevelText(skill.ToString(), newLevel);
            Level.RemovePoints();
        }
    }

    public enum Skill
    {
        Strength = 0,
        Agility = 1,
        Intelligence = 2,
        Focus = 3,
        Constitution = 4
    }
}
