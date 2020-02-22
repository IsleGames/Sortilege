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
    public enum PileAlignType : int
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
        protected PileAlignType align = PileAlignType.Left;
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
                case PileAlignType.Left:
                    StartingIndex = 0;
                    break;
                case PileAlignType.Middle:
                    StartingIndex = (float)(_pile.Count - 1) / 2;
                    break;
                case PileAlignType.Right:
                    StartingIndex = _pile.Count - 1;
                    break;
            }
        }

        protected void AdjustPosition(int index, bool setAlign = false)
        {
            if (setAlign) SetAlign();

            Transform thisTrans = _pile[index].transform;
            thisTrans.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            Vector3 newPos = new Vector3(
                QueueCenter.x + TotalCardWidth * (index - StartingIndex),
                QueueCenter.y,
                QueueCenter.z);
            
            // thisTrans.GetComponent<Render>().PController somethingsomething
            thisTrans.position = newPos;
        }

        public void AdjustAllPositions()
        {
            SetAlign();
            for (var i = 0; i < _pile.Count; i++)
            {
                AdjustPosition(i);
            }
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
        
        public void Add(Card card)
        {
            _pile.Add(card);
            AdjustAllPositions();
        }
        public void AddRange(List<Card> cardList, bool shuffleAfter = false)
        {
            _pile.AddRange(cardList);
            if (shuffleAfter)
                Shuffle();
            else
                AdjustAllPositions();
        }
        public void Clear()
        {
            _pile.Clear();
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
        
        public List<Card> DrawAll()
        {
            List<Card> ret = _pile;
            _pile = new List<Card>();
            
            return ret;
        }
        public void Shuffle()
        {
            _pile.Shuffle();
            AdjustAllPositions();
        }
    }
}