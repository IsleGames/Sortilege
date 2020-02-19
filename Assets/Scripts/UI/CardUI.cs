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

    public bool allowChained = true; // TODO: default to false, check in update() based on game state
    public bool enterPlayArea;
    public float moveSpeed = 0.1f;

    void Start()
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
    
    public void OnMouseDown()
    {
        Debugger.Log("hi");
        home = transform.position;
        cursorhome = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnMouseDrag()
    {
        if (allowChained)
        {
            var cursorPositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.position = home + cursorPositionWorld - cursorhome;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayPile>()) enterPlayArea = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayPile>())
            enterPlayArea = false;
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
        if (enterPlayArea)
        {
            //play this card
            Debug.Log("Playing");

            Game.Ctx.CardOperator.AddCardToQueue(card);
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
