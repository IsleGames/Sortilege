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
		Enemy
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

        public float EffectSize(float multiplier)
        {
            if (Game.Ctx.CardOperator.pilePlay.Count() < minStreak)
            { return 0f; }

            float total = amount;
            if (!notAmplified)
            {
                total *= multiplier;
            }

            switch (type)
            {
                case EffectType.DiscardAllWithPerCardDamage:
                    return total * Game.Ctx.CardOperator.pileHand.Count();
                case EffectType.DamageOnDeceiverInDiscardPile:
                    return total * Game.Ctx.CardOperator.pileDiscard.GetStrategyTypeCards(StrategyType.Deceiver).Count;
                case EffectType.DiscardDeceiver:
                    return 0f;
                default:
                    return total;
            }
        }

        public (float, float) EffectRange(float multiplier )
        {
            float size = EffectSize(multiplier);
            float deviation = notAmplified ? maxDeviation : maxDeviation * multiplier;
            return (size - deviation, size + deviation);
        }

		public void Apply(Unit unit, float multiplier)
		{
			if (!unit.GetComponent(affectiveUnit.ToString("G")))
				throw new InvalidOperationException("Effect unit type mismatch: Expected " + affectiveUnit);
			if (Game.Ctx.CardOperator.pilePlay.Count() < minStreak)
				throw new InvalidOperationException("Minimum streak not satisfied for effect");


            (float low, float high) = EffectRange(multiplier);
            float totAmount = Mathf.Round(Random.Range(low, high));
			
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
					unit.GetComponent<Health>().Damage(totAmount);
					break;
				case EffectType.DiscardAllWithPerCardDamage:
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