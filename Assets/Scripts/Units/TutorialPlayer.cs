using System;

namespace Units
{
	public class TutorialPlayer : Player
	{
		public override void StartTurn()
		{
			Game.Ctx.CardOperator.StartTurn();
			if (Game.Ctx.turnCount == 4)
				Game.Ctx.CardOperator.DrawCards(2, true, false);
			
			onTurnBegin.Invoke();
		}

		public override void EndTurn()
		{
			// Something something coroutine + ienum
			Game.Ctx.CardOperator.Apply(Game.Ctx.enemy);
			
			onTurnEnd.Invoke();

			beingDamagedSomewhere = false;
			if (Game.Ctx.activeUnit == this)
				Game.Ctx.Continue();
			else
				throw new InvalidOperationException("Ending player's turn in non-player round");
		}
	}
}