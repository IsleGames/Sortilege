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

			unit.GetComponent<Health>().Damage(damage);
		}

        public string Text()
        {
            if (damage > 0) return $"{damage}\nDMG";
            else return $"{-damage}\nHEAL";
        }



    }
}