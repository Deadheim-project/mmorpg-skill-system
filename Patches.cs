using HarmonyLib;

namespace MMRPGSkillSystem
{
    public class Patches
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Skills), "RaiseSkill")]
        private static bool RaiseSkillPrefix(Skills __instance, Skills.SkillType skillType, float factor = 1f)
        {
            if (skillType == Skills.SkillType.None)
                return false;
            Skills.Skill skill = __instance.GetSkill(skillType);
            float level = skill.m_level;
            if (level > 1)
                return false;

            return true;          
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Character), "OnDeath")]
        private static void RaiseExpPostfix(Character __instance)
        {
            if (__instance) Level.RaiseExp(__instance.name);
        }

    }
}
