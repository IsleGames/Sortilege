using System;
using System.Collections;
using _Editor;
using Cards;
using TMPro;
using UI;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

// This should maybe just be Collider? 
// We'll see how many dimensions we use
// Ans: If the game is in 2D, there is no need to apply 3D collider
[RequireComponent(typeof(Collider2D))]
public class CardUI : MonoBehaviour {

    public bool movable, enabled = true; // TODO: default to false, check in update() based on game state
    
    [SerializeField]
    private bool triggerPlayArea, triggerHandArea, isDragged;
    public Pile thisPile;
    
    public float moveSpeed = 0.1f;

    void Start()
    {
        GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
        
        isDragged = false;
    }
    
    Vector3 home;
    private Vector3 cursorhome;

    void OnMouseDown()
    {
        Debugger.Log(GetComponent<MetaData>().title + " MouseDown at " + Time.time + ", metadata name is " + GetComponent<MetaData>().title);

        if (!enabled)
        {
            Debugger.Log(gameObject.name + " hided; HandPile virtual Card opening is " + Game.Ctx.CardOperator.pileHand.isVirtualOn + "; exit");
            return;
        }
        
        thisPile = Game.Ctx.CardOperator.GetCardPile(GetComponent<Card>());

        if (thisPile.gameObject.name != "HandPile" && thisPile.gameObject.name != "PlayPile")
        {
            movable = false;
            return;
        }

        Game.Ctx.VfxOperator.draggedCard = GetComponent<Card>();
        
        home = transform.position;
        cursorhome = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragged = true;
        
        if (thisPile == Game.Ctx.CardOperator.pileHand)
        {
            Game.Ctx.CardOperator.pileHand.VirtualInitialize();
        }
        
    }

    void OnMouseDrag()
    {
        if (!movable) return;
        
        var cursorPositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = home + cursorPositionWorld - cursorhome;

        if (thisPile.gameObject.name == "PlayPile")
        {
            if (triggerHandArea && !Game.Ctx.CardOperator.pileHand.isVirtualOn)
                Game.Ctx.CardOperator.pileHand.VirtualInitialize();
            else if (!triggerHandArea && Game.Ctx.CardOperator.pileHand.isVirtualOn)
                Game.Ctx.CardOperator.pileHand.VirtualDestroy(true);
        }
    }

    void OnMouseUp()
    {
        // Debugger.Log(triggerPlayArea + " " + triggerHandArea);
        Card card = GetComponent<Card>();
        
        // thisPile == Game.Ctx.CardOperator.pileHand could also work
        
        if (thisPile.gameObject.name == "HandPile" && triggerPlayArea)
        {
            Game.Ctx.CardOperator.AddCardToQueue(card);
        }
        else if (thisPile.gameObject.name == "PlayPile" && triggerHandArea)
        {
            Game.Ctx.CardOperator.RemoveCardAndAfterFromQueue(card);
        }
        else
        {
            thisPile.AdjustAllPositions();
        }

        Game.Ctx.CardOperator.pileHand.VirtualDestroy(true);
        thisPile = null;
        isDragged = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayPile>())
        {
            triggerPlayArea = true;
        }
        else if (other.gameObject.GetComponent<HandPile>())
        {
            triggerHandArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayPile>())
        {
            triggerPlayArea = false;
        }
        else if (other.gameObject.GetComponent<HandPile>())
        {
            triggerHandArea = false;
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
        enabled = false;
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
        enabled = true;
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
