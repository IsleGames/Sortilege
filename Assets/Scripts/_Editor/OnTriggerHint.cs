using UnityEngine;

namespace UI
{
    public class OnTriggerHint : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Triggered " + gameObject.name + " with " + other.gameObject.name + " at " + Time.time);
        }
    
        void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("No longer trigger " + gameObject.name + " with " + other.gameObject.name + " at " + Time.time);
        }
    }
}