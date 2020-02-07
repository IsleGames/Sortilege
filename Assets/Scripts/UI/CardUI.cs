using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This should maybe just be Collider? 
// We'll see how many dimensions we use
[RequireComponent(typeof(Collider2D))] 
public class CardUI : MonoBehaviour {

    public bool canPlay = true; // TODO: default to false, check in update() based on game state
    public bool beingPlayed = false;
    public float moveSpeed = 1f;
    public Transform discardPile;
    Vector3 home;

    public void OnMouseDown()
    {
        home = transform.position;
    }

    public void OnMouseDrag()
    {
        if (canPlay)
        {
            var cursorPosition = Input.mousePosition;
            var cursorPositionWorld = Camera.main.ScreenToWorldPoint(cursorPosition);
            gameObject.transform.position = new Vector3(cursorPositionWorld.x, cursorPositionWorld.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var mover = collider.gameObject.GetComponent<PlayZone>();
        if (mover != null) beingPlayed = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // leaving play zone
        if (collider.gameObject.GetComponent<PlayZone>() != null)
        { 
            beingPlayed = false;
        }
    }

    IEnumerator MoveCard(Vector3 dest, float delay = 0)
    {
        Vector3 init = new Vector3(transform.position.x, transform.position.y);
        float t = 0f;
        yield return new WaitForSeconds(delay);
        while (t < moveSpeed)
            
        {
            float i = t / moveSpeed;
            transform.SetPositionAndRotation(i * dest + (1f - i) * init,
                transform.rotation);
            t += Time.deltaTime;
            yield return null;
        }
    }

    private void OnMouseUp()
    {
        if (beingPlayed)
        {
            //play this card
            //getComponent<Card>().play()

            //Move to discard pile
            // Note: the movement should maybe be done with springs,
            // rather than scripts?

            StartCoroutine(MoveCard(discardPile.position, 0.5f));
            canPlay = false;
        }
        else
        {
            StartCoroutine(MoveCard(home, 0f));
        }
    }


}
