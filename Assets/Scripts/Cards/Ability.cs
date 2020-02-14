using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;
using Effects;

using _Editor;

namespace Cards
{
    public class Ability: MonoBehaviour
    {
        public bool disableRetract;
        
        public List<Effect> effectList;

        public void Apply(Unit target, float streakCount)
        {
            foreach (Effect effect in effectList)
                if (streakCount >= effect.minStreak)
                    switch (effect.affectiveUnit)
                    {
                        case UnitType.Player:
                            effect.Apply(Game.Ctx.Player, streakCount);
                            break;
                        case UnitType.Enemy:
                            effect.Apply(target, streakCount);
                            break;
                        default:
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