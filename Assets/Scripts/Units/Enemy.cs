using UnityEngine.Events;

using _Editor;

namespace Units
{
	public class Enemy : Unit
	{
		public override void StartTurn()
		{
			Game.Ctx.player.GetComponent<Health>().Damage(6f);
			if (Game.Ctx.IsBattleEnded()) Game.Ctx.EndGame();
			
			EndTurn();
		}
		
		public override void EndTurn()
		{
			Game.Ctx.Continue();
		}
	}
}