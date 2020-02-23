using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Effect = Effects.Effect;

namespace Units
{

	public abstract class Unit : MonoBehaviour
	{
		public UnityEvent onDamage = new UnityEvent();
		
		public abstract void StartTurn();
		public abstract void EndTurn();
	}
}