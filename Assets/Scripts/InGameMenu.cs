using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class InGameMenu : MonoBehaviour
{

    Canvas menu;
    
    void Awake()
    {
        menu = GetComponent<Canvas>();
        menu.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menu.enabled)
            {
                ShowMenu();
            }
            else
            {
                HideMenu();
            }
        }
    }

    public void ShowMenu()
    {
        menu.enabled = true;
    }

    public void HideMenu()
    {
        menu.enabled = false;
    }

    public void QuitToMainMenu()
    {
        Game.Ctx.GameOperator.LoadSceneByName("StartMenu");
    }
}
