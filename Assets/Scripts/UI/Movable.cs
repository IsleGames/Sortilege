using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This should maybe just be Collider? 
// We'll see how many dimensions we use
[RequireComponent(typeof(Collider2D))] 
public class Movable : MonoBehaviour {

    public void Update()
    {
    }

    public void OnMouseDrag()
    {
        var cursorPosition = Input.mousePosition;
        var cursorPositionWorld = Camera.main.ScreenToWorldPoint(cursorPosition);
        gameObject.transform.position = new Vector3(cursorPositionWorld.x, cursorPositionWorld.y);
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked Me");
    }


}
