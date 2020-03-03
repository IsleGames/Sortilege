using System;
using System.Collections.Generic;
using UnityEngine;

using Effects;

namespace Data
{
    [Serializable]
    public class EnemyAbilityData
    {
        public string title;
        public string description;
        
        // Internally, Streak Count is used as turnCount here
        // You can make the enemy do certain things after an amount of turns
        public List<Effect> effectList;
        public List<BuffEffect> buffList;
    }
    
    [CreateAssetMenu(fileName = "New EnemyData", menuName = "Enemy Data", order = 52)]
    public class EnemyData : ScriptableObject
    {
        public string title;
        public string description;
        public Sprite displayImage;
        public float maximumHealth;

        public List<BuffEffect> initialBuffList;

        public List<EnemyAbilityData> abilityList;
    }
}