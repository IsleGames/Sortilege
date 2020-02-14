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
		IncBlock,
		DecBlock,
		DamageIgnoreBlock
	}

	[Serializable]
	public class Effect
	{
		public UnitType affectiveUnit;

		public EffectType type;
		public float amount;

		public bool notAmplified;

		public Effect(UnitType affectiveUnit, EffectType type, float amount, bool notAmplified = false)
		{
			this.affectiveUnit = affectiveUnit;
			this.type = type;
			
			if (amount < 0f)
				throw new ConstraintException("Effect amount should be larger than zero");
			
			this.notAmplified = notAmplified;
		}

		public void Apply(Unit unit, float multiplier)
		{
			if (!unit.GetComponent(this.affectiveUnit.ToString("G")))
				throw new InvalidOperationException("Effect unit type mismatch: Expected " + this.affectiveUnit);
			
			Debugger.Log(amount.ToString() + " " + multiplier.ToString());

			float totAmount;
			// Recalculate amount with multiplier
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
			  case EffectType.IncBlock:
				  unit.GetComponent<Health>().BlockAlter(totAmount);
				  break;
			  case EffectType.DecBlock:
				  unit.GetComponent<Health>().BlockAlter(-totAmount);
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