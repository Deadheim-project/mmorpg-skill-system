using HarmonyLib;
using System.Linq;
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
                __instance.m_StatusEffects.Add(CreateStatusEffect("vls_heal_cooldown", "Heal Cooldown", Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(x => x.name == "health_icon")));                          
            }
        }

        public static SE_Stats CreateStatusEffect(string effectName, string m_name, Sprite icon)
        {
            SE_Stats effect = ScriptableObject.CreateInstance<SE_Stats>();
            effect.name = effectName;
            effect.m_name = m_name;
            effect.m_tooltip = m_name;
            effect.m_icon = icon;
  
            return effect;
        }
    }
}
