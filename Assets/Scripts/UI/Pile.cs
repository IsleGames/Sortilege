using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _Editor;
using Cards;
using Library;

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
        private List<Card> _pile;

        public float scaleFactor = 1.8f;

        public float offsetMargin = 10f;

        [SerializeField]
        private PileAlignType align = PileAlignType.Left;
        // public bool zoomOnMouseOver = false;
        
        private Vector3 queueCenter;
        // Allowing float due to PileAlignType.Middle
        private float _startingIndex;
        
        private void Start()
        {
			_pile = new List<Card>();

            queueCenter = transform.Find("QueueCenter").transform.position;
            _startingIndex = 0;
        }

        private void SetAlign()
        {
            switch (align)
            {
                case PileAlignType.Left:
                    _startingIndex = 0;
                    break;
                case PileAlignType.Middle:
                    _startingIndex = (float)(_pile.Count - 1) / 2;
                    break;
                case PileAlignType.Right:
                    _startingIndex = _pile.Count - 1;
                    break;
            }
        }

        private void AdjustPosition(int index, bool setAlign = false)
        {
            if (setAlign) SetAlign();

            Transform thisTrans = _pile[index].transform;
            thisTrans.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            
            RectTransform thisRec = thisTrans.GetComponent<RectTransform>();
            float thisWidth = thisRec.rect.width * scaleFactor;

            thisTrans.position = new Vector3(
                queueCenter.x + (thisWidth + offsetMargin) * (index - _startingIndex),
                queueCenter.y,
                queueCenter.z);
        }

        private void AdjustAllPositions()
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

        public Card Get(int index)
        {
            return _pile[index]; 
        }
        public bool Remove(Card card)
        {
            bool ret = _pile.Remove(card);
            AdjustAllPositions();
            return ret;
        }
        
        public void RemoveAt(int Index)
        {
            _pile.RemoveAt(Index);
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