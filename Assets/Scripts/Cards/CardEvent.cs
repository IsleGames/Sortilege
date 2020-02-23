using UI;
using UnityEngine;

namespace Cards
{
    public class CardEvent : MonoBehaviour
    {
        public bool movable; // TODO: default to false, check in update() based on game state
    
        [SerializeField]
        private bool triggerPlayArea, triggerHandArea;
        public Pile thisPile;
        
        private Vector3 _cursorShift;
        
        private void OnMouseDown()
        {
            // Debugger.Log(GetComponent<MetaData>().title + " MouseDown at " + Time.time + ", metadata name is " + GetComponent<MetaData>().title);
    
            if (!GetComponent<Render>().visible)
            {
                // Debugger.Log(gameObject.name + " hided; HandPile virtual Card opening is " + Game.Ctx.CardOperator.pileHand.isVirtualOn + "; exit");
                return;
            }
            
            thisPile = Game.Ctx.CardOperator.GetCardPile(GetComponent<Card>());
    
            if (thisPile.gameObject.name == "HandPile" || thisPile.gameObject.name == "PlayPile")
            {
                movable = true;
            }
            else
            {
                movable = false;
                return;
            }
    
            _cursorShift = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            Game.Ctx.VfxOperator.draggedCard = GetComponent<Card>();
            
            if (thisPile == Game.Ctx.CardOperator.pileHand)
            {
                Game.Ctx.CardOperator.pileHand.VirtualInitialize();
            }
            
        }
    
        private void OnMouseDrag()
        {
            if (!movable) return;
            
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
    
        private void OnMouseUp()
        {
            // Debugger.Log(triggerPlayArea + " " + triggerHandArea);
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
                thisPile.AdjustAllPositions();
            }
    
            Game.Ctx.CardOperator.pileHand.VirtualDestroy(true);
            thisPile = null;
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