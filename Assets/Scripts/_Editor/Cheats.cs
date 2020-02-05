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
            
            GetTemporalCard();
        }

        public void GetTemporalCard()
        {
            Effect e1 = new Effect(UnitType.Player, 3);

            GameObject newCard = Instantiate(_cardPrefab) as GameObject;

            if (newCard != null)
            {
                MetaData newCardMD = newCard.GetComponent<MetaData>();
                
                newCardMD.title = "testCard";
                
                newCardMD.strategy = StrategyType.Berserker;
                newCardMD.attribute = AttributeType.None;

                newCardMD.level = 0;
                newCardMD.maxLevel = 0;
                
                Debugger.Log(newCardMD.title);
                
                newCard.GetComponent<Ability>().AddEffect(e1);

                Game.Ctx.CardOperator.AddCard(newCard.GetComponent<Card>());
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