using System;
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

    public bool movable = true; // TODO: default to false, check in update() based on game state
    
    [SerializeField]
    private bool triggerPlayArea, triggerHandArea;
    
    public float moveSpeed = 0.1f;

    void Start()
    {
        GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
    }

    // private Card card;
    Vector3 home;
    private Vector3 cursorhome;

    public void OnMouseDown()
    {
        home = transform.position;
        cursorhome = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Debugger.Log(GetComponent<MetaData>().title + " MouseDown");
    }

    public void OnMouseDrag()
    {
        // Debugger.Log("hi");
        
        if (!movable) return;
        
        var cursorPositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = home + cursorPositionWorld - cursorhome;

        if (triggerHandArea)
            Game.Ctx.CardOperator.pileHand.VirtualPositionChecker(transform.position);
    }

    private void OnMouseUp()
    {
        if (triggerPlayArea)
        {
            Game.Ctx.CardOperator.AddCardToQueue(GetComponent<Card>());
        }
        if (triggerHandArea)
        {
            Game.Ctx.CardOperator.RemoveCardAndAfterFromQueue(GetComponent<Card>());
        }

        triggerHandArea = false;
        triggerPlayArea = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayPile>())
        {
            if (!Game.Ctx.CardOperator.pilePlay.Contains(GetComponent<Card>()))
            {
                triggerPlayArea = true;
                triggerHandArea = false;
                Game.Ctx.CardOperator.pileHand.VirtualRemove();
            }
        }
        else if (other.gameObject.GetComponent<HandPile>())
            if (!Game.Ctx.CardOperator.pileHand.Contains(GetComponent<Card>()))
                triggerHandArea = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayPile>())
        {
            if (!Game.Ctx.CardOperator.pilePlay.Contains(GetComponent<Card>()))
                triggerPlayArea = false;
        }
        else if (other.gameObject.GetComponent<HandPile>())
            if (!Game.Ctx.CardOperator.pileHand.Contains(GetComponent<Card>()))
            {
                triggerHandArea = false;
                Game.Ctx.CardOperator.pileHand.VirtualRemove();
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
