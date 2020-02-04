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

        public void SetMaximumHealth(int maxHealth, bool fillHealth = false)
        {
            maximumHealth = maxHealth;
            if (fillHealth) health = maxHealth;
        }

        public void ChangeHealth(float amount)
        {
            float expectedHitPoint = health + amount;

            if (amount > 0)
            {
                if (expectedHitPoint < 0) expectedHitPoint = 0;
            }
            else if (amount < 0)
                if (expectedHitPoint > maximumHealth) expectedHitPoint = maximumHealth;

            health = expectedHitPoint;
        }

        public bool IsZeroHealth()
        {
            return Mathf.Approximately(health, 0f);
        }
        
        public bool IsFullHealth()
        {
            return Mathf.Approximately(health, maximumHealth);
        }
    }
}