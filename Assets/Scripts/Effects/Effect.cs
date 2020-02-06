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
		Player,
		Enemy,
		Unknown
	}

	public class Effect : MonoBehaviour
	{
		public UnitType AffectiveUnit { get; protected set; }
		public float Diff { get; protected set; }

		public Effect(UnitType affectiveUnit, float diff)
		{

			AffectiveUnit = affectiveUnit;

			// Positive Number for Damage, Negative Number for Heal
			Diff = diff;
			
			Debugger.Log("hi");
		}

		public void Apply(GameObject unit)
		{
			if (!unit.GetComponent(this.AffectiveUnit.ToString("G")))
				throw new InvalidOperationException("Effect unit type mismatch: Expected " + this.AffectiveUnit);

			unit.GetComponent<Health>().Damage(Diff);
		}

	}
}