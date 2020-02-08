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

    public class Ability: MonoBehaviour
    {
        public List<Effect> effectList;

        private void Start()
        {
            effectList = new List<Effect>();
        }

        public void AddEffect(Effect effect)
        {
            Debugger.Log(effect.damage);
            
            effectList.Add(effect);
        }

        public void Apply(Player player, Enemy enemy)
        {
            foreach (Effect effect in effectList)
            {
                if (effect.affectiveUnit == UnitType.Player)
                    effect.Apply(player.gameObject);
                else if (effect.affectiveUnit == UnitType.Enemy)
                    effect.Apply(enemy.gameObject);
                else
                    throw new NullReferenceException("Unknown Unit Type");
            }
        }

        public string Text()
        {
            var text = "";
            if (effectList != null)
            {
                foreach (var effect in effectList)
                {
                    text += effect?.Text() + "\n";
                }
            }
            return text;
        }
    }
}