using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Effect = Effects.Effect;

namespace Units
{

	public abstract class Unit : MonoBehaviour
	{
		public abstract void StartTurn();
		public abstract void EndTurn();

	}
}