using System.Threading;
using UnityEngine.Events;

using _Editor;
using Library;

namespace Units
{
	public class Avocado : Enemy
	{
		public override void StartTurn()
		{
			onTurnBegin.Invoke();
			switch (Game.Ctx.turnCount)
			{
				case 1:
					Game.Ctx.enemy.GetComponent<Health>().AddBarrier(1);
					break;
				case 2:
					break;
				case 3:
					Game.Ctx.player.GetComponent<Health>().Damage(1);
					break;
				case 4:
					Game.Ctx.player.GetComponent<Health>().Damage(11111);
					break;
				case 5:
					Game.Ctx.player.GetComponent<Health>().Damage(11111);
					Game.Ctx.enemy.GetComponent<Health>().AddBarrier(11);
					break;
				default:
					Game.Ctx.enemy.GetComponent<Health>().Heal(111);
					Game.Ctx.player.GetComponent<Health>().Damage(11);
					break;
				
			}
			
			if (Game.Ctx.IsBattleEnded()) Game.Ctx.EndGame();
			EndTurn();
		}
		
	}
}