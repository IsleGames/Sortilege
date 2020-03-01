using System;
using System.Collections.Generic;
using UnityEngine;

using Cards;
using Effects;

namespace Data
{
    [CreateAssetMenu(fileName = "New CardData", menuName = "Card Data", order = 51)]
    public class CardData : ScriptableObject
    {
        public string title;

        public StrategyType strategy;
        public AttributeType attribute;
        public string description;
        
        // public bool disableRetract = false;
        
        public List<Effect> effectList;
        public List<BuffEffect> buffList;
    }
}