using System;
using System.Collections;
using System.Data;
using _Editor;
using Effects;
using Library;
using Managers;
using Units;

// using UI;
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
					GetComponentInParent<Unit>().onDamage.AddListener(Block);
					break;
				case BuffType.Forge:
					GetComponentInParent<Unit>().onTurnBegin.AddListener(Forge);
					break;
				case BuffType.Thorns:
					GetComponentInParent<Unit>().onDamage.AddListener(Thorns);
					break;
				case BuffType.Plague:
					GetComponentInParent<Unit>().onTurnBegin.AddListener(Plague);
					break;
				case BuffType.Flinch:
					GetComponentInParent<Unit>().onAttack.AddListener(Flinch);
					break;
				case BuffType.Voodoo:
					GetComponentInParent<Unit>().onTurnBegin.AddListener(Voodoo);
					break;
				case BuffType.Breeze:
					throw new NotImplementedException();
					// break;
			}
	    }
	    

	    // OnDamage Effect
	    private void Block()
	    {
		    GetComponentInParent<Health>().onGoingEffectAmount = 0;
		    amount -= 1;

		    if (Mathf.Approximately(amount, 0f))
		    {
			    GetComponentInParent<Unit>().onDamage.RemoveListener(Block);
				Game.Ctx.AnimationOperator.PushAction(DestroyAfterAnimation(),true);
		    }
	    }
	    
	    // OnTurnBegin Effect
	    private void Forge()
	    {
		    Game.Ctx.CardOperator.DrawCards((int)(amount + .5f));
		    
		    // You may go away without RemoveListener though
		    GetComponentInParent<Unit>().onTurnBegin.RemoveListener(Forge);
            Game.Ctx.AnimationOperator.PushAction(DestroyAfterAnimation(),true);
	    }
	    
	    // OnDamage Effect
	    private void Thorns()
	    {
		    // yield return new WaitForEndOfFrame();
		    
		    Game.Ctx.BattleOperator.activeUnit.GetComponent<Health>().Damage(amount);
		    
		    GetComponentInParent<Unit>().onDamage.RemoveListener(Thorns);
            Game.Ctx.AnimationOperator.PushAction(DestroyAfterAnimation(),true);
	    }
	    
	    // OnTurnEnd Effect
	    private void Plague()
	    {
		    GetComponentInParent<Health>().Damage(1);
		    amount -= 1;

		    Debugger.Log(GetComponentInParent<Health>().hitPoints);
		    if (GetComponentInParent<Health>().IsDead()) return;

		    if (Mathf.Approximately(amount, 0f))
		    {
			    // Debug.Log("removed");
				GetComponentInParent<Unit>().onTurnEnd.RemoveListener(Plague);
				Game.Ctx.AnimationOperator.PushAction(DestroyAfterAnimation(),true);
		    }
		    
	    }
	    
	    // OnAttack Effect
	    private void Flinch()
	    {
		    GetComponentInParent<Unit>().isUnitFlinched = true;
		    amount -= 1;

		    if (Mathf.Approximately(amount, 0f))
		    {
				GetComponentInParent<Unit>().onAttack.RemoveListener(Flinch);
				Game.Ctx.AnimationOperator.PushAction(DestroyAfterAnimation(),true);
		    }
	    }
	    
	    // OnTurnBegin Effect
	    private void Voodoo()
	    {
		    if (!GetComponentInParent<Unit>().beingDamagedSomewhere)
				GetComponentInParent<Health>().Heal(amount);

			GetComponentInParent<Unit>().onTurnBegin.RemoveListener(Voodoo);
			
            Game.Ctx.AnimationOperator.PushAction(DestroyAfterAnimation(),true);
	    }

	    private IEnumerator DestroyAfterAnimation()
	    {
		    Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
		    
		    GetComponentInParent<BuffManager>().Destroy(this);
		    yield return null;
	    }
    }
}