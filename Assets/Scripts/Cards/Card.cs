using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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


        public UnityEvent onDraw = new UnityEvent();
        public UnityEvent onPlay = new UnityEvent();


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

            GetComponent<Ability>().disableRetract = cardData.disableRetract;
            GetComponent<Ability>().effectList = new List<Effect>(cardData.effectList);
        }
        
        public void Apply(Unit target, float streakCount)
        {
            GetComponent<Ability>().Apply(target, streakCount);
                
            Debugger.OneOnOneStat();
            
            if (Game.Ctx.IsBattleEnded()) Game.Ctx.EndGame();
        }
    }
}