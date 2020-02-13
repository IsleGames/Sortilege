using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Units;
using Effects;
using Data;

using _Editor;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        [SerializeField]
        private CardData cardData;

        public void LogInfo()
        {
            Debugger.Log(cardData.title + ' ' + cardData.strategy + ' ' + cardData.attribute);
        }

        public void Initialize(CardData newCardData)
        {
            cardData = newCardData;
            
            // For inspector visualization
            gameObject.name = cardData.title;
            
            GetComponent<MetaData>().title = cardData.title;
            GetComponent<MetaData>().strategy = cardData.strategy;
            GetComponent<MetaData>().attribute = cardData.attribute;
            GetComponent<Ability>().effectList = new List<Effect>(cardData.effectList);
        }
        
        public void Apply(Unit target)
        {
            GetComponent<Ability>().Apply(target);
                
            Debugger.OneOnOneStat();
            
            if (Game.Ctx.IsBattleEnded()) Game.Ctx.EndGame();
        }
    }
}