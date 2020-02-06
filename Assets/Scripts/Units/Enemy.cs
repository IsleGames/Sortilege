using _Editor;

namespace Units
{
	public class Enemy : Unit
	{
		public override void Initialize()
		{
			Game.Ctx.Continue();
		}

		public override void Play()
		{
			// Debugger.Log("hi");
			Game.Ctx.Player.GetComponent<Health>().Damage(2f);
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