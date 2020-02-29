using UnityEngine.Events;

using _Editor;
using Library;

namespace Units
{
	public class Enemy : Unit
	{
		public override void StartTurn()
		{
			onTurnBegin.Invoke();
			
			// onAttack Event goes here
            isUnitFlinched = false;
            onAttack.Invoke();
            if (!isUnitFlinched) 
				Game.Ctx.player.GetComponent<Health>().Damage(6f);
            else
            {
	            // Some effect
            }
			
			EndTurn();
		}
		
		public override void EndTurn()
		{
			onTurnEnd.Invoke();
			
            Game.Ctx.AnimationOperator.PushAnimation(Utilities.WaitForSecs(0.8f), true);
			
			if (Game.Ctx.IsBattleEnded()) Game.Ctx.EndGame();
			
			beingDamagedSomewhere = false;
			Game.Ctx.Continue();
		}
	}
}