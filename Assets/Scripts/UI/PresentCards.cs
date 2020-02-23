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
    void Start()
    {
        GameObject cardPrefab = Resources.Load("Prefabs/Card") as GameObject;
        var pile = GetComponent<Pile>();
        var cardData = SelectCards();
        foreach (var data in cardData)
        {
            var card = Instantiate(cardPrefab);
            card.GetComponent<Card>().Initialize(data);
            card.gameObject.AddComponent<Select>();
            card.gameObject.GetComponent<CardEvent>().enabled = false;
            pile.Add(card.GetComponent<Card>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
