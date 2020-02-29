using UnityEngine.Events;

using _Editor;
using Library;

namespace Units
{
	public class Enemy : Unit
	{
		public override void StartTurn()
		{
			if (Game.Ctx.activeUnit != this)
			{
				return;
			}
			
			onTurnBegin.Invoke();

			EndTurn();
		}
		
		public override void EndTurn()
		{
			if (Game.Ctx.activeUnit != this)
			{
				return;
			}
			
			// onAttack Event goes here
            isUnitFlinched = false;
            onAttack.Invoke();
            if (!isUnitFlinched) 
				Game.Ctx.player.GetComponent<Health>().Damage(6f);
            else
            {
	            // Some effect
            }
            
			onTurnEnd.Invoke();
			
            Game.Ctx.AnimationOperator.PushAction(Utilities.WaitForSecs(0.8f), true);
			
			if (Game.Ctx.IsBattleEnded()) Game.Ctx.EndGame();
			
			beingDamagedSomewhere = false;
			Game.Ctx.Continue();
		}
	}
}