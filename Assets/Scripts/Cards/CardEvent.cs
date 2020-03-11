using System;
using System.IO.IsolatedStorage;
using _Editor;
using UI;
using UnityEngine;

namespace Cards
{
    public class CardEvent : MonoBehaviour
    {
        public bool availability; // TODO: default to false, check in update() based on game state
        
        // Temporal Solution
        public bool animationLock;

        [SerializeField]
        private bool triggerPlayArea, triggerHandArea;

        [SerializeField] public bool isDragged;
        public Pile thisPile;
        
        // [SerializeField]
        private Vector3 _cursorShift;

        private void Start()
        {
            isDragged = false;
            animationLock = false;

            availability = true;
        }

        private void Update()
        {
            if (isDragged)
            {
                if (!availability) return;
            
                var cursorPositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = cursorPositionWorld + _cursorShift;
        
                if (thisPile.gameObject.name == "PlayPile")
                {
                    if (triggerHandArea && !Game.Ctx.CardOperator.pileHand.isVirtualOn)
                        Game.Ctx.CardOperator.pileHand.VirtualInitialize();
                    else if (!triggerHandArea && Game.Ctx.CardOperator.pileHand.isVirtualOn)
                        Game.Ctx.CardOperator.pileHand.VirtualDestroy(true);
                }
            }
        }

        private void OnMouseUp()
        {
            if (Game.Ctx.paused) return;
            
            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            RaycastHit2D[] hitArray = new RaycastHit2D[20];

            int hitCount = Physics2D.RaycastNonAlloc(new Vector2(p.x, p.y), Vector3.forward, hitArray);

            // Debugger.Log("hitCount: " + hitCount);
            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit2D hit = hitArray[i];
                
                if (hit.collider.GetComponent<Card>()) hit.collider.GetComponent<CardEvent>().RayCast2DTrigger();
            }
        }
        
        public void RayCast2DTrigger()
        {
            if (!Game.Ctx.BattleOperator.player.waitingForAction) return;
            if (!availability) return;
            if (!isDragged && !Game.Ctx.VfxOperator.draggedCard)
            {
                if (!GetComponent<CardRender>().visible)
                {
                    return;
                }

                if (Game.Ctx.BattleOperator.inSelectEnemyMode)
                {
                    return;
                }
                
                thisPile = Game.Ctx.CardOperator.GetCardPile(GetComponent<Card>());

                if (!thisPile.movable) return;
        
                _cursorShift = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _cursorShift = new Vector3(0f, 0f, _cursorShift.z);
                
                Game.Ctx.VfxOperator.draggedCard = GetComponent<Card>();
                GetComponent<CardRender>().SetOrder();
                
                if (thisPile == Game.Ctx.CardOperator.pileHand)
                {
                    Game.Ctx.CardOperator.pileHand.VirtualInitialize();
                }

                isDragged = true;
                GetComponent<CardRender>().OnSelectZoom();
            }
            else if (isDragged)
            {
                Card card = GetComponent<Card>();
                
                // thisPile == Game.Ctx.CardOperator.pileHand could also work
                
                if (thisPile.gameObject.name == "HandPile" && triggerPlayArea)
                {
                    Game.Ctx.CardOperator.AddCardToQueue(card);
                }
                else if (thisPile.gameObject.name == "PlayPile" && triggerHandArea)
                {
                    Game.Ctx.CardOperator.RemoveCardAndAfterFromQueue(card);
                }
                else
                {
                    if (thisPile.movable) thisPile.AdjustAllPositions();
                }
        
                Game.Ctx.CardOperator.pileHand.VirtualDestroy(true);
                Game.Ctx.VfxOperator.draggedCard = null;
                
                thisPile = null;
                isDragged = false;
                
                Game.Ctx.VfxOperator.ChangeMultiplierText(true, Game.Ctx.CardOperator.pilePlay.Count());
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayPile>())
            {
                triggerPlayArea = true;
            }
            else if (other.gameObject.GetComponent<HandPile>())
            {
                triggerHandArea = true;
            }
        }
    
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayPile>())
            {
                triggerPlayArea = false;
            }
            else if (other.gameObject.GetComponent<HandPile>())
            {
                triggerHandArea = false;
            }
        }
    }
}