using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Object = UnityEngine.Object;

using Library;
using Cards;
using UnityEngine;
using Data;

using _Editor;

namespace Managers
{
	public class CardManager : MonoBehaviour
	{
		// Data Field will be moved to somewhere else
		// [SerializeField]
		// private List<CardData> _cardDataArray;
		
		public List<CardData> CardList;
        public List<Card> Deck, Hand;
        public List<Card> DiscardPile;

		private GameObject _cardPrefab;
        private Card _lastCard = null;

        public int handLimit = 2;		

        

		public void Start()
		{
            _cardPrefab = (GameObject)Resources.Load("Prefabs/Card");
			Deck = new List<Card>();
			Hand = new List<Card>();
			DiscardPile = new List<Card>();
			
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

		public Card DrawCard(List<Card> pile, bool onEmptyReturnNull = true)
		{
			if (IsEmpty(pile))
				if (onEmptyReturnNull)
					return null;
				else
					throw new InvalidOperationException("The drawn pile is empty");

			Card card = pile.Draw();
            card.onDraw.Invoke();
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
                card.onDraw.Invoke();
			}

			return newPile;
		}

		public void DrawFullHand(bool onEmptyShuffle = true)
		{
            
			while (Hand.Count < handLimit)
			{
                if (IsEmpty(Deck))
                {
                    if (onEmptyShuffle)
                        ShuffleOnDeckEmpty();
                    else
                        throw new InvalidOperationException("The Deck pile is empty");
                }
                Card card = Deck.Draw();
                card.onDraw.Invoke();
            	Hand.Add(card);
			}
		}

        public void PlayCard(Card card)
        {
            bool ret = Hand.Remove(card);
            Game.Ctx.Player.chainStreak += 1;
            card.Apply(Game.Ctx.Enemy);
            card.onPlay.Invoke();

            if (!ret) {
                throw new InvalidOperationException("The popped card does not appear in the Hand pile");
            }
            else
            {


                _lastCard = card;
                DiscardPile.Add(card);
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