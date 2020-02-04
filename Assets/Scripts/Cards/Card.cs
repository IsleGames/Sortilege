using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;
using Effects;

namespace Cards
{
    // public enum CardStatus : int
    // {
    //     Unknown,
    //     Stored,
    //     Decked,
    //     Held,
    //     Discarded,
    // }

    public class Card : MonoBehaviour
    {
        protected string Name;

        protected List<Effect> EffectList;
    
        private void Start()
        {
            EffectList = new List<Effect>();
        }

        public void LoadData()
        {
            throw new NotImplementedException();
        }

        public void Apply(Player player, Enemy enemy)
        {
            foreach (Effect effect in EffectList)
            {
                if (effect.AffectiveUnit == UnitType.Player)
                {
                    effect.Apply(player.gameObject);
                }
                else if (effect.AffectiveUnit == UnitType.Enemy)
                {
                    effect.Apply(enemy.gameObject);
                }
                else
                {
                    throw new NullReferenceException("Unknown Unit Type");
                }
            }
        }
    }
}