using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;
using Effects;

using _Editor;

namespace Effects
{
    [Serializable]
    public class Ability
    {
        public int activateTurnCount = 1;
        
        public List<Effect> effectList;
        public List<BuffEffect> buffEffectList;
        
        public void ApplyAsEnemy(Unit self)
        {
            // Effects
            foreach (Effect effect in effectList)
                switch (effect.affectiveUnit)
                {
                    case UnitType.Player:
                        effect.Apply(Game.Ctx.player, 1);
                        break;
                    case UnitType.Enemy:
                        effect.Apply(self, 1);
                        break;
                    default:
                        throw new NullReferenceException("Unknown Unit Type");
                }
            
            // Buff Effects
            foreach (BuffEffect buffEffect in buffEffectList)
                switch (buffEffect.affectiveUnit)
                {
                    case UnitType.Player:
                        buffEffect.Apply(Game.Ctx.player, 1);
                        break;
                    case UnitType.Enemy:
                        buffEffect.Apply(self, 1);
                        break;
                    default:
                        throw new NullReferenceException("Unknown Unit Type");
                }
        }

        public void ApplyAsCard(Unit target, float streakCount)
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
    }
}