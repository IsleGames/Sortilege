using System;
using _Editor;
using UnityEngine;

namespace Units
{
    public class Health : MonoBehaviour
    {
        private bool _initialized = false;
        
        // Make it SerializableField For now
        // Later it will read from an upper level
        [NonSerialized]
        public float health;
        public float maximumHealth;

        public float block;

        public void Initialize()
        {
            // maximumHealth = maxHealth;
            health = maximumHealth;

            _initialized = true;
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
                
            Debugger.Log("Deal " + amount + " Damage to " + health + " Health", this);
            if (!ignoreBlock) 
                health = ValidityCheck(health - Mathf.Max(amount - block, 0));
            else
                health = ValidityCheck(health - Mathf.Max(amount, 0));
        }
        

        public void Heal(float amount)
        {
            if (amount < 0)
                Debugger.Warning("Negative amount detected for Damage", this);
                
            health = ValidityCheck(health + amount);
        }
        
        public void BlockAlter(float amount)
        {
            if (amount < 0)
                Debugger.Warning("Negative amount detected for BlockAlter", this);
                
            block += amount;
        }
        
        public bool IsDead()
        {
            return _initialized && Mathf.Approximately(health, 0f);
        }
        
        public bool IsFullHealth()
        {
            return _initialized && Mathf.Approximately(health, maximumHealth);
        }
    }
}