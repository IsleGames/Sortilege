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
			
			Game.Ctx.player.GetComponent<Health>().Damage(6f);
			if (Game.Ctx.IsBattleEnded()) Game.Ctx.EndGame();
			
			EndTurn();
		}
		
		public override void EndTurn()
		{
			onTurnEnd.Invoke();
			
            Game.Ctx.AnimationOperator.PushAnimation(Utilities.WaitForSecs(0.8f), true);
			
			beingDamagedSomewhere = false;
			Game.Ctx.Continue();
		}
	}
}