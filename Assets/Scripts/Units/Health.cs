using System;
using _Editor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Units
{
    public class Health : MonoBehaviour
    {
        // Make it SerializableField For now
        // Later it will read from an upper level
        [FormerlySerializedAs("maximumHealth")] public float maximumHitPoints = -1f;
        public float block;

        [FormerlySerializedAs("HitPoints")] public float hitPoints = -1f;
        
        public void Start()
        {
            if (hitPoints < 0f)
                hitPoints = maximumHitPoints;
        }

        private float ValidityCheck(float expectedHitPoint)
        {
            if (expectedHitPoint < 0) expectedHitPoint = 0;
            if (expectedHitPoint > maximumHitPoints) expectedHitPoint = maximumHitPoints;
            
            return expectedHitPoint;
        }
        
        public void Damage(float amount, bool ignoreBlock = false)
        {
            if (amount < 0)
                Debugger.Warning("Negative amount detected for Damage", this);
                
            if (!ignoreBlock) 
                hitPoints = ValidityCheck(hitPoints - Mathf.Max(amount - block, 0));
            else
                hitPoints = ValidityCheck(hitPoints - Mathf.Max(amount, 0));
        }
        

        public void Heal(float amount)
        {
            if (amount < 0)
                Debugger.Warning("Negative amount detected for Damage", this);
                
            hitPoints = ValidityCheck(hitPoints + amount);
        }
        
        public void BlockAlter(float amount)
        {
            block += amount;
        }
        
        public bool IsDead()
        {
            return !Mathf.Approximately( maximumHitPoints, -1f) && Mathf.Approximately(hitPoints, 0f);
        }
        
        public bool IsFullHealth()
        {
            return !Mathf.Approximately( maximumHitPoints, -1f) && Mathf.Approximately(hitPoints, maximumHitPoints);
        }
    }
}