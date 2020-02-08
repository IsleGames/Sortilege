using System;
using System.Collections.Generic;
using UnityEngine;

using Cards;
using Effects;

namespace Data
{
    [UnityEngine.CreateAssetMenu(fileName = "New CardData", menuName = "Card Data", order = 51)]
    public class CardData : UnityEngine.ScriptableObject
    {
        public string title;
        public StrategyType strategy;
        public AttributeType attribute;
        public string description;
        
        public List<Effect> effectList;
    }
}