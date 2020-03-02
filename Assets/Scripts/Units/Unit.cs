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
		public bool beingDamagedSomewhere = false;
		public bool isUnitFlinched = false;
		
		public UnityEvent onTurnBegin = new UnityEvent();
		public UnityEvent onAttack = new UnityEvent();
		public UnityEvent onDamage = new UnityEvent();
		public UnityEvent onTurnEnd = new UnityEvent();
		public UnityEvent onDead = new UnityEvent();
		
		public UnityEvent onHealthChange = new UnityEvent();
		public virtual void Initialize()
		{
			GetComponent<Health>().Initialize();
		}
				
		public abstract void StartTurn();
		// public abstract void EndTurn();
	}
}