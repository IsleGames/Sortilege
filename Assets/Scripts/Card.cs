using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Player
{
    public enum CardStatus : int
    {
        Unknown,
        Stored,
        Decked,
        Held,
        Discarded,
    }

    public class Card : MonoBehaviour
    {
        protected CardStatus status;
        
        public CardStatus Status
        {
            get => status;
            set => status = value;
        }

        protected Skill baseSkill, endSkill;

        public void SetStatus(CardStatus newStatus)
        {
            Status = newStatus;
        }
        
        private void Start()
        {
            status = CardStatus.Unknown;
            
            baseSkill = gameObject.AddComponent<Skill>();
            baseSkill.isEndSkill = false;
            
            endSkill = gameObject.AddComponent<Skill>();
            endSkill.isEndSkill = true;
        }
 
        public void LoadData()
        {
            throw new NotImplementedException();
        }


        public void ApplyBaseSkill()
        {
            if (status != CardStatus.Held)
                throw new InvalidOperationException();
            
            
        }
    }
    
    public class Skill : MonoBehaviour
    {
        public bool isEndSkill;
        public float damage;

        private void Start()
        {
            isEndSkill = true;
            
            damage = 2;
        }

        public void LoadData()
        {
            throw new NotImplementedException();
        }
        
        

    }
}