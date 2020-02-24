using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class UserManager : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(1);
        }
    }
}