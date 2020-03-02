using System;
using System.Collections;
using System.Data;
using _Editor;
using Effects;
using Library;
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
					// Todo: Check if an instance of self already exists
					GetComponentInParent<Unit>().onDamage.AddListener(
						delegate { StartCoroutine(Block()); }
						);
					break;
				case BuffType.Forge:
					GetComponentInParent<Unit>().onTurnBegin.AddListener(
						delegate { StartCoroutine(Forge()); }
						);
					break;
				case BuffType.Thorns:
					GetComponentInParent<Unit>().onDamage.AddListener(
						delegate { StartCoroutine(Thorns()); }
						);
					break;
				case BuffType.Plague:
					GetComponentInParent<Unit>().onTurnBegin.AddListener(
						delegate { StartCoroutine(Plague()); }
						);
					break;
				case BuffType.Flinch:
					GetComponentInParent<Unit>().onAttack.AddListener(
						delegate { StartCoroutine(Flinch()); }
						);
					break;
				case BuffType.Voodoo:
					GetComponentInParent<Unit>().onTurnBegin.AddListener(
						delegate { StartCoroutine(Voodoo()); }
						);
					break;
				case BuffType.Breeze:
					throw new NotImplementedException();
					// break;
			}
	    }
	    

	    // OnDamage Effect
	    private IEnumerator Block()
	    {
		    GetComponentInParent<Health>().onGoingEffectAmount = 0;
		    amount -= 1;

		    if (Mathf.Approximately(amount, 0f))
		    {
			    // GetComponentInParent<Unit>().onDamage.RemoveListener(Block);
				Game.Ctx.AnimationOperator.PushAction(DestroyAfterAnimation(),true);
		    }
		    yield return null;
	    }
	    
	    // OnTurnBegin Effect
	    private IEnumerator Forge()
	    {
		    Game.Ctx.CardOperator.DrawCards((int)(amount + .5f));
		    
		    // You may go away without RemoveListener though
		    // GetComponentInParent<Unit>().onTurnBegin.RemoveListener(Forge);
            Game.Ctx.AnimationOperator.PushAction(DestroyAfterAnimation(),true);
		    yield return null;
	    }
	    
	    // OnDamage Effect
	    private IEnumerator Thorns()
	    {
		    yield return new WaitForEndOfFrame();
		    
		    Game.Ctx.activeUnit.GetComponent<Health>().Damage(amount);
		    
		    // GetComponentInParent<Unit>().onDamage.RemoveListener(Thorns);
            Game.Ctx.AnimationOperator.PushAction(DestroyAfterAnimation(),true);
		    yield return null;
	    }
	    
	    // OnTurnEnd Effect
	    private IEnumerator Plague()
	    {
		    GetComponentInParent<Health>().Damage(1);
		    amount -= 1;

		    if (Mathf.Approximately(amount, 0f))
		    {
				// GetComponentInParent<Unit>().onTurnEnd.RemoveListener(Plague);
				Game.Ctx.AnimationOperator.PushAction(DestroyAfterAnimation(),true);
		    }
		    yield return null;
		    
	    }
	    
	    // OnAttack Effect
	    private IEnumerator Flinch()
	    {
		    GetComponentInParent<Unit>().isUnitFlinched = true;
		    amount -= 1;

		    if (Mathf.Approximately(amount, 0f))
		    {
				// GetComponentInParent<Unit>().onAttack.RemoveListener(Flinch);
				Game.Ctx.AnimationOperator.PushAction(DestroyAfterAnimation(),true);
		    }
		    yield return null;
	    }
	    
	    // OnTurnBegin Effect
	    private IEnumerator Voodoo()
	    {
		    if (!GetComponentInParent<Unit>().beingDamagedSomewhere)
				GetComponentInParent<Health>().Heal(amount);

			// GetComponentInParent<Unit>().onTurnBegin.RemoveListener(Voodoo);
			
            Game.Ctx.AnimationOperator.PushAction(DestroyAfterAnimation(),true);

            yield return null;
	    }

	    private IEnumerator DestroyAfterAnimation()
	    {
		    Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
		    
		    GetComponentInParent<BuffManager>().Destroy(this);
		    yield return null;
	    }
    }
}