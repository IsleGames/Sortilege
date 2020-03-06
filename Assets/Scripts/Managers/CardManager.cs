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
using UnityEngine.Events;

namespace Managers
{
	public class CardManager : MonoBehaviour
	{
		// Data Field will be moved to somewhere else
		// [SerializeField]
		// private List<CardData> _cardDataArray;
		
        public UnityEvent onTopChange = new UnityEvent();

        [SerializeField] public bool _disableMetaTypeFiltering = false;

        public List<CardData> CardList;
        public Pile pileDeck, pileDiscard;
        public HandPile pileHand;
        public PlayPile pilePlay;

        public GameObject cardPrefab;
        public GameObject buffPrefab;

		public int cardsDrawnFirstTurn = 5;
		public int cardsDrawnPerTurn = -1;
		public int maxCardCount = 5;

		public bool randomDraw = true;

		private void Awake()
		{
			cardPrefab = (GameObject)Resources.Load("Prefabs/Card");
            buffPrefab = (GameObject)Resources.Load("Prefabs/Buff");
		}

		public void Start()
		{
            pileDeck = transform.Find("DeckPile").GetComponent<Pile>();
			pileHand = transform.Find("HandPile").GetComponent<HandPile>();
			pileDiscard = transform.Find("DiscardPile").GetComponent<Pile>();
			pilePlay = transform.Find("PlayPile").GetComponent<PlayPile>();

			/*if (CardList.Count > maxCardCount)
			{
				CardList.Shuffle();
				CardList.RemoveRange(maxCardCount, CardList.Count - maxCardCount);
			} */

			foreach (CardData cardData in CardList)
			{
				GameObject newCardObj = Instantiate(cardPrefab, transform);
				Card newCard = newCardObj.GetComponent<Card>();
				
				newCard.Initialize(cardData);

                pileDeck.Add(newCard, false);
            }
			
			onTopChange.AddListener(EvaluateAvailability);
        }

		public void EvaluateAvailability()
		{
			pilePlay.SetAllAvailabilities(true);
			pileHand.CheckAllAvailabilities();
			pileDeck.SetAllAvailabilities(true);
			pileDiscard.SetAllAvailabilities(true);
		}

        public Card MakeCard(CardData cardData)
        {
            GameObject newCardObj = Instantiate(cardPrefab);
            Card newCard = newCardObj.GetComponent<Card>();

            newCard.Initialize(cardData);
            return newCard;
        }

        public void StartTurn()
		{
			if (pilePlay.Count() > 0)
				throw new InvalidConstraintException("PlayQueue is not empty at the start of the turn");
			
			if (cardsDrawnPerTurn == -1)
				throw new SerializationException("cardsDrawnEachTurn not Initialized");

			Game.Ctx.VfxOperator.SetAllSortOrders();
			DrawCards(Game.Ctx.turnCount == 1 ? cardsDrawnFirstTurn : cardsDrawnPerTurn, true, randomDraw);
		}

        public void EndTurn()
        {
	        Game.Ctx.VfxOperator.SetAllSortOrders();
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
			throw new InvalidOperationException("Card not found in any pile");
		}
        
		public void AddCardToQueue(Card card)
		{
			if (!pileHand.Contains(card))
				throw new InvalidOperationException("Card not in Hand");
			
			pilePlay.Add(card);
			pileHand.Remove(card);
			
			if (!_disableMetaTypeFiltering) onTopChange.Invoke();
		}

		public void RemoveCardAndAfterFromQueue(Card card)
		{
			// if (card.GetComponent<Ability>().disableRetract)
			// {
			// 	// This is a fail-safe error; Show it in the UI directly
			// 	throw new InvalidOperationException("Card is not retractable");
			// }

			int cardIndex = pilePlay.IndexOf(card);
			if (cardIndex == -1)
				throw new InvalidOperationException("Card not in Hand");
			
			pileHand.Add(card);
			// card.onDeleteFromPlayPile.Invoke();
			pilePlay.Remove(card);
			
			List<Card> discardList = new List<Card>();
			
			for (int i = pilePlay.Count() - 1; i >= cardIndex; i--)
			{
				discardList.Add(pilePlay.Get(i));
				pilePlay.RemoveAt(i);
			}

			pileHand.AddRange(discardList);
			
			if (!_disableMetaTypeFiltering) onTopChange.Invoke();
		}

		public void DrawCards(int number, bool onEmptyShuffle = true, bool random = true)
		{
			List<Card> drawList = new List<Card>();
			for (int i = 0; i < number; i++)
			{
				if (IsEmpty(pileDeck))
					if (onEmptyShuffle)
						if (IsEmpty(pileDiscard))
						{
							Debugger.Warning("Number of cards drawn is larger than total amount of cards");
							break;
						}
						else
							ShuffleOnDeckEmpty();
					else
						break;
				
				Card card = random? pileDeck.Draw() : pileDeck.DrawNoShuffle();
                // card.onDraw.Invoke();
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
				pileDiscard.AddRange(discardList, false, true);
				
				if (!_disableMetaTypeFiltering) onTopChange.Invoke();
            }
        }
		
        public int DiscardAllHandCards()
        {
	        List<Card> discardList = pileHand.DrawAll();
	        int ret = discardList.Count;
	        
	        pileDiscard.AddRange(discardList);
	        return ret;
        }

        public int DiscardStrategyTypeCards(StrategyType strategyType)
        {
	        List<Card> discardList = pileHand.GetStrategyTypeCards(strategyType);
	        int ret = discardList.Count;

	        foreach (Card card in discardList)
	        {
		        pileHand.Remove(card);
                pileDiscard.Add(card);
            }

	        return ret;
        }

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