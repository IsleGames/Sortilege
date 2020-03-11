using System;
using System.Collections;
using Library;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private GameObject sceneFaderPrefab;

        [SerializeField]
        private int roundCount;
        
        private void Start()
        {
            sceneFaderPrefab = (GameObject)Resources.Load("Prefabs/SceneFader");
            
            gameObject.AddComponent<UserManager>();
            
            LoadSceneByName("StartMenu", false);
        }

        public void StartGame()
        {
            roundCount = 0;
            
            LoadSceneByName("Battle", true, 1.5f);
        }

        public int GetRoundCount()
        {
            return roundCount;
        }

        public void EndBattle()
        {
            if (roundCount < Game.Ctx.UserOperator.totalRound - 1)
            {
                LoadSceneByName("AddCard", true, 1.5f);
            }
            else
            {
                Game.Ctx.VfxOperator.ShowTurnText("Game Clear!", true);
                
                Game.Ctx.AnimationOperator.PushAction(Utilities.WaitForSecs(20f), true);
                LoadSceneByName("StartMenu");
            }
        }

        public void EnterBattle()
        {
            roundCount += 1;
            LoadSceneByName("Battle", true, 1.5f);
        }
        
        public void LoadSceneByName(string sceneName, bool fade = true, float fadeTime = 1.5f)
        {
            IEnumerator loadIEnum = LoadSceneByNameIEnum(sceneName);
            
            if (fade)
            {
                GameObject sceneFader = Instantiate(sceneFaderPrefab);
                sceneFader.GetComponent<SceneFader>().FadeAndLoadScene(loadIEnum, fadeTime);
            }
            else
            {
                StartCoroutine(loadIEnum);
            }
        }

        public IEnumerator LoadSceneByNameIEnum(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            yield return null;
        }

        public void EndGame()
        {

#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

    }
}