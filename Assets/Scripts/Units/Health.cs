using System;
using _Editor;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Units
{
    public class Health : MonoBehaviour
    {
        // Make it SerializableField For now
        // Later it will read from an upper level
        [FormerlySerializedAs("maximumHealth")] public float maximumHitPoints = -1f;

        public float barrierHitPoints = 0;
        
        [SerializeField]
        public float onGoingEffectAmount = -1;

        [FormerlySerializedAs("HitPoints")] public float hitPoints = -1f;
        
        public void Start()
        {
            if (hitPoints < 0f)
                hitPoints = maximumHitPoints;
        }

        public void Initialize()
        {
            GetComponentInChildren<HealthBar>().UpdateStatus(false);
        }

        public float GetMaximumDisplayHP()
        {
            if (barrierHitPoints + hitPoints > maximumHitPoints)
                return barrierHitPoints + hitPoints;
            else
                return maximumHitPoints;
        }

        private float ValidityCheck(float expectedHitPoint)
        {
            if (expectedHitPoint < 0) expectedHitPoint = 0;
            if (expectedHitPoint > maximumHitPoints) expectedHitPoint = maximumHitPoints;
            
            return expectedHitPoint;
        }
        
        public void Damage(float amount, bool ignoreBarrier = false)
        {
            if (amount < 0)
                Debugger.Warning("Negative amount detected for Damage", this);

            // Trigger onDamage Buffs
            // Currently onDamage effect goes before the damage happens
            onGoingEffectAmount = amount;
            GetComponent<Unit>().onDamage.Invoke();
            amount = onGoingEffectAmount;

            if (barrierHitPoints > 0f && !ignoreBarrier)
            {
                if (amount < barrierHitPoints)
                {
                    barrierHitPoints -= amount;
                    amount = 0f;
                }
                else
                {
                    amount -= barrierHitPoints;
                    barrierHitPoints = 0f;
                }
            }
            
            if (!Mathf.Approximately(amount, 0f))
            {
                hitPoints = ValidityCheck(hitPoints - amount);
                GetComponent<Unit>().beingDamagedSomewhere = true;
            }
            
            GetComponent<Unit>().onHealthChange.Invoke();
        }

        public void Heal(float amount)
        {
            if (amount < 0)
                Debugger.Warning("Negative amount detected for Damage", this);
            
            hitPoints = ValidityCheck(hitPoints + amount);
            GetComponent<Unit>().onHealthChange.Invoke();
        }
        
        public void AddBarrier(float amount)
        {
            barrierHitPoints += amount;
            GetComponent<Unit>().onHealthChange.Invoke();
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