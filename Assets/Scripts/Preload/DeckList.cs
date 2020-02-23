using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Data;
using Cards;
using Managers;

public class DeckList : MonoBehaviour
{

    public string path;
    public List<CardData> Deck {get; private set; }
    protected Dictionary<string, CardData> cards = new Dictionary<string, CardData>();
    // Start is called before the first frame update
    void Start()
    {
        LoadCards();
        Deck = LoadDeckList(path);
    }

    public void Add(CardData newCard)
    {
        Deck.Add(newCard);
    }

    

    void LoadCards()
    {
        var all_cards = Resources.LoadAll("Card", typeof(CardData));
        foreach (CardData card in all_cards){
            cards[card.title] = card;
        }
        
    }

    List<CardData> LoadDeckList(string path)
    {
        List<CardData> cardList = new List<CardData>();
        TextAsset listText = Resources.Load<TextAsset>(path);
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(listText.text);
        var cardnodes = xmlDocument.FirstChild.SelectNodes("descendant::card");
        foreach (XmlNode node in cardnodes)
        {
            string name = node.Attributes["name"].Value;
            int num = int.Parse(node.Attributes["number"].Value);
            for (int i = 0; i < num; i++)
            {
                cardList.Add(cards[name]);
            }
        }
        return cardList;
    }


    List<Card> GetCards(CardManager manager)
    {
        var newCards = new List<Card>();
        foreach (var data in Deck){
            newCards.Add(
                manager.MakeCard(data)
                );
        }
        return newCards;
    }

    

}
