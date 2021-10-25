using Jotunn.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MMRPGSkillSystem
{
    public class GUIConfirm
    {
        public static GameObject menu;

        public static void CreateResetSkillMenu()
        {
            GUI.DestroyMenu();

            if (!menu && Player.m_localPlayer)
            {   
                menu = GUIManager.Instance.CreateWoodpanel(
                                        parent: GUIManager.CustomGUIFront.transform,
                                        anchorMin: new Vector2(0.5f, 0.5f),
                                        anchorMax: new Vector2(0.5f, 0.5f),
                                        position: new Vector2(0, 0),
                                        width: 300,
                                        height: 220,
                                        draggable: true);
                MMRPGSkillSystem.Menu.SetActive(false);

                GameObject textObject = GUIManager.Instance.CreateText(
                    text: "Reset skills",
                    parent: menu.transform,
                    anchorMin: new Vector2(0.5f, 1f),
                    anchorMax: new Vector2(0.5f, 1f),
                    position: new Vector2(0f, -40f),
                    font: GUIManager.Instance.AveriaSerifBold,
                    fontSize: 25,
                    color: GUIManager.Instance.ValheimOrange,
                    outline: true,
                    outlineColor: Color.black,
                    width: 150f,
                    height: 40f,
                    addContentSizeFitter: false);

                GameObject yesButton = GUIManager.Instance.CreateButton(
                   text: "Yes",
                   parent: menu.transform,
                   anchorMin: new Vector2(0.5f, 0.5f),
                   anchorMax: new Vector2(0.5f, 0.5f),
                   position: new Vector2(70, -30),
                   width: 100,
                   height: 50f);
                yesButton.SetActive(true);

                Button buttonYes = yesButton.GetComponent<Button>();
                buttonYes.onClick.AddListener(Reset.ResetSkills);

                GameObject noButton = GUIManager.Instance.CreateButton(
                   text: "No",
                   parent: menu.transform,
                   anchorMin: new Vector2(0.5f, 0.5f),
                   anchorMax: new Vector2(0.5f, 0.5f),
                   position: new Vector2(-70, -30),
                   width: 100,
                   height: 50f);
                noButton.SetActive(true);

                Button buttonNo = noButton.GetComponent<Button>();
                buttonNo.onClick.AddListener(DestroyMenu);
            }

            bool state = !MMRPGSkillSystem.Menu.activeSelf;

            menu.SetActive(state);
        }


        public static void DestroyMenu()
        {
            menu.SetActive(false);
        }
    }
}
