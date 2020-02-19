using System.Collections;
using _Editor;
using Cards;
using TMPro;
using UI;
using UnityEngine;

// This should maybe just be Collider? 
// We'll see how many dimensions we use
// Ans: If the game is in 2D, there is no need to apply 3D collider
[RequireComponent(typeof(Collider2D))]
public class CardUI : MonoBehaviour {

    public bool canPlay = true; // TODO: default to false, check in update() based on game state
    public bool beingPlayed;
    public float moveSpeed = 0.1f;

    void Awake()
    {
        GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
    }

    private Card card;
    Vector3 home;
    private Vector3 cursorhome;

    public void SetCard()
    {
        card = GetComponent<Card>();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayPile>()) beingPlayed = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // leaving play zone
        if (other.gameObject.GetComponent<PlayPile>())
        { 
            beingPlayed = false;
        }
    }

    IEnumerator MoveCard(Vector3 dest, float delay = 0)
    {
        Vector3 init = new Vector3(transform.position.x, transform.position.y);
        float t = 0f;
        yield return new WaitForSeconds(delay);
        while (t < moveSpeed) {
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
            // TODO: Add meta data check

            Game.Ctx.CardOperator.AddCardToQueue(card); 
            
            //Move to discard pile
            // Note: the movement should maybe be done with springs,
            // rather than scripts?

            //StartCoroutine(MoveCard(discardPile.position, 0.5f));
            canPlay = false;
        }
    }

    public void Hide()
    {
        TextMeshProUGUI[] tmPros = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var r in tmPros) {
            r.enabled = false;
        }
        SpriteRenderer[] spRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in spRenderers) {
            r.enabled = false;
        }
    }

    public void Show()
    {
        TextMeshProUGUI[] tmPros = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var r in tmPros) {
            r.enabled = true;
        }
        SpriteRenderer[] spRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in spRenderers) {
            r.enabled = true;
        }
    }

}
