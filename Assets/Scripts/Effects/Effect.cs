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
		
		public Effect(UnitType affectiveUnit, EffectType effectType, float amount)
		{
			this.affectiveUnit = affectiveUnit;
			this.type = type;
			
			if (amount < 0f)
				throw new ConstraintException("Effect amount should be larger than zero");
			
			this.amount = amount;
		}

		public void Apply(Unit unit)
		{
			if (!unit.GetComponent(this.affectiveUnit.ToString("G")))
				throw new InvalidOperationException("Effect unit type mismatch: Expected " + this.affectiveUnit);
			
			Debugger.Log("I am applied!");
			
			switch (type)
			{
			  case EffectType.Damage: 
				  unit.GetComponent<Health>().Damage(amount);
				  break;
			  case EffectType.Heal:
				  unit.GetComponent<Health>().Heal(amount);
				  break;
			  case EffectType.IncBlock:
				  unit.GetComponent<Health>().BlockAlter(amount);
				  break;
			  case EffectType.DecBlock:
				  unit.GetComponent<Health>().BlockAlter(-amount);
				  break;
			  case EffectType.DamageIgnoreBlock: 
				  unit.GetComponent<Health>().Damage(amount, true);
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