using System;
using System.Collections;
using UnityEngine;

// This should maybe just be Collider? 
// We'll see how many dimensions we use
[RequireComponent(typeof(Collider2D))]
public class CardUI : MonoBehaviour {

    public bool canPlay = true; // TODO: default to false, check in update() based on game state
    public bool beingPlayed = false;
    public float moveSpeed = 0.1f;
    public Transform discardPile;

    private Cards.Card card;
    Vector3 home;
    private Vector3 cursorhome;

    public void SetCard(Cards.Card c)
    {
        card = c;
    }

    public void OnMouseDown()
    {
        home = transform.position;
        cursorhome = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnMouseDrag()
    {
        if (canPlay)
        {
            var cursorPositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.position = home + cursorPositionWorld - cursorhome;
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
            Debug.Log("Playing");
            // TODO: get rid of this try
            // Follow up: throw new exceptions doesn't work here
            // Switched to Debugger.Error (basically Debug.Error)
            
            Game.Ctx.CardOperator.PlayCard(card);
            
            Hide();
            
            //Move to discard pile
            // Note: the movement should maybe be done with springs,
            // rather than scripts?

            //StartCoroutine(MoveCard(discardPile.position, 0.5f));
            canPlay = false;
        }
        /*
        else
        {
            StartCoroutine(MoveCard(home, 0f));
        }*/
    }

    public void Hide()
    {
        Debug.Log("Hiding");
        GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("CardName").GetComponent<TMPro.TextMeshProUGUI>().enabled = false;
        transform.Find("CardText").GetComponent<TMPro.TextMeshProUGUI>().enabled = false;
        transform.Find("AttributeSprite").GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Show()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        transform.Find("CardName").GetComponent<TMPro.TextMeshProUGUI>().enabled = true;
        transform.Find("CardText").GetComponent<TMPro.TextMeshProUGUI>().enabled = true;
        transform.Find("AttributeSprite").GetComponent<SpriteRenderer>().enabled = true;
    }


}
