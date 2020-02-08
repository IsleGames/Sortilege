using _Editor;
using UnityEngine;

namespace Units
{
    public class Health : MonoBehaviour
    {
        private bool _initialized = false;
        
        public float health;
        public float maximumHealth;

        public void Initialize(float maxHealth)
        {
            maximumHealth = maxHealth;
            health = maxHealth;

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
            health = ValidityCheck(health - amount);
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