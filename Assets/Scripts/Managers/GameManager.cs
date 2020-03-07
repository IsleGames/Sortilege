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
            
            LoadSceneByName("StartMenu");
        }

        public void LoadSceneByName(string sceneName, bool fade = false, float fadeTime = 1.5f)
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

        private IEnumerator LoadSceneByNameIEnum(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            yield return null;
        }

    }
}