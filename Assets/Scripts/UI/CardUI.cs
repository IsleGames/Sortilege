using System;
using System.Collections;
using UnityEngine;
using _Editor;
using Cards;

// This should maybe just be Collider? 
// We'll see how many dimensions we use
[RequireComponent(typeof(Collider2D))]
public class CardUI : MonoBehaviour {

    public bool canPlay = true; // TODO: default to false, check in update() based on game state
    public bool beingPlayed = false;
    public float moveSpeed = 0.1f;

    void Awake()
    {
        GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>() as Camera;
    }

    private Cards.Card card;
    Vector3 home;
    private Vector3 cursorhome;

    public void SetCard(Cards.Card c)
    {
        card = c;
    }

    public void setCallbacks()
    {
        card.onDiscard.AddListener(() => Hide());
        card.onPlay.AddListener(() => Debugger.Log("Played " + card.GetComponent<MetaData>().name));
        card.onPlay.AddListener(() => Board.Ctx.Queue.AddObject(gameObject, 0.1f));
        card.onPlay.AddListener(() => Board.Ctx.Hand.RemoveObject(gameObject));

        card.onDraw.AddListener(() => Debugger.Log("Drew " + card.GetComponent<MetaData>().name));
        card.onDraw.AddListener(() => Show());
        card.onDraw.AddListener(() => Board.Ctx.Hand.AddObject(gameObject, 5f));

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
        var mover = collider.gameObject.GetComponent<PlayArea>();
        if (mover != null) beingPlayed = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // leaving play zone
        if (collider.gameObject.GetComponent<PlayArea>() != null)
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
            //TODO: get rid of this try/
            try
            {
                Game.Ctx.CardOperator.AddCardToQueue(card);
            }
            catch (InvalidOperationException)
            { }// this is a really bad hack 
            
            //Move to discard pile
            // Note: the movement should maybe be done with springs,
            // rather than scripts?

            //StartCoroutine(MoveCard(discardPile.position, 0.5f));
            canPlay = false;
        }
    }

    public void Hide()
    {
        
        transform.Find("CardName").GetComponent<TMPro.TextMeshProUGUI>().enabled = false;
        transform.Find("CardText").GetComponent<TMPro.TextMeshProUGUI>().enabled = false;
        transform.Find("AttributeSprite").GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("CardBackground").GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("CardBorder").GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Show()
    {
        transform.Find("AttributeSprite").GetComponent<SpriteRenderer>().enabled = true;
        transform.Find("CardBackground").GetComponent<SpriteRenderer>().enabled = true;
        transform.Find("CardBorder").GetComponent<SpriteRenderer>().enabled = true;
        transform.Find("CardName").GetComponent<TMPro.TextMeshProUGUI>().enabled = true;
        transform.Find("CardText").GetComponent<TMPro.TextMeshProUGUI>().enabled = true;

    }


}
