using System;
using System.Collections.Generic;
using UnityEngine;
using Effect = Effects.Effect;

namespace Units
{

	public abstract class Unit : MonoBehaviour
	{
		public bool initialized = false;

		public Health health;

		// Calling the Effect list BuffList makes it clearer
		public List<Effect> buffList;

		private void Start()
		{
			buffList = new List<Effect>();
			health = gameObject.GetComponent<Health>();
		}

		private void LoadData()
		{
			throw new NotImplementedException();
		}
    
    
	}
}