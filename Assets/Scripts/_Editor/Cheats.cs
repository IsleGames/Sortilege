using System;
using System.Collections.Generic;
using UnityEngine;

using Cards;
using Effects;
using Library;
using Object = UnityEngine.Object;
using Card = Cards.Card;

namespace _Editor
{
    public class Cheats : MonoBehaviour
    {
        private Object _cardPrefab;

        private void Start()
        {
            _cardPrefab = Resources.Load("Card");
            
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.G))
            {
                GetTemporalCard();
            }
        }
        
        public void GetTemporalCard()
        {
            // Effect e1 = new Effect(UnitType.Player, 3);

            GameObject newCard = Instantiate(_cardPrefab) as GameObject;

            if (newCard != null)
            {
                MetaData newCardMD = newCard.GetComponent<MetaData>();
                
                newCardMD.title = "testCard";
                
                newCardMD.strategy = StrategyType.Berserker;
                newCardMD.attribute = AttributeType.None;

                newCardMD.level = 0;
                newCardMD.maxLevel = 0;
                
                Debugger.Log(newCard);
                
                // newCard.GetComponent<Ability>().AddEffect(e1);

                Card tempc = newCard.GetComponent<Card>();
                Debugger.Log(tempc);
                
                Debugger.Log(Game.Ctx);
                
                Game.Ctx.CardOperator.AddCard(tempc);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
        
        
        public void PlayTemporalCard()
        {

        }
    }
}