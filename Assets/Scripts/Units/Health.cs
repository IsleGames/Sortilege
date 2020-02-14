using System;
using _Editor;
using UnityEngine;

namespace Units
{
    public class Health : MonoBehaviour
    {
        // Make it SerializableField For now
        // Later it will read from an upper level
        public float maximumHealth = -1f;
        public float block;

        [NonSerialized]
        public float HitPoints;
        
        public void Start()
        {
            HitPoints = maximumHealth;
        }

        private float ValidityCheck(float expectedHitPoint)
        {
            if (expectedHitPoint < 0) expectedHitPoint = 0;
            if (expectedHitPoint > maximumHealth) expectedHitPoint = maximumHealth;
            
            return expectedHitPoint;
        }
        
        public void Damage(float amount, bool ignoreBlock = false)
        {
            if (amount < 0)
                Debugger.Warning("Negative amount detected for Damage", this);
                
            if (!ignoreBlock) 
                HitPoints = ValidityCheck(HitPoints - Mathf.Max(amount - block, 0));
            else
                HitPoints = ValidityCheck(HitPoints - Mathf.Max(amount, 0));
        }
        

        public void Heal(float amount)
        {
            if (amount < 0)
                Debugger.Warning("Negative amount detected for Damage", this);
                
            HitPoints = ValidityCheck(HitPoints + amount);
        }
        
        public void BlockAlter(float amount)
        {
            block += amount;
        }
        
        public bool IsDead()
        {
            return !Mathf.Approximately( maximumHealth, -1f) && Mathf.Approximately(HitPoints, 0f);
        }
        
        public bool IsFullHealth()
        {
            return !Mathf.Approximately( maximumHealth, -1f) && Mathf.Approximately(HitPoints, maximumHealth);
        }
    }
}