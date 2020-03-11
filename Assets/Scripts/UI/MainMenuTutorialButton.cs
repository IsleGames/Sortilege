using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuTutorialButton : MonoBehaviour
    {
        private void Start()
        {
            GameManager gameOperator = FindObjectOfType<GameManager>();
            GetComponent<Button>().onClick.AddListener(gameOperator.StartTutorial);
        }
    }
}