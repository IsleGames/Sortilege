using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Data;
using Library;
using Cards;
using UI;

public class PresentCards : MonoBehaviour
{
    public int NumPresenting = 3;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject cardPrefab = Resources.Load("Prefabs/Card") as GameObject;

        var pile = transform.Find("AddPile").GetComponent<Pile>();
        
        var cardData = SelectCards();
        
        foreach (var data in cardData)
        {
            var card = Instantiate(cardPrefab, transform);
            
            card.GetComponent<Card>().Initialize(data);
            card.gameObject.AddComponent<Select>();
            Destroy(card.gameObject.GetComponent<CardEvent>());
            
            pile.Add(card.GetComponent<Card>(), false);
        }
        pile.AdjustAllPositions();
    }

    private List<CardData> SelectCards()
    {
        List<CardData> cards = new List<CardData>();
        CardData[] all_cards = Resources.LoadAll<CardData>("Card");
        
        cards.AddRange(all_cards);
        cards.Shuffle();
        if (cards.Count > NumPresenting)
        {
            cards.RemoveRange(NumPresenting, cards.Count - NumPresenting);
        }
        return cards;
    }

}
