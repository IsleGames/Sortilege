using System;
using System.Collections.Generic;
using Effects;
using UnityEngine;

namespace Cards
{
    // Both StrategyType.None and AttributeType.None is set to be non-functional
    // for the card. That is, it is not considered to be available as part of the
    // chain.
    
    public enum StrategyType : int
    {
        None,
        Detriment,
        Berserker,
        Craftsman,
        Knight,
        Sorcerer,
        Deceiver
    }
    
    public enum AttributeType : int
    {
        None,
        Thunder,
        Infernal,
        Venom,
        Storm
    }
    public class MetaData : MonoBehaviour
    {
        public string title;
        public string description;
        
        public StrategyType strategy = StrategyType.None;
        public AttributeType attribute = AttributeType.None;

        public UnitType target;

        public Ability ability;
            
        public int level;
        public int maxLevel;

        public bool HasSharedProperty(Card otherCard) 
        {
            MetaData otherMeta = otherCard.GetComponent<MetaData>();

            if (otherMeta.strategy != StrategyType.None && strategy == otherMeta.strategy)
                return true;
            if (otherMeta.attribute != AttributeType.None && attribute == otherMeta.attribute)
                return true;

            return false;
        }
    }
}