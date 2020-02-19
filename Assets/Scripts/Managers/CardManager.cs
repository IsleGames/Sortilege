using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using Object = UnityEngine.Object;

using Library;
using Cards;
using UnityEngine;
using Data;


using _Editor;
using UI;
using Units;

namespace Managers
{
	public class CardManager : MonoBehaviour
	{
		// Data Field will be moved to somewhere else
		// [SerializeField]
		// private List<CardData> _cardDataArray;
		
		public List<CardData> CardList;
        public Pile pileDeck, pileDiscard;
        public HandPile pileHand;
        public PlayPile pilePlay;

        public GameObject cardPrefab;

		public int cardsDrawnPerTurn = -1;
		public int maxCardCount = 5;
		
		public void Start()
		{
            cardPrefab = (GameObject)Resources.Load("Prefabs/Card");
            
			pileDeck = GameObject.Find("DeckPile").GetComponent<Pile>();
			pileHand = GameObject.Find("HandPile").GetComponent<HandPile>();
			pileDiscard = GameObject.Find("DiscardPile").GetComponent<Pile>();
			pilePlay = GameObject.Find("PlayPile").GetComponent<PlayPile>();

			if (CardList.Count > maxCardCount)
			{
				CardList.Shuffle();
				CardList.RemoveRange(maxCardCount, CardList.Count - maxCardCount);
			} 

			foreach (CardData cardData in CardList)
			{
				GameObject newCardObj = Instantiate(cardPrefab);
				Card newCard = newCardObj.GetComponent<Card>();
				
				newCard.Initialize(cardData);
                //newCard.GetComponent<Render>().Initialize();

				pileDeck.Add(newCard);
			}
		}

		public void StartTurn()
		{
			if (pilePlay.Count() > 0)
				throw new InvalidConstraintException("PlayQueue is not empty at the start of the turn");
			
			if (cardsDrawnPerTurn == -1)
				throw new SerializationException("cardsDrawnEachTurn not Initialized");
			
			DrawCards(cardsDrawnPerTurn);
			
			// foreach (Card card in Game.Ctx.CardOperator.pileHand)
			// {
			// 	card.LogInfo();
			// }
		}
		public Pile GetCardPile(Card card)
		{
			if (pileHand.Contains(card))
				return pileHand;
			if (pilePlay.Contains(card))
				return pilePlay;
			if (pileDeck.Contains(card))
				return pileDeck;
			if (pileDiscard.Contains(card))
				return pileDiscard;
			throw new ArgumentOutOfRangeException("Card not found in any pile");
		}
		
		public void AddCardToQueue(Card card)
		{
			if (!pileHand.Contains(card) && card.GetComponent<CardUI>().thisPile != pileHand)
				throw new InvalidOperationException("Card not in Hand");
			
			pilePlay.Add(card);
			// card.onAddToPlayPile.Invoke();
			pileHand.VirtualRemove();
		}

		public void RemoveCardAndAfterFromQueue(Card card)
		{
			if (card.GetComponent<Ability>().disableRetract)
			{
				// This is a fail-safe error; Show it in the UI directly
				throw new InvalidOperationException("Card is not retractable");
			}

			int cardID = pilePlay.IndexOf(card);
			if (cardID == -1)
				throw new InvalidOperationException("Card not in Hand");
			
			pileHand.AddOnVirtual(card);
			// card.onDeleteFromPlayPile.Invoke();
			pilePlay.Remove(card);
			
			List<Card> discardList = new List<Card>();
			
			for (int i = pilePlay.Count() - 1; i >= cardID; i--)
			{
				discardList.Add(pilePlay.Get(i));
				pilePlay.RemoveAt(i);
			}

			pileHand.AddRange(discardList);
		}

		public void DrawCards(int number, bool onEmptyShuffle = true)
		{
			List<Card> drawList = new List<Card>();
			for (int i = 0; i < number; i++)
			{
				if (IsEmpty(pileDeck))
					if (onEmptyShuffle)
						if (IsEmpty(pileDiscard))
							break;
						else
							ShuffleOnDeckEmpty();
					else
						break;

				Card card = pileDeck.Draw();
                card.onDraw.Invoke();
                drawList.Add(card);
			}
			pileHand.AddRange(drawList);
		}

		public void Apply(Unit target)
        {
	        if (pilePlay.Count() > 0)
	        {
		        Card poweredCard = pilePlay.Get(pilePlay.Count() - 1);
		        
		        poweredCard.Apply(target, pilePlay.Count());
		        List<Card> discardList = pilePlay.DrawAll();
				pileDiscard.AddRange(discardList);
	        }
        }
		
		public void PopCard(Card card)
		{
			bool ret = pileHand.Remove(card);

			if (!ret)
				throw new InvalidOperationException("The popped card does not appear in the Hand pile");

            card.onDiscard.Invoke();
			pileDiscard.Add(card);
		}
/*
        public void PopHand()
        {
            foreach (Card card in pileHand)
            {
	            card.onDiscard.Invoke();
                pileDiscard.Add(card);
            }
            pileHand.RemoveAll((Card c)=>true);

        }
*/
		public bool IsEmpty(Pile pile)
		{
			return pile.Count() == 0;
		}

		public void ShuffleOnDeckEmpty() {
			if (!IsEmpty(pileDeck))
				throw new InvalidOperationException("The deck is not empty");

			List<Card> retrieveList = pileDiscard.DrawAll();
			pileDeck.AddRange(retrieveList, true);
		}

	}
}