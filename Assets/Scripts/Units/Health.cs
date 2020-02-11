using _Editor;
using UnityEngine;

namespace Units
{
    public class Health : MonoBehaviour
    {
        private bool _initialized = false;
        
        // Make it SerializableField For now
        // Later it will read from an upper level
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

        public void Damage(float amount)
        {
            if (amount < 0)
                Debugger.Warning("Negative amount detected for Damage", this);
                
            Debugger.Log("Deal " + amount + " Damage to " + health + " Health", this);
            health = ValidityCheck(health - Mathf.Max(amount - block, 0));
        }
        
        public void Heal(float amount)
        {
            if (amount < 0)
                Debugger.Warning("Negative amount detected for Damage", this);
                
            health = ValidityCheck(health + amount);
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