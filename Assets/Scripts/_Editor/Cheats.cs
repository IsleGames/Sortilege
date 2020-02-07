using System;
using System.Collections.Generic;
using UnityEngine;

using Cards;
using Effects;
using Library;
using Units;
using Object = UnityEngine.Object;
using Card = Cards.Card;
using Debug = System.Diagnostics.Debug;

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
            if (Input.GetKeyUp(KeyCode.A))
            {
                GetTemporalCard();
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                SetUnitStatus();
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                PlayTemporalCard();
            }
        }

        public void SetUnitStatus()
        {
            Game.Ctx.Player.GetComponent<Health>().Initialize(10);
            Game.Ctx.Enemy.GetComponent<Health>().Initialize(8);
            
            Debugger.Log("Player HP: " + Game.Ctx.Player.GetComponent<Health>().health);
            Debugger.Log("Enemy HP: " + Game.Ctx.Enemy.GetComponent<Health>().health);
        }
        
        private void GetTemporalCard()
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
                
                // newCard.GetComponent<Ability>().AddEffect(e1);

                Card tempc = newCard.GetComponent<Card>();
                
                Game.Ctx.CardOperator.AddCard(tempc);
            }
            else
            {
                throw new NullReferenceException();
            }
            
            Debugger.Log("Card Added");
        }
        
        
        public void PlayTemporalCard()
        {
            Debugger.Log("Player Play Card 0");
            Debugger.Log("Cardlist Length: " + Game.Ctx.CardOperator.CardList.Count);
            
            Game.Ctx.CardOperator.CardList[0].Apply(Game.Ctx.Enemy);
            Game.Ctx.Player.EndTurn();
        }
    }
}