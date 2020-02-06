using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Effect = Effects.Effect;

namespace Units
{

	public abstract class Unit : MonoBehaviour
	{
		public bool initialized = false;

		public abstract void Initialize();
		public abstract void Play();
		public abstract void EndTurn();

	}
}