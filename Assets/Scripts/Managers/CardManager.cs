using System;
using System.Collections.Generic;
using Library;
using Object = UnityEngine.Object;
using Cards;

namespace Managers
{
	public class CardManager : Object
	{
		public List<Card> CardList;
		public List<Card> Deck, Hand, DiscardPile;

		public int HandLimit;

		public CardManager()
		{
			CardList = new List<Card>();

			Deck = new List<Card>();
			Hand = new List<Card>();
			DiscardPile = new List<Card>();
		}

		public void AddCard(Card card)
		{
			CardList.Add(card);
		}

		public void AddCards(List<Card> newCards)
		{
			CardList.AddRange(newCards);
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
			while (Hand.Count < HandLimit)
			{
				if (IsEmpty(Deck))
					if (onEmptyShuffle)
						ShuffleOnDeckEmpty();
					else
						throw new InvalidOperationException("The Deck pile is empty");

				Card card = Deck.Draw();
				Hand.Add(card);
			}
		}

		public void PopCard(Card card)
		{

			bool ret = Hand.Remove(card);

			if (!ret)
				throw new InvalidOperationException("The popped card does not appear in the Hand pile");
			else
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