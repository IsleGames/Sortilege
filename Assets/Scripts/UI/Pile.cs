using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _Editor;
using Cards;
using Data;
using Library;
using Debug = System.Diagnostics.Debug;

namespace UI
{
    public enum QueueAlignType : int
    {
        Left,
        Middle,
        Right
    }

    public class Pile : MonoBehaviour
    {
        [SerializeField]
        protected List<Card> _pile;

        public bool movable;

        public float scaleFactor = 1.8f;
        public float offsetMargin = 10f;

        [SerializeField]
        protected QueueAlignType align = QueueAlignType.Left;
        // public bool zoomOnMouseOver = false;
        
        protected Vector3 QueueCenter;
        // Allowing float due to PileAlignType.Middle
        protected float StartingIndex;
        protected float TotalCardWidth;
        
        protected virtual void Start()
        {
			_pile = new List<Card>();

            QueueCenter = transform.Find("QueueCenter").transform.position;
            StartingIndex = 0;
            TotalCardWidth = Constant.cardWidth * scaleFactor + offsetMargin;
        }

        protected void SetAlign()
        {
            switch (align)
            {
                case QueueAlignType.Left:
                    StartingIndex = 0;
                    break;
                case QueueAlignType.Middle:
                    StartingIndex = (float)(_pile.Count - 1) / 2;
                    break;
                case QueueAlignType.Right:
                    StartingIndex = _pile.Count - 1;
                    break;
            }
        }

        public void SetAllAvailabilities(bool availability)
        {
            foreach (Card card in _pile)
            {
                card.SetAvailability(availability);
            }
        }
        
        public void CheckAllAvailabilities()
        {
            foreach (Card card in _pile)
            {
                card.CheckChainedAvailability();
            }
        }

        public void SetSortOrders()
        {
            if (_pile.Count == 0) return;
            
            int st, ed, inc;
            if (align != QueueAlignType.Right)
            {
                st = 0;
                ed = _pile.Count;
                inc = 1;
            }
            else
            {
                ed = -1;
                st = _pile.Count - 1;
                inc = -1;
            }

            // Debugger.Log(gameObject.name + $" setting orders with {st} {ed} and {inc}...");
            for (int i = st; i != ed; i += inc)
            {
                // Debugger.Log("i: " + i);
                _pile[i].GetComponent<CardRender>().SetOrder();
            }
        }

        protected void AdjustPosition(int index, bool stopFlag = false)
        {
            Transform thisTrans = _pile[index].transform;
            Vector3 newScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            Vector3 newPos = new Vector3(
                QueueCenter.x + TotalCardWidth * (index - StartingIndex),
                QueueCenter.y,
                QueueCenter.z);

            Game.Ctx.AnimationOperator.PushAction(
                Utilities.MoveAndScaleTo(thisTrans.gameObject, newPos, newScale, 0.15f),
                stopFlag
            );
            
            // thisTrans.localScale = newScale;
            // thisTrans.position = newPos;
        }

        public void AdjustAllPositions(bool stopFlag = false, bool setOrder = true)
        {
            SetAlign();
            if (setOrder) SetSortOrders();
            for (var i = 0; i < _pile.Count; i++)
                if (!stopFlag)
                    AdjustPosition(i);
                else
                    if (i < _pile.Count - 1)
                        AdjustPosition(i);
                    else
                        AdjustPosition(i, true);
        }

        public int Count()
        {
            return _pile.Count;
        }
        public bool Contains(Card card)
        {
            return _pile.Contains(card);
        }
        public Card Get(int index)
        {
            return _pile[index]; 
        }
        public int IndexOf(Card card)
        {
            return _pile.IndexOf(card);
        }
        
        public void Add(Card card, bool adjust = true)
        {
            _pile.Add(card);
            if (adjust) AdjustAllPositions();
        }

        public void AddRange(List<Card> cardList, bool shuffleAfter = false, bool stopFlag = false)
        {
            _pile.AddRange(cardList);
            if (shuffleAfter)
                Shuffle(stopFlag);
            else
                AdjustAllPositions(stopFlag);
        }
        public void Clear()
        {
            _pile.Clear();
        }
        public List<Card> GetStrategyTypeCards(StrategyType strategyType)
        {
            List<Card> ret = new List<Card>();
            foreach (Card card in _pile)
                if (card.GetComponent<MetaData>().strategy == strategyType)
                {
                    ret.Add(card);
                }

            return ret;
        }
        public void Insert(int index, Card card)
        {
            _pile.Insert(index, card);
            AdjustAllPositions();
        }
        public bool Remove(Card card)
        {
            bool ret = _pile.Remove(card);
            AdjustAllPositions();
            return ret;
        }
        
        public void RemoveAt(int index)
        {
            _pile.RemoveAt(index);
            AdjustAllPositions();
        }
        
        public Card Draw()
        {
            Card drawnCard = _pile.Draw();
            AdjustAllPositions();
            return drawnCard;
        }

        public Card DrawNoShuffle()
        {
            Card drawnCard = _pile.DrawNoShuffle();
            AdjustAllPositions();
            return drawnCard;
        }
        
        public List<Card> DrawAll()
        {
            List<Card> ret = _pile;
            _pile = new List<Card>();
            AdjustAllPositions();
            
            return ret;
        }

        public void Shuffle(bool stopFlag = false)
        {
            _pile.Shuffle();
            AdjustAllPositions(stopFlag);
        }

    }
}