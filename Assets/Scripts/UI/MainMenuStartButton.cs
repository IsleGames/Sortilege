using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuStartButton : MonoBehaviour
    {
        private void Start()
        {
            GameManager gameOperator = FindObjectOfType<GameManager>();
            GetComponent<Button>().onClick.AddListener(delegate { gameOperator.LoadSceneByName("Battle", true, 1.5f); });
        }
    }
}