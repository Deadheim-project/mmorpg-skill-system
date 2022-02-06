using HarmonyLib;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;

namespace ValheimLevelSystem
{
    public class StatusEffects
    {
        [HarmonyPatch(typeof(ObjectDB), nameof(ObjectDB.Awake))]
        public static class ConsumeItem
        {
            private static void Postfix(ObjectDB __instance)
            {
                __instance.m_StatusEffects.Add(CreateStatusEffect("vls_strength"));
                __instance.m_StatusEffects.Add(CreateStatusEffect("vls_agility"));
                __instance.m_StatusEffects.Add(CreateStatusEffect("vls_focus"));
                __instance.m_StatusEffects.Add(CreateStatusEffect("vls_intelligence"));
                __instance.m_StatusEffects.Add(CreateStatusEffect("vls_constitution"));
            }
        }

        public static SE_Stats CreateStatusEffect(string efectName)
        {
            SE_Stats effect = ScriptableObject.CreateInstance<SE_Stats>();
            effect.name = efectName;
            effect.m_name = efectName;
            effect.m_startMessageType = MessageHud.MessageType.TopLeft;
            effect.m_startMessage = efectName;
            effect.m_stopMessageType = MessageHud.MessageType.TopLeft;
            effect.m_stopMessage = efectName;

            return effect;
        }
    }
}
