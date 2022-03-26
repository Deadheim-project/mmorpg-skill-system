using BepInEx.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ValheimLevelSystem.PlayerSkills
{
    public class Intelligence
    {
        public static ConfigEntry<float> PassiveQuickLearner;
        public static ConfigEntry<float> PassiveMagicDamage;
        public static ConfigEntry<float> Level50MagicDamage;
        public static ConfigEntry<float> Level50MoveSpeed;
        public static ConfigEntry<float> Level100QuickLearner;
        public static ConfigEntry<float> Level100BuffAndPotionsCooldown;
        public static ConfigEntry<float> Level150MagicDamage;
        public static ConfigEntry<int> Level150AllSkillBonus;
        public static ConfigEntry<int> Level200AllSkillsBonus;
        public static ConfigEntry<float> Level200ComfortBonus;

        public static int skillLevel = 1;

        public static void InitConfigs(ConfigFile config)
        {
            PassiveQuickLearner = config.Bind("Intelligence Server config", "PassiveQuickLearner", 1.05f,
                    new ConfigDescription("PassiveQuickLearner", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            PassiveMagicDamage = config.Bind("Intelligence Server config", "PassiveMagicDamage", 1.1f,
                    new ConfigDescription("PassiveMagicDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level50MagicDamage = config.Bind("Intelligence Server config", "Level50MagicDamage", 1.1f,
                    new ConfigDescription("Level50MagicDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level50MoveSpeed = config.Bind("Intelligence Server config", "Level50MoveSpeed", 1.05f,
                     new ConfigDescription("Level50MoveSpeed", null, null,
                     new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100QuickLearner = config.Bind("Intelligence Server config", "Level100QuickLearner", 1.1f,
                    new ConfigDescription("Level100QuickLearner", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100BuffAndPotionsCooldown = config.Bind("Intelligence Server config", "Level100LifeRegen", 1.2f,
                    new ConfigDescription("Level50CarLevel100LifeRegenryWeight", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level100BuffAndPotionsCooldown = config.Bind("Intelligence Server config", "Level100StaminaBonus", 10f,
                    new ConfigDescription("Level100StaminaBonus", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150MagicDamage = config.Bind("Intelligence Server config", "Level150MagicDamage", 1.15f,
                    new ConfigDescription("Level150MagicDamage", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level150AllSkillBonus = config.Bind("Intelligence Server config", "Level150AllSkillBonus", 10,
                    new ConfigDescription("Level150AllSkillBonus", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200AllSkillsBonus = config.Bind("Intelligence Server config", "Level200AllSkillsBonus", 10,
                    new ConfigDescription("Level200AllSkillsBonus", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Level200ComfortBonus = config.Bind("Intelligence Server config", "Level200ComfortBonus", 20f,
                    new ConfigDescription("Level200ComfortBonus", null, null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }

        [HarmonyPatch(typeof(SE_Rested), nameof(SE_Rested.CalculateComfortLevel))]
        public class CalculateComfortLevel
        {
            private static void Postfix(Player player, ref int __result)
            {
                if (Player.m_localPlayer == null) return;

                skillLevel = Level.GetSkillLevel(Skill.Intelligence);
                if (skillLevel < 200) return;
                __result += (int)Level200ComfortBonus.Value;
            }
        }

        [HarmonyPatch(typeof(Skills), nameof(Skills.RaiseSkill))]
        public class RaiseSkill
        {
            private static void Prefix(Skills __instance, ref float factor)
            {
                if (Player.m_localPlayer == null) return;

                skillLevel = Level.GetSkillLevel(Skill.Intelligence);
                if (skillLevel == 1) return;

                float skillBonus = ((skillLevel / 100f) * PassiveQuickLearner.Value) / 100f + 1f;

                if (skillLevel >= 100) skillBonus += (Level100QuickLearner.Value - 1f);

                factor *= skillBonus;
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.UpdateMovementModifier))]
        public static class UpdateMovementModifier
        {
            public static void Postfix(Player __instance)
            {
                if (Player.m_localPlayer == null) return;

                skillLevel = Level.GetSkillLevel(Skill.Intelligence);

                if (skillLevel < 50) return;

                __instance.m_equipmentMovementModifier += Level50MoveSpeed.Value - 1;
            }
        }

        [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetDamage), typeof(int))]
        public class GetDamage
        {
            public static void Postfix(ItemDrop.ItemData __instance, ref HitData.DamageTypes __result)
            {

                if (Player.m_localPlayer == null) return;

                if (!Player.m_localPlayer.m_inventory.m_inventory.Exists(x => x == __instance)) return;

                skillLevel = Level.GetSkillLevel(Skill.Intelligence);

                if (skillLevel < 1) return;

                var magicDamageMultier = 1 + (skillLevel / 100f) * (PassiveMagicDamage.Value - 1);

                if (skillLevel > 50) magicDamageMultier += Level50MagicDamage.Value - 1;

                if (skillLevel >= 100) magicDamageMultier += Level150MagicDamage.Value - 1;

                __result.m_fire *= magicDamageMultier;
                __result.m_frost *= magicDamageMultier;
                __result.m_lightning *= magicDamageMultier;
                __result.m_poison *= magicDamageMultier;
                __result.m_spirit *= magicDamageMultier;
            }
        }

        [HarmonyPatch(typeof(Skills), nameof(Skills.GetSkillFactor))]
        public static class GetSkillFactor
        {
            private static void Postfix(Skills __instance, Skills.SkillType skillType, ref float __result)
            {
                if (Player.m_localPlayer == null) return;

                if (skillLevel < 150) return;

                __result *= Level150AllSkillBonus.Value / 100 + 1;
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.ConsumeItem))]
        public static class ConsumeItem
        {
            private static void Prefix(Inventory inventory, ItemDrop.ItemData item)
            {
                Player localPlayer = Player.m_localPlayer;
                if (!localPlayer || localPlayer.IsDead() || (localPlayer.InCutscene() || localPlayer.IsTeleporting()))
                    return;

                skillLevel = Level.GetSkillLevel(Skill.Intelligence);

                if (skillLevel < 100) return;

                if (ValheimLevelSystem.PotionToReduceCooldownNameList.Contains(item.m_dropPrefab.name))
                {
                    float ttl = item.m_shared.m_consumeStatusEffect.m_ttl;
                    item.m_shared.m_consumeStatusEffect.m_ttl = ttl - (ttl / 100f * Level100BuffAndPotionsCooldown.Value);
                };
            }
        }

        [HarmonyPatch(typeof(Player), "SetGuardianPower")]
        public static class SetGuardianPower
        {
            private static void Postfix(ref Player __instance)
            {
                if (skillLevel < 100) return;

                if (__instance.m_guardianSE)
                {
                    __instance.m_guardianSE.m_cooldown = __instance.m_guardianSE.m_cooldown - (__instance.m_guardianSE.m_cooldown / 100 * Level100BuffAndPotionsCooldown.Value);
                }
            }
        }

        [HarmonyPatch(typeof(SkillsDialog), nameof(SkillsDialog.Setup))]
        public static class SetupBar
        {
            private static void Postfix(SkillsDialog __instance, Player player)
            {
                if (Player.m_localPlayer == null) return;

                skillLevel = Level.GetSkillLevel(Skill.Intelligence);

                if (skillLevel < 150) return;
                int bonus = Level150AllSkillBonus.Value;
                if (skillLevel >= 200) bonus += Level200AllSkillsBonus.Value;

                foreach (GameObject gameObject in __instance.m_elements)
                {
                    Skills.Skill skill = player.m_skills.GetSkillList().Find(s => s.m_info.m_description == gameObject.GetComponentInChildren<UITooltip>().m_text);

                    Transform levelbar = Utils.FindChild(gameObject.transform, "bar");
                    GameObject extraLevelbar = UnityEngine.Object.Instantiate(levelbar.gameObject, levelbar.parent);
                    RectTransform rect = extraLevelbar.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2((skill.m_level + bonus) * 1.6f, rect.sizeDelta.y);
                    extraLevelbar.GetComponent<Image>().color = Color.red;
                    extraLevelbar.transform.SetSiblingIndex(levelbar.GetSiblingIndex());
                    Transform levelText = Utils.FindChild(gameObject.transform, "leveltext");
                    levelText.GetComponent<Text>().text += $" <color=cyan>+{Level150AllSkillBonus}</color>";
                }
            }
        }
    }
}