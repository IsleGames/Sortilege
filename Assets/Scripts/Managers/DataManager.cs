using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using _Editor;
using Cards;
using Data;
using UnityEngine;

namespace Managers
{
    public class DataManager : MonoBehaviour
    {
        protected Dictionary<string, CardData> cardDataScript = new Dictionary<string, CardData>();

        private void Start()
        {
            LoadCards();
        }

        public void LoadCards()
        {
            var allCards = Resources.LoadAll("Card", typeof(CardData));
            foreach (CardData card in allCards){
                cardDataScript[card.title] = card;
                Debugger.Log("Add " + card.title);
            }
        }

        public CardData GetCard(string title)
        {
            Debugger.Log("checking " + title);
            
            return cardDataScript[title];
        }
    }
}