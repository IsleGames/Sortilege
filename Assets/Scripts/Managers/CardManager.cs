using System;
using System.Collections.Generic;
using System.IO;
using _Editor;
using Cards;
using Data;
using Library;
using UnityEngine;

namespace Managers
{
	public class CardManager : MonoBehaviour
	{
		// Data Field will be moved to somewhere else
		// [SerializeField]
		// private List<CardData> _cardDataArray;
		
		public List<CardData> CardList;
		public List<Card> Deck, Hand, DiscardPile;

		public int handLimit;
        
		private GameObject _cardPrefab;
		private Card _lastPlayed = null;

        private void Awake()
        {
            _cardPrefab = (GameObject)Resources.Load("Prefabs/Card");
            
			Deck = new List<Card>();
			Hand = new List<Card>();
			DiscardPile = new List<Card>();
			
			handLimit = 2;
        }

        private void Start()
		{
			
			// Grab the list from the Inspector for now
		}
		
		public void Initialize()
		{
			// All Initialize functions should have Game.Ctx.Continue() for seq control

			foreach (CardData cardData in CardList)
			{
				GameObject newCardObj = Instantiate(_cardPrefab);
				Card newCard = newCardObj.GetComponent<Card>();

				newCard.Initialize(cardData);
				Deck.Add(newCard);
			}
			
			Debugger.Log("Deck initialize complete!");
			
			Game.Ctx.Continue();
		}

		public Card DrawCard(List<Card> pile, bool onEmptyReturnNull = true)
		{
			if (IsEmpty(pile))
				if (onEmptyReturnNull)
					return null;
				else
					throw new InvalidOperationException("The drawn pile is empty");

			Card card = pile.Draw();
			return card;

		}

		public List<Card> DrawCards(
			int number,
			List<Card> drawPile,
			bool onEmptyReturnRemainingCount = true
		)
		{
			List<Card> newPile = new List<Card>();

			for (int i = 0; i < number; i++)
			{
				if (IsEmpty(drawPile))
					if (onEmptyReturnRemainingCount)
						return newPile;
					else
						throw new InvalidOperationException("The drawn pile is empty");

				Card card = drawPile.Draw();
			}

			return newPile;
		}

		public void DrawFullHand(bool onEmptyShuffle = true)
		{
			Debugger.Log("Hand now has " + Hand.Count + ". Drawing up to " + handLimit + "...");
			
			while (Hand.Count < handLimit)
			{
				if (IsEmpty(Deck))
					if (onEmptyShuffle)
						ShuffleOnDeckEmpty();
					else
						throw new InvalidOperationException("The Deck pile is empty");

				Card card = Deck.Draw();
				
                Debugger.Log(card);
                
				Hand.Add(card);
			}
		}

        public void PlayCard(Card card)
        {
            bool ret = Hand.Remove(card);
            if (!ret)
                throw new InvalidDataException("The popped card does not appear in the Hand pile");
            
            card.Apply(Game.Ctx.Enemy);
            
            _lastPlayed = card;
            DiscardPile.Add(card);
        }
		
		public void PopCard(Card card)
		{
			bool ret = Hand.Remove(card);

			if (!ret)
				throw new InvalidDataException("The popped card does not appear in the Hand pile");
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