using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuSceneQuitButton : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GameManager gameOperator = FindObjectOfType<GameManager>();
            GetComponent<Button>().onClick.AddListener(delegate { gameOperator.EndGame(); });
        }
    
    }
}
