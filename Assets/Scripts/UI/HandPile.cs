using System;
using _Editor;
using Cards;
using UnityEditor;
using UnityEngine;

using Library;

namespace UI
{
    public class HandPile : Pile
    {
        private Card _virtualCard;
        [SerializeField]
        private int _virtualCardIndex;
        
        private new void Start()
        {
            base.Start();
            
            GameObject virtualObject = Instantiate(Game.Ctx.CardOperator.cardPrefab);
            virtualObject.name = "VirtualCard";
            
            _virtualCard = virtualObject.GetComponent<Card>();
            _virtualCard.GetComponent<CardUI>().Hide();
            _virtualCardIndex = -1;
        }

        public int RealCount()
        {
            if (_virtualCardIndex == -1)
                return _pile.Count;
            else
                return _pile.Count - 1;
        }

        public void Insert(int index, Card card)
        {
            _pile.Insert(index, card);
            AdjustAllPositions();
        }

        public void VirtualPositionChecker(Vector3 mousePosition)
        {
            float curIndexf = (int)Mathf.Round((mousePosition.x - QueueCenter.x) / TotalCardWidth + StartingIndex);
            int curIndex = (int) Mathf.Clamp(curIndexf, 0, RealCount());

            if (_virtualCardIndex != curIndex)
            {
                if (_virtualCardIndex != -1)
                    VirtualMove(curIndex);
                else
                    VirtualInsertAt(curIndex);
            }
        }
        
        public void AddOnVirtual(Card card)
        {
            _pile[_virtualCardIndex] = card;
            AdjustAllPositions();
        }

        public void VirtualInsertAt(int index, bool adjust = true)
        {
            _pile.Insert(index, _virtualCard);
            if (adjust) AdjustAllPositions();
            _virtualCardIndex = index;
        }
        
        public void VirtualRemove(bool adjust = true)
        {
            if (_virtualCardIndex == -1) return;
            
            _pile.Remove(_virtualCard);
            if (adjust) AdjustAllPositions();
            _virtualCardIndex = -1;
        }
        
        public void VirtualMove(int index, bool adjust = true)
        {
            Card temp = _pile[_virtualCardIndex];
            _pile[_virtualCardIndex] = _pile[index];
            _pile[index] = temp;
            _virtualCardIndex = index;
            
            if (adjust) AdjustAllPositions();
        }
        public void ReplaceWithVirtualCard(Card card)
        {
            int i = _pile.IndexOf(card);
            _pile[i] = _virtualCard;
            _virtualCardIndex = i;
            AdjustPosition(i);
        }
        
        public void ReplaceWithRealCard(Card card)
        {
            // Debugger.Log("hi");
            
            if (_virtualCardIndex == -1) 
                throw new InvalidOperationException("No virtual card exists");
            
            _pile[_virtualCardIndex] = card;
            AdjustPosition(_virtualCardIndex);
            _virtualCardIndex = -1;
        }
    }
}