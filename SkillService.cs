using System;

namespace MMRPGSkillSystem
{
    public class SkillService
    {
        public static void SkillUp(Skills.SkillType skill)
        {
            var sk = Player.m_localPlayer.m_skills.m_skillData[skill];
            if (sk.m_level >= 100) return;

            int availablePoints = Convert.ToInt32(Level.GetAvailablePoints());

            if (availablePoints < 1) return;

            sk.m_level += 1;

            Level.RemovePoints();
            GUI.UpdateSkillLevelText(skill, sk.m_level);
        }
    }
}
