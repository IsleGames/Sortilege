using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Button))]
public class TestButton : MonoBehaviour
{


    public void Log()
    {
        Debug.Log("pressed button");
    }
}
