
using UnityEngine;
using Managers;
using UnityEngine.UI;

public class MainMenuSceneQuitButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager gameOperator = FindObjectOfType<GameManager>();
        GetComponent<Button>().onClick.AddListener(gameOperator.EndGame);

    }
    
}
