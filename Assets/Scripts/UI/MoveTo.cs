using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MoveTo : MonoBehaviour
{
    public Transform destination;
    public float duration = 0.1f;


    IEnumerator MoveCard(GameObject g)
    {
        Vector3 init = g.transform.position;
        float t = 0f;
        while (t < duration)
        {
            float i = t / duration;
            g.transform.SetPositionAndRotation(i * destination.position + (1f - i) * init,
                g.transform.rotation);
            t += Time.deltaTime;
            yield return null;
        }
    }
    

}