using System;
using System.Data;
using Units;
using UnityEngine;
using Object = UnityEngine.Object;

using _Editor;

// Task: Rewrite the file using Scriptable Objects
namespace Effects
{
	public enum UnitType : int
	{
		Unknown,
		Player,
		Enemy
	}

	public enum EffectType : int
	{
		Damage,
		Heal,
		Barrier,
		DamageIgnoreBlock
	}

	[Serializable]
	public class Effect
	{
		public UnitType affectiveUnit;

		public EffectType type;
		public float amount;

		public int minStreak = 1;
		public bool notAmplified = false;

		public Effect(
			UnitType affectiveUnit,
			EffectType type,
			float amount,
			int streakCount = 1,
			bool notAmplified = false
		)
		{
			if (amount < 0f)
				throw new ConstraintException("Effect amount should be larger than zero");
			
			this.affectiveUnit = affectiveUnit;
			this.type = type;
			this.amount = amount;
			this.minStreak = streakCount;
			this.notAmplified = notAmplified;
		}

		public void Apply(Unit unit, float multiplier)
		{
			if (!unit.GetComponent(this.affectiveUnit.ToString("G")))
				throw new InvalidOperationException("Effect unit type mismatch: Expected " + this.affectiveUnit);
			if (Game.Ctx.CardOperator.pilePlay.Count() < minStreak)
				throw new InvalidOperationException("Minimum streak not satisfied for effect");

			float totAmount;
			if (notAmplified)
				totAmount = amount;
			else
				totAmount = amount * multiplier;
			
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
				case EffectType.DamageIgnoreBlock: 
				  unit.GetComponent<Health>().Damage(totAmount, true);
				  break;
			}
		}

		// Need change
        public string Text()
        {
	        return affectiveUnit + " " + type + " " + amount;
        }



    }
}