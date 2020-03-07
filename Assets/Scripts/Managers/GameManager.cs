using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private GameObject sceneFaderPrefab;
        private void Start()
        {
            sceneFaderPrefab = (GameObject)Resources.Load("Prefabs/SceneFader");
            
            gameObject.AddComponent<UserManager>();
            
            LoadSceneByName("StartMenu", false);
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

    }
}