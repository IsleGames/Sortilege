using _Editor;

namespace Units
{
	public class Enemy : Unit
	{
		public override void Initialize()
		{
			// Pending: ask for HP from upper Manager instead of doing it here
			
			GetComponent<Health>().Initialize(8);
			Debugger.Log("Enemy HP: " + GetComponent<Health>().health);
			
			Game.Ctx.Continue();
		}

		public override void Play()
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