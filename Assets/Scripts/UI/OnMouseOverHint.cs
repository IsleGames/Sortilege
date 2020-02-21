using UnityEngine;

namespace UI
{
    public class OnMouseOverHint : MonoBehaviour
    {
        void OnMouseOver()
        {
            Debug.Log("Mouse is over " + gameObject.name + " at " + Time.time);
        }
    
        void OnMouseExit()
        {
            Debug.Log("Mouse is no longer on " + gameObject.name + " at " + Time.time);
        }
    }
}