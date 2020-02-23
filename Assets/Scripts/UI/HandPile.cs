using System;
using System.Collections.Generic;
using _Editor;
using Cards;
using UnityEditor;
using UnityEngine;

using Library;

namespace UI
{
    public class HandPile : Pile
    {
        public bool isVirtualOn = false;
        
        private Card _virtualCard;
        private int _virtualCardIndex;

        private List<Card> _virtualPile;
        private float _startingVirtualIndex;
        
        private new void Start()
        {
            base.Start();
            
            GameObject virtualObject = Instantiate(Game.Ctx.CardOperator.cardPrefab, Game.Ctx.transform);
            virtualObject.name = "VirtualCard";
            virtualObject.layer = 2;
            
            _virtualCard = virtualObject.GetComponent<Card>();
            _virtualCard.GetComponent<CardRender>().Hide();
            _virtualCardIndex = -1;
        }

        private void Update()
        {
            if (isVirtualOn)
            {
                VirtualMove(Game.Ctx.VfxOperator.draggedCard.transform.position);
            }
        }
        
        private void SetVirtualAlign()
        {
            switch (align)
            {
                case QueueAlignType.Left:
                    _startingVirtualIndex = 0;
                    break;
                case QueueAlignType.Middle:
                    _startingVirtualIndex = (float)(_virtualPile.Count - 1) / 2;
                    break;
                case QueueAlignType.Right:
                    _startingVirtualIndex = _virtualPile.Count - 1;
                    break;
            }
        }
        private void AdjustVirtualPosition(int index, bool setAlign = false)
        {
            if (setAlign) SetVirtualAlign();

            Transform thisTrans = _virtualPile[index].transform;
            thisTrans.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            Vector3 newPos = new Vector3(
                QueueCenter.x + TotalCardWidth * (index - _startingVirtualIndex),
                QueueCenter.y,
                QueueCenter.z);
            thisTrans.position = newPos;

            // if (thisTrans.gameObject.name == "Shockwave")
            // {
            //     Debugger.Log("AdjustPosition(skwv) " + Time.time + " " + thisTrans.position + " " + newPos);
            // }
        }
        private void AdjustAllVirtualPositions()
        {
            SetVirtualAlign();
            for (var i = 0; i < _virtualPile.Count; i++)
            {
                AdjustVirtualPosition(i);
            }
        }

        public void VirtualInitialize()
        {
            isVirtualOn = true;
            _virtualPile = new List<Card>(_pile);

            if (Contains(Game.Ctx.VfxOperator.draggedCard))
            {
                _virtualCardIndex = _virtualPile.IndexOf(Game.Ctx.VfxOperator.draggedCard);
                _virtualPile[_virtualCardIndex] = _virtualCard;
            }
            else
            {
                _virtualPile.Add(_virtualCard); 
                _virtualCardIndex = _virtualPile.Count - 1;
            }
            
            
            VirtualMove(Game.Ctx.VfxOperator.draggedCard.transform.position, true);
        }

        public void VirtualDestroy(bool adjust = false)
        {
            if (!isVirtualOn) return;

            if (Contains(Game.Ctx.VfxOperator.draggedCard))
            {
                _pile.Remove(Game.Ctx.VfxOperator.draggedCard);
                _pile.Insert(_virtualCardIndex, Game.Ctx.VfxOperator.draggedCard);
            }
            
            _virtualPile = null;
            _virtualCardIndex = -1;
            isVirtualOn = false;
            
            if (adjust) AdjustAllPositions();
            
            // Debugger.Log("I am called");
        }
        
        public void VirtualMove(Vector3 mousePosition, bool forceReset = false)
        {
            SetVirtualAlign();
            
            float curIndexf = (int)Mathf.Round((mousePosition.x - QueueCenter.x) / TotalCardWidth + _startingVirtualIndex);
            int index = (int) Mathf.Clamp(curIndexf, 0, _virtualPile.Count - 1);

            if (_virtualCardIndex != index || forceReset)
            {
                Card temp = _virtualPile[_virtualCardIndex];
                _virtualPile[_virtualCardIndex] = _virtualPile[index];
                _virtualPile[index] = temp;
                
                _virtualCardIndex = index;
                AdjustAllVirtualPositions();
            }
        }

        public new void Add(Card card)
        {
            VirtualDestroy();
            base.Add(card);
        }
        public new void AddRange(List<Card> cardList, bool shuffleAfter = false)
        {
            VirtualDestroy();
            base.AddRange(cardList, shuffleAfter);
        }
        public new void Clear()
        {
            VirtualDestroy();
            base.Clear();
        }
        public new void Insert(int index, Card card)
        {
            VirtualDestroy();
            base.Insert(index, card);
        }
        public new bool Remove(Card card)
        {
            VirtualDestroy();
            return base.Remove(card);
        }
        public new void RemoveAt(int index)
        {
            VirtualDestroy();
            base.RemoveAt(index);
        }
        public new Card Draw()
        {
            VirtualDestroy();
            return base.Draw();
        }
        
        public new List<Card> DrawAll()
        {
            VirtualDestroy();
            return base.DrawAll();
        }
        public new void Shuffle()
        {
            VirtualDestroy();
            base.Shuffle();
        }
    }
}