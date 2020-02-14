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
using Units;

namespace Managers
{
	public class CardManager : MonoBehaviour
	{
		// Data Field will be moved to somewhere else
		// [SerializeField]
		// private List<CardData> _cardDataArray;
		
		public List<CardData> CardList;
        public List<Card> Deck, Hand, DiscardPile, PlayQueue;

		private GameObject _cardPrefab;
        private Card _lastCard = null;

		public int cardsDrawnPerTurn = -1;
		
		public void Start()
		{
            _cardPrefab = (GameObject)Resources.Load("Prefabs/Card");
			Deck = new List<Card>();
			Hand = new List<Card>();
			DiscardPile = new List<Card>();
			PlayQueue = new List<Card>();
			
			// Grab the list from the Inspector for now
			// CardList = new List<Card>();

			
		}
		
		public void Initialize()
		{
            if (Deck.Count > 0) return;

			foreach (CardData cardData in CardList)
			{
				GameObject newCardObj = Instantiate(_cardPrefab);
				Card newCard = newCardObj.GetComponent<Card>();
				
				newCard.Initialize(cardData);
                newCard.GetComponent<Render>().Initialize();

				Deck.Add(newCard);
			}
		}

		public void StartTurn()
		{

			if (PlayQueue.Count > 0)
				throw new InvalidConstraintException("PlayQueue is not empty at the start of the turn");
			
			if (cardsDrawnPerTurn == -1)
				throw new SerializationException("cardsDrawnEachTurn not Initialized");
			
			DrawCards(cardsDrawnPerTurn);

			foreach (Card card in Game.Ctx.CardOperator.Hand)
			{
				card.LogInfo();
			}
		}

		public void AddCardToQueue(Card card)
		{
			if (!Hand.Remove(card))
				throw new InvalidOperationException("Card not in Hand");
			
			PlayQueue.Add(card);
			Hand.Remove(card);
		}

		public void RemoveCardAndAfterFromQueue(Card card)
		{
			if (card.GetComponent<Ability>().disableRetract)
			{

				// This is a fail-safe error; Show it in the UI directly
				throw new InvalidOperationException("Card is not retractable");
			}

			int cardID = PlayQueue.IndexOf(card);
			
			if (cardID == -1)
				throw new InvalidOperationException("Card not in Hand");
			
			for (int i = PlayQueue.Count - 1; i >= cardID; i--)
			{
				Card thisCard = PlayQueue[i];
				PlayQueue.RemoveAt(i);
				Hand.Add(thisCard);
			}
		}


		public void DrawCards(int number, bool onEmptyShuffle = true)
		{
			for (int i = 0; i < number; i++)
			{
				if (IsEmpty(Deck))
					if (onEmptyShuffle)
						ShuffleOnDeckEmpty();
					else
						throw new InvalidOperationException("The drawn pile is empty");

				Card card = Deck.Draw();
				Hand.Add(card);
			}
		}

		public void Apply(Unit target)
        {
	        if (PlayQueue.Count > 0)
	        {
		        // Only for one enemy
		        PlayQueue[PlayQueue.Count - 1].Apply(target, PlayQueue.Count);
	        
				for (int i = PlayQueue.Count - 1; i >= 0; i--)
				{
					Card thisCard = PlayQueue[i];
					PlayQueue.RemoveAt(i);
					DiscardPile.Add(thisCard);
				}
	        }
        }
		
		public void PopCard(Card card)
		{
			bool ret = Hand.Remove(card);

			if (!ret)
				throw new InvalidOperationException("The popped card does not appear in the Hand pile");
			
			
			DiscardPile.Add(card);
		}

		public bool IsEmpty(List<Card> pile)
		{
			return pile.Count == 0;
		}

		public void ShuffleOnDeckEmpty() {
			if (!IsEmpty(Deck))
				throw new InvalidOperationException("The deck is not empty");

			Deck.AddRange(Hand);
			Deck.AddRange(DiscardPile);

			Hand.Clear();
			DiscardPile.Clear();

			Deck.Shuffle();
		}

	}
}