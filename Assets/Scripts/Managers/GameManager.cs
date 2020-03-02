using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            gameObject.AddComponent<UserManager>();
            
            SceneManager.LoadScene(1);
        }

    }
}