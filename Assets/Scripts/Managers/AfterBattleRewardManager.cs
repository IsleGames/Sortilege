using System.Collections;
using System.Collections.Generic;
using _Editor;
using UnityEngine;

using Cards;
using Data;
using Library;
using UI;

namespace Managers
{
    public class AfterBattleRewardManager : MonoBehaviour
    {
        public int NumPresenting = 3;

        public Pile pile;
        
        public IEnumerator ContinueAfterLoadScene()
        {
            // Wait a frame so every Awake and Start method is called
            yield return new WaitForEndOfFrame();
            
            Debugger.Log(pile.Count());
            pile.AdjustAllPositions();

            yield return null;
        }
        
        // Start is called before the first frame update
        private void Start()
        {
            GameObject cardPrefab = Resources.Load("Prefabs/Card") as GameObject;

            pile = transform.Find("AddPile").GetComponent<Pile>();
            pile.Clear();
        
            var cardData = SelectCards();
        
            foreach (var data in cardData)
            {
                var card = Instantiate(cardPrefab, transform);
            
                card.GetComponent<Card>().Initialize(data);
                card.gameObject.AddComponent<Select>();
                
                Destroy(card.gameObject.GetComponent<CardEvent>());
            
                pile.Add(card.GetComponent<Card>(), false);
            }
            
            Debugger.Log(pile.Count());
            
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
}
