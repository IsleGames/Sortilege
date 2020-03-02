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
        public List<BuffEffect> buffEffectList;
        EffectDescription effectDescription = new EffectDescription();
        BuffDescription buffDescription = new BuffDescription();


        private void Update()
        {
            
            effectDescription.Update(effectList,
                Game.Ctx.CardOperator.pilePlay.Count(),
                Game.Ctx.CardOperator.pileDiscard
                    .GetStrategyTypeCards(StrategyType.Deceiver).Count,
                Game.Ctx.CardOperator.pileHand.Count());

            buffDescription.Update(buffEffectList,
                Game.Ctx.CardOperator.pilePlay.Count());
        }

        public void Apply(Unit target, float streakCount)
        {
            // Effects
            foreach (Effect effect in effectList)
                if (streakCount >= effect.minStreak)
                    switch (effect.affectiveUnit)
                    {
                        case UnitType.Player:
                            effect.Apply(Game.Ctx.player, streakCount);
                            break;
                        case UnitType.Enemy:
                            effect.Apply(target, streakCount);
                            break;
                        default:
                            throw new NullReferenceException("Unknown Unit Type");
                    }
            
            // Buff Effects
            foreach (BuffEffect buffEffect in buffEffectList)
                if (streakCount >= buffEffect.minStreak)
                    switch (buffEffect.affectiveUnit)
                    {
                        case UnitType.Player:
                            buffEffect.Apply(Game.Ctx.player, streakCount);
                            break;
                        case UnitType.Enemy:
                            buffEffect.Apply(target, streakCount);
                            break;
                        default:
                            throw new NullReferenceException("Unknown Unit Type");
                    }
        }

        public string Info()
        {
            var text = "";
            if (effectList != null)
            {
                foreach (var effect in effectList)
                {
                    text += effect?.Info() + "\n";
                }
            }
            return text;
        }

        public string Description()
        {
            return effectDescription.ToString() + "\n\n"
                + buffDescription.ToString();
        }
    }
}