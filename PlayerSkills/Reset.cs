﻿using MMRPGSkillSystem.PlayerSkills;
using System;
using System.Linq;

namespace MMRPGSkillSystem
{
    public class Reset
    {
        public static void ResetSkills()
        {
            var inventory = Player.m_localPlayer.GetInventory();
            ItemDrop.ItemData resetToken = inventory.m_inventory.FirstOrDefault(x => x.m_dropPrefab.name.ToLower() == "resettoken");

            if (resetToken == null && MMRPGSkillSystem.RequiresTokenToResetSkill.Value)
            {
                Player.m_localPlayer.Message(MessageHud.MessageType.Center, "You need a Reset token", 0, null);
                GUIConfirm.DestroyMenu();
                return;
            }

            foreach (Skill skill in Enum.GetValues(typeof(Skill)).Cast<Skill>().ToList())
            {
                Player.m_localPlayer.m_knownTexts["player" + skill.ToString()] = "1";
                GUI.UpdateSkillLevelText(skill.ToString(), "1");
            }

            int level = Convert.ToInt32(Level.GetLevel());
            Player.m_localPlayer.m_knownTexts["playerAvailablePoints"] = (level * MMRPGSkillSystem.PointsPerLevel.Value + MMRPGSkillSystem.StartingPoints.Value).ToString();

            if (MMRPGSkillSystem.RequiresTokenToResetSkill.Value) inventory.RemoveOneItem(resetToken);

            GUI.UpdatePlayerPointsAvailable();
            GUIConfirm.DestroyMenu();
        }
    }
}