using System;
using System.Data;
using Effects;
using Managers;
using Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Buffs
{
    public class Buff : MonoBehaviour
    {
	    public BuffType type;
	    public float amount;

	    public void Initialize(BuffType buffType, float buffAmount)
	    {
		    type = buffType;
		    amount = buffAmount;
		    
			switch (type)
			{
				// Bind them to individual functions
				case BuffType.Block:
					GetComponentInParent<Unit>().onDamage.AddListener(Block);
					break;
				case BuffType.Forge:
					break;
				case BuffType.Thorns:
					break;
				case BuffType.Plague:
					break;
				case BuffType.Flinch:
					break;
				case BuffType.Voodoo:
					break;
				case BuffType.Breeze:
					break;
			}
	    }
	    
	    // Todo: Check if an instance of self already exists

	    // OnDamage Effect
	    private void Block()
	    {
		    GetComponentInParent<Health>().onGoingEffectAmount = 0;
		    amount -= 1;

		    if (Mathf.Approximately(amount, 0f))
		    {
			    GetComponentInParent<Unit>().onDamage.RemoveListener(Block);
			    GetComponentInParent<BuffManager>().Destroy(this);
		    }
	    }
	    
	    // OnTurnBegin Effect
	    private void Forge()
	    {
	    }
    }
}