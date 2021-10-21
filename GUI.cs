using Jotunn.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            MMRPGSkillSystem.Menu = GUIManager.Instance.CreateWoodpanel(
                                                                       parent: GUIManager.CustomGUIFront.transform,
                                                                       anchorMin: new Vector2(0.5f, 0.5f),
                                                                       anchorMax: new Vector2(0.5f, 0.5f),
                                                                       position: new Vector2(0, 0),
                                                                       width: 400,
                                                                       height: 600,
                                                                       draggable: true);
            MMRPGSkillSystem.Menu.SetActive(false);


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
            foreach (string skill in MMRPGSkillSystem.playerSkills)
            {
                GameObject buttonObject2 = GUIManager.Instance.CreateButton(
                          text: " + ",
                          parent: MMRPGSkillSystem.Menu.transform,
                          anchorMin: new Vector2(0.5f, 0.5f),
                          anchorMax: new Vector2(0.5f, 0.5f),
                          position: new Vector2(50f, 200f + (-25 * interfaceMultiplier)),
                          width: 25f,
                          height: 20f);
                buttonObject2.SetActive(true);

                Button button2 = buttonObject2.GetComponent<Button>();
                button2.onClick.AddListener(delegate { SkillService.SkillUp(skill); });

                GameObject textObject2 = GUIManager.Instance.CreateText(
                    text: skill + " - " + Level.GetSkillLevel(skill),
                    parent: MMRPGSkillSystem.Menu.transform,
                    anchorMin: new Vector2(0.5f, 1f),
                    anchorMax: new Vector2(0.5f, 1f),
                    position: new Vector2(-100f, -100f + (-25 * interfaceMultiplier++)),
                    font: GUIManager.Instance.AveriaSerifBold,
                    fontSize: 15,
                    color: GUIManager.Instance.ValheimOrange,
                    outline: true,
                    outlineColor: Color.black,
                    width: 150f,
                    height: 20f,
                    addContentSizeFitter: false);

                MMRPGSkillSystem.menuItems.Add(skill.ToString() + "Text", textObject2);
            }

            GameObject expTextObject = GUIManager.Instance.CreateText(
                 text: "Exp: " + Level.GetExp() + "/" + Level.GetMaxExpForCurrentLevel(),
                 parent: MMRPGSkillSystem.Menu.transform,
                 anchorMin: new Vector2(0.5f, 1f),
                 anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(-100f, -550f),
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
                    position: new Vector2(-100f, -500f),
                     font: GUIManager.Instance.AveriaSerifBold,
                     fontSize: 15,
                     color: GUIManager.Instance.ValheimOrange,
                     outline: true,
                     outlineColor: Color.black,
                     width: 150f,
                     height: 20f,
                     addContentSizeFitter: false);

            MMRPGSkillSystem.menuItems.Add("PlayerPointsAvailableText", pointsAvailableObject);

            GameObject buttonObject = GUIManager.Instance.CreateButton(
                text: "Close",
                parent: MMRPGSkillSystem.Menu.transform,
                anchorMin: new Vector2(0.5f, 0.5f),
                anchorMax: new Vector2(0.5f, 0.5f),
                position: new Vector2(0, -250f),
                width: 100,
                height: 60f);
            buttonObject.SetActive(true);

            Button button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(DestroyMenu);
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
                txt.GetComponent<Text>().text = skill + " - "  + level;
            }
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
    }
}
