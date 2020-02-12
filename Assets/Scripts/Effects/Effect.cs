using System;
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
		BlockUp,
		BlockDown
	}

	[Serializable]
	public class Effect
	{
		public UnitType affectiveUnit;
		public float damage;
		
		public Effect(UnitType affectiveUnit, float amount)
		{
			this.affectiveUnit = affectiveUnit;

			this.damage = amount;
		}

		public void Apply(Unit unit)
		{
			if (!unit.GetComponent(this.affectiveUnit.ToString("G")))
				throw new InvalidOperationException("Effect unit type mismatch: Expected " + this.affectiveUnit);

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

        public string Text()
        {
            if (damage > 0) return $"{damage}\nDMG";
            else return $"{-damage}\nHEAL";
        }



    }
}