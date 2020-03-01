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
        public CardData cardData { get; private set; }

        public void Info()
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
            
            GetComponent<MetaData>().description = cardData.description;

            // GetComponent<Ability>().disableRetract = cardData.disableRetract;
            
            GetComponent<MetaData>().ability.effectList = new List<Effect>(cardData.effectList);
            GetComponent<MetaData>().ability.buffEffectList = new List<BuffEffect>(cardData.buffList);
        }

        public void SetAvailability(bool availability)
        {
            GetComponent<CardRender>().SetAvailability(availability);
            GetComponent<CardEvent>().availability = availability;
        } 
        
        
        public void CheckChainedAvailability()
        {
            Card compCard = Game.Ctx.CardOperator.pilePlay.GetCurrentTopCard();
            SetAvailability(!compCard || compCard && GetComponent<MetaData>().HasSharedProperty(compCard));
        } 
        
        public void Apply(Unit target, float streakCount)
        {
            GetComponent<MetaData>().ability.Apply(target, streakCount);

            if (Game.Ctx.IsBattleEnded()) Game.Ctx.EndGame();
        }
    }
}