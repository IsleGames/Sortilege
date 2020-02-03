using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// public enum CardStatus : int
// {
//     Unknown,
//     Stored,
//     Decked,
//     Held,
//     Discarded,
// }

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

	public void Add(List<Card> newCards)
	{
		CardList.AddRange(newCards);
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

		Card card = Utilities.Draw(pile);
		return card;

	}

	public int DrawCards(
		int number,
		List<Card> drawPile,
		List<Card> targetPile,
		bool onEmptyReturnRemainingCount = true
	) {

		for (int i = 0; i < number; i++)
		{
			if (IsEmpty(drawPile))
				if (onEmptyReturnRemainingCount)
					return number - i;
				else
					throw new InvalidOperationException("The drawn pile is empty");

			Card card = Utilities.Draw(drawPile);
			targetPile.Add(card);
		}

		return 0;

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

			Card card = Utilities.Draw(Deck);
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