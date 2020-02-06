using _Editor;
using UnityEngine;

namespace Units
{
    public class Health : MonoBehaviour
    {
        public float health;
        public float maximumHealth;

        private void Start()
        {
            
        }

        public void Initialize(float maxHealth)
        {
            maximumHealth = maxHealth;
            health = maxHealth;
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
            return Mathf.Approximately(health, 0f);
        }
        
        public bool IsFullHealth()
        {
            return Mathf.Approximately(health, maximumHealth);
        }
    }
}