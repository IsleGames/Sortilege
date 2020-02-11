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
            _cardPrefab = Resources.Load("Prefabs/Card");
            
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                PlayCard(0);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                PlayCard(1);
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                Game.Ctx.Player.EndTurn();
            }
        }

        private void PlayCard(int index)
        {
            Game.Ctx.CardOperator.PlayCard(Game.Ctx.CardOperator.Hand[index]);
        }

    }
}

// private void GetTemporalCard()
// {
//     // Effect e1 = new Effect(UnitType.Player, 3);
//
//     GameObject newCard = Instantiate(_cardPrefab) as GameObject;
//
//     if (newCard != null)
//     {
//         MetaData newCardMD = newCard.GetComponent<MetaData>();
//         
//         newCardMD.title = "testCard";
//         
//         newCardMD.strategy = StrategyType.Berserker;
//         newCardMD.attribute = AttributeType.None;
//
//         newCardMD.level = 0;
//         newCardMD.maxLevel = 0;
//         
//         // newCard.GetComponent<Ability>().AddEffect(e1);
//
//         Card tempc = newCard.GetComponent<Card>();
//         
//         Game.Ctx.CardOperator.Hand.Add(tempc);
//     }
//     else
//     {
//         throw new NullReferenceException();
//     }
//     
//     Debugger.Log("Card Added");
// }
        
        
// public void PlayTemporalCard()
// {
//     Debugger.Log("Player Play Card 0");
//     Debugger.Log("Cardlist Length: " + Game.Ctx.CardOperator.CardList.Count);
//     
//     Game.Ctx.CardOperator.Hand[0].Apply(Game.Ctx.Enemy);
//     Game.Ctx.Player.EndTurn();
// }