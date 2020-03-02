using System;
using System.Data;
using Units;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

using _Editor;
using Cards;

// This should be called OnPlayEffect
namespace Effects
{
	public enum UnitType : int
	{
		Player,
		SingleEnemy,
		NearbyEnemy,
		AllEnemy,
		RandomEnemy
	}

	public enum EffectType : int
	{
		Damage,
		Heal,
		Barrier,
		DamageIgnoreBarrier,
		DiscardDeceiver,
		DamageOnDeceiverInDiscardPile,
		DiscardAllWithPerCardDamage
	}

	[Serializable]
	public class Effect
	{
		public UnitType affectiveUnit;

		public EffectType type;
		public float amount, maxDeviation;

		public int minStreak;
		public bool notAmplified;
		
		public Effect(
			UnitType affectiveUnit,
			EffectType type,
			float amount,
			int streakCount = 1,
			int turnCountEnemy = 1,
			bool notAmplified = false,
			float maxDeviation = 0f
		)
		{
			if (amount < 0f)
				throw new ConstraintException("Effect amount should be larger than zero");
			
			this.affectiveUnit = affectiveUnit;
			this.type = type;
			this.amount = amount;
			this.minStreak = streakCount;
			this.notAmplified = notAmplified;
			this.maxDeviation = maxDeviation;
		}

		public void Apply(Unit unit, float multiplier)
		{
			// if (!unit.GetComponent(affectiveUnit.ToString("G")))
			// 	throw new InvalidOperationException("Effect unit type mismatch: Expected " + affectiveUnit);
			if (Game.Ctx.CardOperator.pilePlay.Count() < minStreak)
				throw new InvalidOperationException("Minimum streak not satisfied for effect");

			float localAmount, totAmount;
			int counter;

			if (Mathf.Approximately(maxDeviation, 0f))
				localAmount = amount;
			else
				localAmount = Mathf.Round(Random.Range(amount - maxDeviation, amount + maxDeviation));
			
			if (notAmplified)
				totAmount = localAmount;
			else
				totAmount = localAmount * multiplier;
			
			switch (type)
			{
				case EffectType.Damage: 
				    unit.GetComponent<Health>().Damage(totAmount);
				    break;
				case EffectType.Heal:
				    unit.GetComponent<Health>().Heal(totAmount);
				    break;
				case EffectType.Barrier:
				    unit.GetComponent<Health>().AddBarrier(totAmount);
				    break;
				case EffectType.DamageIgnoreBarrier: 
				    unit.GetComponent<Health>().Damage(totAmount, true);
				    break;
				case EffectType.DiscardDeceiver:
					Game.Ctx.CardOperator.DiscardStrategyTypeCards(StrategyType.Deceiver);
				    break;
				case EffectType.DamageOnDeceiverInDiscardPile:
					counter = Game.Ctx.CardOperator.pileDiscard.GetStrategyTypeCards(StrategyType.Deceiver).Count;
					if (notAmplified)
						totAmount = localAmount * counter;
					else
						totAmount = localAmount * counter * multiplier;
					unit.GetComponent<Health>().Damage(totAmount);
					break;
				case EffectType.DiscardAllWithPerCardDamage:
					counter = Game.Ctx.CardOperator.DiscardAllHandCards();
					if (notAmplified)
						totAmount = localAmount * counter;
					else
						totAmount = localAmount * counter * multiplier;
					unit.GetComponent<Health>().Damage(totAmount);
					break;
			}
		}

		// Need change
        public string Info()
        {
	        return affectiveUnit + " " + type + " " + amount;
        }

	}
}