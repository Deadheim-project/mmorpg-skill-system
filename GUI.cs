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
            if (!MMRPGSkillSystem.Menu && Player.m_localPlayer)
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

            bool state = !MMRPGSkillSystem.Menu.activeSelf;

            MMRPGSkillSystem.Menu.SetActive(state);
        }

        public static void DestroyMenu()
        {
            MMRPGSkillSystem.Menu.SetActive(false);
        }

        public static void LoadMenu()
        {
            if (Player.m_localPlayer == null) return;

            MMRPGSkillSystem.Menu = GUIManager.Instance.CreateWoodpanel(
                                                                       parent: GUIManager.CustomGUIFront.transform,
                                                                       anchorMin: new Vector2(0.5f, 0.5f),
                                                                       anchorMax: new Vector2(0.5f, 0.5f),
                                                                       position: new Vector2(0, 0),
                                                                       width: 400,
                                                                       height: 700,
                                                                       draggable: true);
            MMRPGSkillSystem.Menu.SetActive(false);

            GameObject scrollView = GUIManager.Instance.CreateScrollView(parent: MMRPGSkillSystem.Menu.transform,
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
                parent: MMRPGSkillSystem.Menu.transform,
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
                parent: MMRPGSkillSystem.Menu.transform,
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

            MMRPGSkillSystem.menuItems.Add("levelText", levelTextObject);

            int interfaceMultiplier = 0;
            List<GameObject> texts = new List<GameObject>();


            foreach (Skill skill in Enum.GetValues(typeof(PlayerSkills.Skill)).Cast<PlayerSkills.Skill>().ToList())
            {
                var skillLevel = Level.GetSkillLevel(skill);

                GameObject skillText = GUIManager.Instance.CreateText(
                    text: skill.ToString(),
                    parent: MMRPGSkillSystem.Menu.transform,
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
                          parent: MMRPGSkillSystem.Menu.transform,
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
                      parent: MMRPGSkillSystem.Menu.transform,
                      anchorMin: new Vector2(0.5f, 1f),
                      anchorMax: new Vector2(0.5f, 1f),
                      position: new Vector2(70f, -100f + (-28 * interfaceMultiplier++)),
                      font: GUIManager.Instance.AveriaSerifBold,
                      fontSize: 18,
                      color: GUIManager.Instance.ValheimOrange,
                      outline: true,
                      outlineColor: Color.black,
                      width: 30f,
                      height: 25f,
                      addContentSizeFitter: false);

                MMRPGSkillSystem.menuItems.Add(skill.ToString() + "Text", levelText);
            }

            GameObject availableEffectsTitle = GUIManager.Instance.CreateText(
                 text: "Available Effects:",
                 parent: MMRPGSkillSystem.Menu.transform,
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
                 parent: MMRPGSkillSystem.Menu.transform,
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
            MMRPGSkillSystem.menuItems.Add("ExpText", expTextObject);

            GameObject pointsAvailableObject = GUIManager.Instance.CreateText(
                     text: "Available Points: " + Level.GetAvailablePoints(),
                     parent: MMRPGSkillSystem.Menu.transform,
                     anchorMin: new Vector2(0.5f, 1f),
                     anchorMax: new Vector2(0.5f, 1f),
                    position: new Vector2(-100f, -600f),
                     font: GUIManager.Instance.AveriaSerifBold,
                     fontSize: 15,
                     color: GUIManager.Instance.ValheimOrange,
                     outline: true,
                     outlineColor: Color.black,
                     width: 150f,
                     height: 20f,
                     addContentSizeFitter: false);
            MMRPGSkillSystem.menuItems.Add("PlayerPointsAvailableText", pointsAvailableObject);

            GameObject resetButton = GUIManager.Instance.CreateButton(
              text: "Reset",
              parent: MMRPGSkillSystem.Menu.transform,
              anchorMin: new Vector2(0.5f, 0.5f),
              anchorMax: new Vector2(0.5f, 0.5f),
              position: new Vector2(140, -240f),
              width: 80,
              height: 30f);
            resetButton.SetActive(true);

            Button buttonReset = resetButton.GetComponent<Button>();
            buttonReset.onClick.AddListener(GUIConfirm.CreateResetSkillMenu);

            GameObject buttonObject = GUIManager.Instance.CreateButton(
                text: "Close",
                parent: MMRPGSkillSystem.Menu.transform,
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

            MMRPGSkillSystem.menuItems.Add("availableEffectsListText", availableEffectsListText);

            SetAvailableEffectListText();
        }

        public static void SetAvailableEffectListText()
        {

            GameObject txt;
            MMRPGSkillSystem.menuItems.TryGetValue("availableEffectsListText", out txt);

            if (txt)
            {
                txt.GetComponent<Text>().text = "";
                SetStrengthAvailableEffectListText(txt);
                SetAgilityAvailableEffectListText(txt);
            }
        }

        private static void SetAgilityAvailableEffectListText(GameObject txt)
        {
            int skillLevel = Level.GetSkillLevel(Skill.Agility);

            if (skillLevel <= 1) return;

            float bowPolearmMultiplier = (skillLevel / 100f) * Agility.PassiveBowPolearmMultiplier;
            float spearKnifeMultiplier = (skillLevel / 100f) * Agility.PassiveSpearKnifeMultiplier;

            txt.GetComponent<Text>().text += "\n +" + bowPolearmMultiplier + "% Bow and polearm damage";
            txt.GetComponent<Text>().text += "\n +" + spearKnifeMultiplier + "% Knife and spear damage";

            if (skillLevel >= 50)
            {
                txt.GetComponent<Text>().text += "\n Agility 50: +5% move speed";
                txt.GetComponent<Text>().text += "\n Agility 50: +10% backstab damage";
            }

            if (skillLevel >= 100)
            {
                txt.GetComponent<Text>().text += "\n Agility 100: +10% Bow and knife damage";
                txt.GetComponent<Text>().text += "\n Agility 100: -10% Run and Jump stamina";
            }

            if (skillLevel >= 150)
            {
                txt.GetComponent<Text>().text += "\n Agility 150: +12% move speed";
                txt.GetComponent<Text>().text += "\n Agility 150: +20% Spears and polearm damage";
            }

            if (skillLevel >= 200)
            {
                txt.GetComponent<Text>().text += "\n Agility 200: +20% backstab";
                txt.GetComponent<Text>().text += "\n Agility 200: +10% movespeed";
                txt.GetComponent<Text>().text += "\n Agility 200: +20% Knives and Bows damage";
            }
        }

        private static void SetStrengthAvailableEffectListText(GameObject txt)
        {
            int skillLevel = Level.GetSkillLevel(Skill.Strength);

            if (skillLevel <= 1) return;

            float twoHandedMultiplier = (skillLevel / 100f) * Strength.PassiveTwoHandedMultiplier;
            float oneHandedMultiplier = (skillLevel / 100f) * Strength.PassiveOneHandedMultiplier;
            float chopAndPickaxeMultiplier = (skillLevel / 100f) * Strength.PassiveChopAndPickaxeMultiplier;

            txt.GetComponent<Text>().text += "\n +" + twoHandedMultiplier + "% Two-handed damage";
            txt.GetComponent<Text>().text += "\n +" + oneHandedMultiplier + "% One-handed damage";
            txt.GetComponent<Text>().text += "\n +" + chopAndPickaxeMultiplier + "% Chop and pickaxe damage";

            if (skillLevel >= 50)
            {
                txt.GetComponent<Text>().text += "\n Strength 50: +10% One-handed damage";
                txt.GetComponent<Text>().text += "\n Strength 50: +50 carry weight";
            }

            if (skillLevel >= 100)
            {
                txt.GetComponent<Text>().text += "\n Strength 100: +20% Two-handed damage";
                txt.GetComponent<Text>().text += "\n Strength 100: -10% Attack stamina";
            }

            if (skillLevel >= 150)
            {
                txt.GetComponent<Text>().text += "\n Strength 150: +10% One-handed speed";
                txt.GetComponent<Text>().text += "\n Strength 150: +20% Sword damage";
                txt.GetComponent<Text>().text += "\n Strength 150: +50 carry weight";
            }

            if (skillLevel >= 200)
            {
                txt.GetComponent<Text>().text += "\n Strength 200: +15% damage";
                txt.GetComponent<Text>().text += "\n Strength 200: +5% Two-handed speed";
            }
        }

        public static void UpdatePlayerPointsAvailable()
        {
            GameObject txt;
            MMRPGSkillSystem.menuItems.TryGetValue("PlayerPointsAvailableText", out txt);

            if (txt)
            {
                txt.GetComponent<Text>().text = "Available Points: " + Level.GetAvailablePoints();
            }
        }

        public static void UpdateSkillLevelText(string skill, string level)
        {
            GameObject txt;
            MMRPGSkillSystem.menuItems.TryGetValue(skill + "Text", out txt);

            if (txt)
            {
                txt.GetComponent<Text>().text = level + "    " + skill;
            }
            SetAvailableEffectListText();
        }

        public static void UpdateExpText()
        {
            GameObject txt;
            MMRPGSkillSystem.menuItems.TryGetValue("ExpText", out txt);

            if (txt)
            {
                txt.GetComponent<Text>().text = "Exp: " + Level.GetExp() + "/" + Level.GetMaxExpForCurrentLevel();
            }
        }

        public static void UpdatePlayerLevelText()
        {
            GameObject txt;
            MMRPGSkillSystem.menuItems.TryGetValue("levelText", out txt);

            if (txt)
            {
                txt.GetComponent<Text>().text = "Level: " + Level.GetLevel();
            }
        }

        public static void AddAvailableEffectText(string text)
        {
            GameObject txt;
            MMRPGSkillSystem.menuItems.TryGetValue("availableEffectsListText", out txt);

            if (txt)
            {
                txt.GetComponent<Text>().text += "* " + text;
            }
        }
    }
}
