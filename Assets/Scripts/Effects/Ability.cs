using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;
using Effects;

using _Editor;
using Units.Enemies;
using Random = UnityEngine.Random;

namespace Effects
{
    [Serializable]
    public class Ability
    {
        public int activateTurnCount = 1;
        
        public List<Effect> effectList;
        public List<BuffEffect> buffEffectList;

        public void ApplyAsCard(Unit target, float streakCount)
        {
            Apply(target, streakCount);
        }

        public void ApplyAsEnemy(Unit self)
        {
            Apply(self, 1);
        }

        private void Apply(Unit target, float multiplier)
        {
            int thisIndex = Game.Ctx.EnemyOperator.EnemyList.IndexOf((Enemy)target);
            
            // Effects
            foreach (Effect effect in effectList)
                if (multiplier >= effect.minStreak)
                    switch (effect.affectiveUnit)
                    {
                        case UnitType.Player:
                            effect.Apply(Game.Ctx.player, multiplier);
                            break;
                        case UnitType.SingleEnemy:
                            effect.Apply(target, multiplier);
                            break;
                        case UnitType.NearbyEnemy:
                            if (Game.Ctx.EnemyOperator.EnemyList.Count > 1)
                            { 
                                if (thisIndex == 0)
                                    effect.Apply(Game.Ctx.EnemyOperator.EnemyList[thisIndex + 1], multiplier);
                                else if (thisIndex == Game.Ctx.EnemyOperator.EnemyList.Count - 1)
                                    effect.Apply(Game.Ctx.EnemyOperator.EnemyList[thisIndex - 1], multiplier);
                                else
                                {
                                    int indexDelta = Random.Range(0, 1) * 2 - 1;
                                    effect.Apply(Game.Ctx.EnemyOperator.EnemyList[thisIndex + indexDelta], multiplier);
                                }
                            }
                            break;
                        case UnitType.AllEnemy:
                            foreach (var enemy in Game.Ctx.EnemyOperator.EnemyList)
                            {
                                effect.Apply(enemy, multiplier);
                            }
                            break;
                        case UnitType.RandomEnemy:
                            int randIndex = Random.Range(0, Game.Ctx.EnemyOperator.EnemyList.Count);
                            effect.Apply(Game.Ctx.EnemyOperator.EnemyList[randIndex], multiplier);
                            break;
                    }
            
            // Buff Effects
            foreach (BuffEffect buffEffect in buffEffectList)
                if (multiplier >= buffEffect.minStreak)
                    switch (buffEffect.affectiveUnit)
                    {
                        case UnitType.Player:
                            buffEffect.Apply(Game.Ctx.player, multiplier);
                            break;
                        case UnitType.SingleEnemy:
                            buffEffect.Apply(target, multiplier);
                            break;
                        case UnitType.NearbyEnemy:
                            if (Game.Ctx.EnemyOperator.EnemyList.Count > 1)
                            {
                                if (thisIndex == 0)
                                    buffEffect.Apply(Game.Ctx.EnemyOperator.EnemyList[thisIndex + 1], multiplier);
                                else if (thisIndex == Game.Ctx.EnemyOperator.EnemyList.Count - 1)
                                    buffEffect.Apply(Game.Ctx.EnemyOperator.EnemyList[thisIndex - 1], multiplier);
                                else
                                {
                                    int indexDelta = Random.Range(0, 1) * 2 - 1;
                                    buffEffect.Apply(Game.Ctx.EnemyOperator.EnemyList[thisIndex + indexDelta], multiplier);
                                }
                            }
                            break;
                        case UnitType.AllEnemy:
                            foreach (var enemy in Game.Ctx.EnemyOperator.EnemyList)
                            {
                                buffEffect.Apply(enemy, multiplier);
                            }
                            break;
                        case UnitType.RandomEnemy:
                            int randIndex = Random.Range(0, Game.Ctx.EnemyOperator.EnemyList.Count);
                            buffEffect.Apply(Game.Ctx.EnemyOperator.EnemyList[randIndex], multiplier);
                            break;
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