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
        public List<Effect> effectList;

        public void Apply(Unit target)
        {
            foreach (Effect effect in effectList)
            {
                if (effect.affectiveUnit == UnitType.Player)
                    effect.Apply(Game.Ctx.Player);
                else if (effect.affectiveUnit == UnitType.Enemy)
                    effect.Apply(target);
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