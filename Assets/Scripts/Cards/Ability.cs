using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;
using Effects;

using _Editor;

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

    public class Ability : MonoBehaviour
    {
        protected List<Effect> EffectList;
    
        private void Start()
        {
            EffectList = new List<Effect>();
        }

        public void AddEffect(Effect effect)
        {
            Debugger.Log(effect.Diff);
            
            EffectList.Add(effect);
        }

        public void Apply(Player player, Enemy enemy)
        {
            foreach (Effect effect in EffectList)
            {
                if (effect.AffectiveUnit == UnitType.Player)
                    effect.Apply(player.gameObject);
                else if (effect.AffectiveUnit == UnitType.Enemy)
                    effect.Apply(enemy.gameObject);
                else
                    throw new NullReferenceException("Unknown Unit Type");
            }
        }
    }
}