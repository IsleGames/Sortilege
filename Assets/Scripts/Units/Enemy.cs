using _Editor;

namespace Units
{
	public class Enemy : Unit
	{

		public override void StartTurn()
		{
			// Debugger.Log("hi");
			Game.Ctx.Player.GetComponent<Health>().Damage(6f);
			Debugger.OneOnOneStat();
			if (Game.Ctx.IsBattleEnded()) Game.Ctx.EndGame();
			
			EndTurn();
		}
		
		public override void EndTurn()
		{
			Game.Ctx.Continue();
		}
	}
}