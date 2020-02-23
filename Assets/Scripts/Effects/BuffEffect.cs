using System;
using System.Data;
using Managers;
using UnityEngine;

using Units;

namespace Effects
{
	public enum BuffType : int
	{
		Block,
		Forge,
		Thorns,
		Plague,
		Flinch,
		Voodoo,
		Breeze,
	}
	
	[Serializable]
    public class BuffEffect
    {
		public UnitType affectiveUnit;
		public BuffType type;

		public float amount;
		public int minStreak = 1;
		public bool notAmplified = false;
		
	    public BuffEffect(
			UnitType affectiveUnit,
			BuffType type,
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
			
			unit.GetComponent<BuffManager>().Create(type, totAmount);
	    }
    }
}