using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Effect : Object
{

	public UnitType AffectiveUnit { get; protected set; }
	public float Shift { get; protected set; }

	public Effect(UnitType affectiveUnit, float shift)
	{

		AffectiveUnit = affectiveUnit;

		// Positive Number for Damage, Negative Number for Heal
		Shift = shift;

	}

	public void Apply(Unit unit)
	{

		if (unit.type != this.AffectiveUnit)
			throw new InvalidOperationException("Effect unit type mismatch: Expected " + this.AffectiveUnit);

		float expectedHitPoint = unit.hitPoint + this.Shift;
		
		if (this.Shift > 0) {
			if (expectedHitPoint < 0) expectedHitPoint = 0;
		}
		else if (this.Shift < 0) {
			if (expectedHitPoint > unit.maximumHitPoint) expectedHitPoint = unit.maximumHitPoint;
		}

		unit.hitPoint = expectedHitPoint;

	}

}