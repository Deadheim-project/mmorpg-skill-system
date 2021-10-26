using Jotunn.Managers;
using MMRPGSkillSystem.PlayerSkills;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using SkillManager = MMRPGSkillSystem.PlayerSkills.SkillManager;

namespace MMRPGSkillSystem
{
    class GUI
    {
        public static void ToggleMenu()
        {
            if (!ValheimLevelSystem.Menu && Player.m_localPlayer)
            {
                if (GUIManager.Instance == null)
                {
                    Debug.LogError("GUIManager instance is null");
                    return;
                }

                if (!GUIManager.CustomGUIFront)
                {
                    Debug.LogError("GUIManager CustomGUI is null");
                    return;
                }

                LoadMenu();
            }

            bool state = !ValheimLevelSystem.Menu.activeSelf;

            ValheimLevelSystem.Menu.SetActive(state);
        }

        public static void DestroyMenu()
        {
            ValheimLevelSystem.Menu.SetActive(false);
        }

        public static void LoadMenu()
        {
            if (Player.m_localPlayer == null) return;

            ValheimLevelSystem.Menu = GUIManager.Instance.CreateWoodpanel(
                                                                       parent: GUIManager.CustomGUIFront.transform,
                                                                       anchorMin: new Vector2(0.5f, 0.5f),
                                                                       anchorMax: new Vector2(0.5f, 0.5f),
                                                                       position: new Vector2(0, 0),
                                                                       width: 400,
                                                                       height: 700,
                                                                       draggable: true);
            ValheimLevelSystem.Menu.SetActive(false);

            GameObject scrollView = GUIManager.Instance.CreateScrollView(parent: ValheimLevelSystem.Menu.transform,
                    showHorizontalScrollbar: false,
                    showVerticalScrollbar: true,
                    handleSize: 8f,
                    handleColors: GUIManager.Instance.ValheimScrollbarHandleColorBlock,
                    handleDistanceToBorder: 50f,
                    slidingAreaBackgroundColor: new Color(0.1568628f, 0.1019608f, 0.0627451f, 1f),
                    width: 350f,
                    height: 270f
                );

            var tf = (RectTransform)scrollView.transform;
            tf.anchoredPosition = new Vector2(0, -50);
            scrollView.SetActive(true);

            scrollView.transform.Find("Scroll View").GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;

            GameObject textObject = GUIManager.Instance.CreateText(
                text: Player.m_localPlayer.GetPlayerName(),
                parent: ValheimLevelSystem.Menu.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(-100f, -50f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 25,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 150f,
                height: 40f,
                addContentSizeFitter: false);

            GameObject levelTextObject = GUIManager.Instance.CreateText(
                text: "Level: " + Level.GetLevel(),
                parent: ValheimLevelSystem.Menu.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(120f, -50f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 25,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 150f,
                height: 40f,
                addContentSizeFitter: false);

            ValheimLevelSystem.menuItems.Add("levelText", levelTextObject);

            int interfaceMultiplier = 0;
            List<GameObject> texts = new List<GameObject>();


            foreach (Skill skill in Enum.GetValues(typeof(PlayerSkills.Skill)).Cast<PlayerSkills.Skill>().ToList())
            {
                var skillLevel = Level.GetSkillLevel(skill);

                GameObject skillText = GUIManager.Instance.CreateText(
                    text: skill.ToString(),
                    parent: ValheimLevelSystem.Menu.transform,
                    anchorMin: new Vector2(0.5f, 1f),
                    anchorMax: new Vector2(0.5f, 1f),
                    position: new Vector2(-100f, -100f + (-28 * interfaceMultiplier)),
                    font: GUIManager.Instance.AveriaSerifBold,
                    fontSize: 18,
                    color: GUIManager.Instance.ValheimOrange,
                    outline: true,
                    outlineColor: Color.black,
                    width: 150f,
                    height: 25f,
                    addContentSizeFitter: false);

                GameObject buttonObject2 = GUIManager.Instance.CreateButton(
                          text: " + ",
                          parent: ValheimLevelSystem.Menu.transform,
                          anchorMin: new Vector2(0.5f, 0.5f),
                          anchorMax: new Vector2(0.5f, 0.5f),
                          position: new Vector2(110f, 252f + (-28 * interfaceMultiplier)),
                          width: 30f,
                          height: 25f);
                buttonObject2.SetActive(true);

                Button button2 = buttonObject2.GetComponent<Button>();
                button2.onClick.AddListener(delegate { SkillManager.SkillUp(skill); });


                GameObject levelText = GUIManager.Instance.CreateText(
                      text: skillLevel.ToString(),
                      parent: ValheimLevelSystem.Menu.transform,
                      anchorMin: new Vector2(0.5f, 1f),
                      anchorMax: new Vector2(0.5f, 1f),
                      position: new Vector2(70f, -100f + (-28 * interfaceMultiplier++)),
                      font: GUIManager.Instance.AveriaSerifBold,
                      fontSize: 17,
                      color: GUIManager.Instance.ValheimOrange,
                      outline: true,
                      outlineColor: Color.black,
                      width: 40f,
                      height: 35f,
                      addContentSizeFitter: false);

                ValheimLevelSystem.menuItems.Add(skill.ToString() + "Text", levelText);
            }

            GameObject availableEffectsTitle = GUIManager.Instance.CreateText(
                 text: "Available Effects:",
                 parent: ValheimLevelSystem.Menu.transform,
                   new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f),
                        new Vector2(-100f, 100f),
                 font: GUIManager.Instance.AveriaSerifBold,
                 fontSize: 15,
                 color: GUIManager.Instance.ValheimOrange,
                 outline: true,
                 outlineColor: Color.black,
                 width: 150f,
                 height: 20f,
                 addContentSizeFitter: false);

            CreateAvailableEffectListText(scrollView);

            scrollView.transform.Find("Scroll View").GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;

            GameObject expTextObject = GUIManager.Instance.CreateText(
                 text: "Exp: " + Level.GetExp() + "/" + Level.GetMaxExpForCurrentLevel(),
                 parent: ValheimLevelSystem.Menu.transform,
                 anchorMin: new Vector2(0.5f, 1f),
                 anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(-100f, -570),
                 font: GUIManager.Instance.AveriaSerifBold,
                 fontSize: 15,
                 color: GUIManager.Instance.ValheimOrange,
                 outline: true,
                 outlineColor: Color.black,
                 width: 150f,
                 height: 20f,
                 addContentSizeFitter: false);
            ValheimLevelSystem.menuItems.Add("ExpText", expTextObject);

            GameObject pointsAvailableObject = GUIManager.Instance.CreateText(
                     text: "Available Points: " + Level.GetAvailablePoints(),
                     parent: ValheimLevelSystem.Menu.transform,
                     anchorMin: new Vector2(0.5f, 1f),
                     anchorMax: new Vector2(0.5f, 1f),
                    position: new Vector2(-100f, -600f),
                     font: GUIManager.Instance.AveriaSerifBold,
                     fontSize: 15,
                     color: GUIManager.Instance.ValheimOrange,
                     outline: true,
                     outlineColor: Color.black,
                     width: 150f,
                     height: 40f,
                     addContentSizeFitter: false);
            ValheimLevelSystem.menuItems.Add("PlayerPointsAvailableText", pointsAvailableObject);

            GameObject resetButton = GUIManager.Instance.CreateButton(
              text: "Reset",
              parent: ValheimLevelSystem.Menu.transform,
              anchorMin: new Vector2(0.5f, 0.5f),
              anchorMax: new Vector2(0.5f, 0.5f),
              position: new Vector2(140, -230f),
              width: 80,
              height: 30f);
            resetButton.SetActive(true);

            Button buttonReset = resetButton.GetComponent<Button>();
            buttonReset.onClick.AddListener(GUIConfirm.CreateResetSkillMenu);

            GameObject buttonObject = GUIManager.Instance.CreateButton(
                text: "Close",
                parent: ValheimLevelSystem.Menu.transform,
                anchorMin: new Vector2(0.5f, 0.5f),
                anchorMax: new Vector2(0.5f, 0.5f),
                position: new Vector2(0, -300f),
                width: 170,
                height: 45f);
            buttonObject.SetActive(true);

            Button button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(DestroyMenu);
        }

        private static void CreateAvailableEffectListText(GameObject scrollView)
        {
            GameObject availableEffectsListText = GUIManager.Instance.CreateText(
                 text: "",
                 parent: scrollView.transform.Find("Scroll View/Viewport/Content"),
                 new Vector2(0.5f, 0.5f),
                 new Vector2(0.5f, 0.5f),
                 new Vector2(0f, 80f),
                 font: GUIManager.Instance.AveriaSerifBold,
                 fontSize: 15,
                 color: GUIManager.Instance.ValheimOrange,
                 outline: true,
                 outlineColor: Color.black,
                 width: 300f,
                 height: 150f,
                 addContentSizeFitter: false);

            ValheimLevelSystem.menuItems.Add("availableEffectsListText", availableEffectsListText);

            SetAvailableEffectListText();
        }

        public static void SetAvailableEffectListText()
        {

            GameObject txt;
            ValheimLevelSystem.menuItems.TryGetValue("availableEffectsListText", out txt);

            if (txt)
            {
                txt.GetComponent<Text>().text = "";
                SetStrengthAvailableEffectListText(txt);
                SetAgilityAvailableEffectListText(txt);
                SetIntelligenceAvailableEffectListText(txt);
                SetFocusAvailableEffectListText(txt);
                SetConstitutionAvailableEffectListText(txt);
            }
        }

        private static void SetConstitutionAvailableEffectListText(GameObject txt)
        {
            int skillLevel = Level.GetSkillLevel(Skill.Constitution);

            if (skillLevel <= 1) return;

            float passiveLifeRegen = (skillLevel / 100f) * (Constitution.PassiveLifeBonus.Value - 1) * 100;
            float passiveStaminaRegen = (skillLevel / 100f) * (Constitution.PassiveStaminaBonus.Value - 1) * 100;

            txt.GetComponent<Text>().text += "\n Con +" + Math.Round(passiveLifeRegen, 2) + "% Passive life bonus";
            txt.GetComponent<Text>().text += "\n Con +" + Math.Round(passiveStaminaRegen, 2) + "% Passive stamina bonus";

            if (skillLevel >= 50)
            {
                txt.GetComponent<Text>().text += "\n Con 50: +" + Math.Round((Constitution.Level50FoodDuration.Value - 1) * 100, 2) + "% Food duration";
                txt.GetComponent<Text>().text += "\n Con 50: +" + Math.Round((Constitution.Level50PhysicalDamageReduction.Value - 1) * 100, 2) + "% Physical damage reduction";
            }

            if (skillLevel >= 100)
            {
                txt.GetComponent<Text>().text += "\n Con 100: +" + Constitution.Level100LifeBonus.Value + " Life bonus";
                txt.GetComponent<Text>().text += "\n Con 100: +" + Math.Round(((Constitution.Level100PhysicalDamageReduction.Value - 1) * 100), 2) + "%  Physical damage reduction";
            }

            if (skillLevel >= 150)
            {
                txt.GetComponent<Text>().text += "\n Con 150: +" + Constitution.Level150BonusStaminaAndLife.Value + " Stamina and life bonus";
                txt.GetComponent<Text>().text += "\n Con 150: +" + (Constitution.Level150BonusArmor.Value - 1) * 100 + "% Armor bonus";
            }

            if (skillLevel >= 200)
            {
                txt.GetComponent<Text>().text += "\n Con 200: +" + (Constitution.Level200BonusArmor.Value - 1) * 100 + "% Armor bonus";
                txt.GetComponent<Text>().text += "\n Con 200: +" + (Constitution.Level200PhysicalDamageReduction.Value - 1) * 100 + "% Physical damage reduction";
            }
        }

        private static void SetFocusAvailableEffectListText(GameObject txt)
        {
            int skillLevel = Level.GetSkillLevel(Skill.Focus);

            if (skillLevel <= 1) return;

            float passiveLifeRegen = (skillLevel / 100f) * (Focus.PassiveLifeRegen.Value - 1) * 100;
            float passiveStaminaRegen = (skillLevel / 100f) * (Focus.PassiveStaminaRegen.Value - 1) * 100;

            txt.GetComponent<Text>().text += "\n Foc +" + Math.Round(passiveLifeRegen, 2) + "% Passive life regen";
            txt.GetComponent<Text>().text += "\n Foc +" + Math.Round(passiveStaminaRegen, 2) + "% Passive stamina regen";

            if (skillLevel >= 50)
            {
                txt.GetComponent<Text>().text += "\n Foc 50: +" + Math.Round((Focus.Level50ElementalReduction.Value - 1) * 100, 2) + "% Elemental damage reduction";
                txt.GetComponent<Text>().text += "\n Foc 50: +" + (Focus.Level50StaminaRegen.Value - 1) * 100 + "% Stamina regen";
            }

            if (skillLevel >= 100)
            {
                txt.GetComponent<Text>().text += "\n Foc 100: +" + Focus.Level100StaminaBonus.Value + " Stamina bonus";
                txt.GetComponent<Text>().text += "\n Foc 100: +" + ((Focus.Level100LifeRegen.Value - 1) * 100) + "% Life regen";
            }

            if (skillLevel >= 150)
            {
                txt.GetComponent<Text>().text += "\n Foc 150: +" + Focus.Level150BuffAndPotionsDuration.Value + " Boss buff and potions duration";
                txt.GetComponent<Text>().text += "\n Foc 150: +" + (Focus.Level150LifeBonus.Value) + " Life bonus";
            }

            if (skillLevel >= 200)
            {
                txt.GetComponent<Text>().text += "\n Foc 200: +" + (Focus.Level200ElementalReduction.Value - 1) * 100 + "% Elemental damage reduction";
                txt.GetComponent<Text>().text += "\n Foc 200: +" + (Focus.Level200LifeAndStaminaRegen.Value - 1) *100 + "% Life and stamina regen";
            }
        }

        private static void SetIntelligenceAvailableEffectListText(GameObject txt)
        {
            int skillLevel = Level.GetSkillLevel(Skill.Intelligence);

            if (skillLevel <= 1) return;

            float passiveMagicDamage = (skillLevel / 100f) * (Intelligence.PassiveMagicDamage.Value - 1) * 100;
            float passiveQuickLearner = (skillLevel / 100f) * (Intelligence.PassiveQuickLearner.Value - 1) *100;

            txt.GetComponent<Text>().text += "\n Int +" + Math.Round(passiveMagicDamage, 2) + "% Magic damage";
            txt.GetComponent<Text>().text += "\n Int +" + Math.Round(passiveQuickLearner, 2) + "% Quick learner";

            if (skillLevel >= 50)
            {
                txt.GetComponent<Text>().text += "\n Int 50: +" + Math.Round((Intelligence.Level50MoveSpeed.Value - 1) * 100, 2) + "% move speed";
                txt.GetComponent<Text>().text += "\n Int 50: +" + (Intelligence.Level50MagicDamage.Value - 1) * 100 + "% Magic damage";
            }                                        
                                                     
            if (skillLevel >= 100)                   
            {                                        
                txt.GetComponent<Text>().text += "\n Int 100: -" + Intelligence.Level100BuffAndPotionsCooldown.Value + "% Boss buff and potions cd";
                txt.GetComponent<Text>().text += "\n Int 100: +" + ((Intelligence.Level100QuickLearner.Value - 1) * 100) + "% Quick learner";
            }                                            
                                                         
            if (skillLevel >= 150)                       
            {                                            
                txt.GetComponent<Text>().text += "\n Int 150: +" + Intelligence.Level150AllSkillBonus.Value + " all skills points";
                txt.GetComponent<Text>().text += "\n Int 150: +" + (Intelligence.Level150MagicDamage.Value - 1) * 100 + "% Magic damage";
            }

            if (skillLevel >= 200)
            {
                txt.GetComponent<Text>().text += "\n Int 200: +" + (Intelligence.Level200Atackspeed.Value - 1) * 100 + "% Attack speed";
                txt.GetComponent<Text>().text += "\n Int 200: +" + Intelligence.Level200ComfortBonus.Value + " comfort";
            }
        }

        private static void SetAgilityAvailableEffectListText(GameObject txt)
        {
            int skillLevel = Level.GetSkillLevel(Skill.Agility);

            if (skillLevel <= 1) return;

            float bowPolearmMultiplier = (skillLevel / 100f) * Agility.PassiveBowPolearmMultiplier.Value;
            float spearKnifeMultiplier = (skillLevel / 100f) * Agility.PassiveSpearKnifeMultiplier.Value;

            txt.GetComponent<Text>().text += "\n Agi +" + bowPolearmMultiplier + "% Bow and polearm damage";
            txt.GetComponent<Text>().text += "\n Agi +" + spearKnifeMultiplier + "% Knife and spear damage";

            if (skillLevel >= 50)
            {
                txt.GetComponent<Text>().text += "\n Agi 50: +" + Math.Round((Agility.Level50RunSpeed.Value - 1) * 100,2) + "% move speed";
                txt.GetComponent<Text>().text += "\n Agi 50: +" + (Agility.Level50Backstab.Value - 1) * 100 + "% backstab damage";
            }                                            
                                                         
            if (skillLevel >= 100)                       
            {                                            
                txt.GetComponent<Text>().text += "\n Agi 100: +" + (Agility.Level100BowKnivesDamage.Value - 1) * 100 + "% Bow and knife damage";
                txt.GetComponent<Text>().text += "\n Agi 100: +" + (Agility.Level100JumAndRunStaminaReduction.Value - 1) * 100 + "% Run and Jump stamina";
             }                                            
                                                         
            if (skillLevel >= 150)                       
            {                                            
                txt.GetComponent<Text>().text += "\n Agi 150: +" + (Agility.Level150RunSpeed.Value - 1) * 100 + "% move speed";
                txt.GetComponent<Text>().text += "\n Agi 150:  +" + (Agility.Level150SpearAndPolearmDamage.Value - 1) * 100 + "% Spears and polearm damage";
            }                                            
                                                         
            if (skillLevel >= 200)                       
            {                                            
                txt.GetComponent<Text>().text += "\n Agi 200: +" + (Agility.Level200Backstab.Value - 1) * 100 + "% backstab";
                txt.GetComponent<Text>().text += "\n Agi 200: +" + (Agility.Level200RunSpeed.Value- 1) * 100 + "% movespeed";
                txt.GetComponent<Text>().text += "\n Agi 200: +" + (Agility.Level200BowKnivesDamage.Value - 1) * 100 + "% Knives and Bows damage";
            }
        }

        private static void SetStrengthAvailableEffectListText(GameObject txt)
        {
            int skillLevel = Level.GetSkillLevel(Skill.Strength);

            if (skillLevel <= 1) return;

            float twoHandedMultiplier = (skillLevel / 100f) * Strength.PassiveTwoHandedMultiplier.Value;
            float oneHandedMultiplier = (skillLevel / 100f) * Strength.PassiveOneHandedMultiplier.Value;
            float chopAndPickaxeMultiplier = (skillLevel / 100f) * Strength.PassiveChopAndPickaxeMultiplier.Value;

            txt.GetComponent<Text>().text += "\n Str +" + twoHandedMultiplier + "% Two-handed damage";
            txt.GetComponent<Text>().text += "\n Str +" + oneHandedMultiplier + "% One-handed damage";
            txt.GetComponent<Text>().text += "\n Str +" + chopAndPickaxeMultiplier + "% Chop and pickaxe damage";

            if (skillLevel >= 50)
            {
                txt.GetComponent<Text>().text += "\n Str 50: +" + (Strength.Level50OneHandedDamage.Value - 1) * 100 + "% One-handed damage";
                txt.GetComponent<Text>().text += "\n Str 50: +" + Strength.Level50CarryWeight.Value + " carry weight";
            }

            if (skillLevel >= 100)
            {
                txt.GetComponent<Text>().text += "\n Str 100:  +" + (Strength.Level100TwoHandedDamage.Value - 1) * 100 + "% Two-handed damage";
                txt.GetComponent<Text>().text += "\n Str 100:  -" + (Strength.Level100ReduceAttackStamina.Value - 1) * 100 + "% Attack stamina";
            }

            if (skillLevel >= 150)
            {
                txt.GetComponent<Text>().text += "\n Str 150: +" + (Strength.Level150SpeedMultiplierOneHanded.Value - 1) * 100 + " One-handed speed";
                txt.GetComponent<Text>().text += "\n Str 150:  +" + (Strength.Level150SwordsDamage.Value - 1) * 100 + "% Sword damage";
                txt.GetComponent<Text>().text += "\n Str 150:  +" + Strength.Level150CarryWeight.Value + "carry weight";
            }

            if (skillLevel >= 200)
            {
                txt.GetComponent<Text>().text += "\n Str 200: +" + (Strength.Level200OneTwoHandedDamage.Value - 1) * 100 + "% damage";
                txt.GetComponent<Text>().text += "\n Str 200: +" + Math.Round((Strength.Level200SpeedMultiplierTwoHanded.Value - 1) * 100, 2) + "% Two-handed speed";
            }
        }

        public static void UpdatePlayerPointsAvailable()
        {
            GameObject txt;
            ValheimLevelSystem.menuItems.TryGetValue("PlayerPointsAvailableText", out txt);

            if (txt)
            {
                txt.GetComponent<Text>().text = "Available Points: " + Level.GetAvailablePoints();
            }
        }

        public static void UpdateSkillLevelText(string skill, string level)
        {
            GameObject txt;
            ValheimLevelSystem.menuItems.TryGetValue(skill + "Text", out txt);

            if (txt)
            {
                txt.GetComponent<Text>().text = level + "    " + skill;
            }
            SetAvailableEffectListText();
        }

        public static void UpdateExpText()
        {
            GameObject txt;
            ValheimLevelSystem.menuItems.TryGetValue("ExpText", out txt);

            if (txt)
            {
                txt.GetComponent<Text>().text = "Exp: " + Level.GetExp() + "/" + Level.GetMaxExpForCurrentLevel();
            }
        }

        public static void UpdatePlayerLevelText()
        {
            GameObject txt;
            ValheimLevelSystem.menuItems.TryGetValue("levelText", out txt);

            if (txt)
            {
                txt.GetComponent<Text>().text = "Level: " + Level.GetLevel();
            }
        }

        public static void AddAvailableEffectText(string text)
        {
            GameObject txt;
            ValheimLevelSystem.menuItems.TryGetValue("availableEffectsListText", out txt);

            if (txt)
            {
                txt.GetComponent<Text>().text += "* " + text;
            }
        }
    }
}
