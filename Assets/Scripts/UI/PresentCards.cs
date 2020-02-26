using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using Library;
using Cards;
using UI;
using Managers;

public class PresentCards : MonoBehaviour
{

    public int NumPresenting = 3;
    public List<CardData> cardData;
    public List<Card> cards;
    private AnimationManager animation_manager;
    // Start is called before the first frame update
    void Start()
    {
        GameObject cardPrefab = Resources.Load("Prefabs/Card") as GameObject;
        animation_manager = GetComponent<AnimationManager>();

        SelectCards();
        var pile = GetComponent<Pile>();

        foreach (var data in cardData)
        {
            var cardObj = Instantiate(cardPrefab);
            var card = cardObj.GetComponent<Card>();
            card.Initialize(data);
            cardObj.gameObject.AddComponent<Select>();
            Destroy(cardObj.gameObject.GetComponent<CardEvent>());
            pile.Add(card);

        }
        Game.Ctx.AnimationOperator.PushAnimation(StartAnimation());
        
    }

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        Game.Ctx.VfxOperator.ShowTurnText("New Card!");
        yield return null;

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void SelectCards()
    {
        List<CardData> cards = new List<CardData>();
        CardData[] all_cards = Resources.LoadAll<CardData>("Card");
        cards.AddRange(all_cards);
        cards.Shuffle();
        if (cards.Count > NumPresenting)
        {
            cards.RemoveRange(NumPresenting, cards.Count - NumPresenting);
        }
        cardData = cards;
        
    }

}
