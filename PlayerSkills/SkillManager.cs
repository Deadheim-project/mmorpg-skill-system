using BepInEx.Configuration;
using System;
using UnityEngine;

namespace ValheimLevelSystem.PlayerSkills
{
    public class SkillManager
    {
        static KeyboardShortcut morepoints = new KeyboardShortcut(KeyCode.LeftShift);
        public static void SkillUp(Skill skill, int pointsToRaise = 1)
        {
            if (morepoints.IsPressed()) pointsToRaise = 5;

            int availablePoints = Convert.ToInt32(Level.GetAvailablePoints());
            if (availablePoints < pointsToRaise) return;

            int skillLevel = Level.GetSkillLevel(skill);
            string newLevel = (skillLevel + pointsToRaise).ToString();
            Player.m_localPlayer.m_knownTexts["player" + skill] = newLevel;

            GUI.UpdateSkillLevelText(skill.ToString(), newLevel);
            Level.RemovePoints();

            if (skill.Equals(Skill.Agility)) Agility.UpdateStatusEffect();
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
