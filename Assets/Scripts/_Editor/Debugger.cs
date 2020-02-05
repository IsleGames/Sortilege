using UnityEngine;
using System.Collections;

namespace _Editor
{
    public class Debugger : MonoBehaviour
    {
        public static bool EnableDebugOutput = true;

        public static void Log<T>(T obj)
        {
            if (EnableDebugOutput)
                Debug.Log(obj.ToString());
        }
    }
}