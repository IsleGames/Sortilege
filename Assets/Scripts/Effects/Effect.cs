using System;
using Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Effects
{
	public enum UnitType : int
	{
		Player,
		Enemy,
		Unknown
	}

	public class Effect : Object
	{

		public UnitType AffectiveUnit { get; protected set; }
		public float Diff { get; protected set; }

		public Effect(UnitType affectiveUnit, float diff)
		{

			AffectiveUnit = affectiveUnit;

			// Positive Number for Damage, Negative Number for Heal
			Diff = diff;

		}

		public void Apply(GameObject unit)
		{
			if (!unit.GetComponent(this.AffectiveUnit.ToString("G")))
				throw new InvalidOperationException("Effect unit type mismatch: Expected " + this.AffectiveUnit);

			unit.GetComponent<Health>().ChangeHealth(Diff);
		}

	}
}