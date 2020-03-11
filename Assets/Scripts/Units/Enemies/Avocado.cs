namespace Units.Enemies
{
	public class Avocado : Enemy
	{
		protected override void Attack()
		{
			switch (Game.Ctx.BattleOperator.turnCount)
			{
				case 1:
					GetComponent<Health>().AddBarrier(1);
					break;
				case 2:
					break;
				case 3:
					Game.Ctx.BattleOperator.player.GetComponent<Health>().Damage(1);
					break;
				case 4:
					Game.Ctx.BattleOperator.player.GetComponent<Health>().Damage(11111);
					break;
				case 5:
					Game.Ctx.BattleOperator.player.GetComponent<Health>().Damage(11111);
					GetComponent<Health>().AddBarrier(11);
					break;
				default:
					GetComponent<Health>().Heal(111);
					Game.Ctx.BattleOperator.player.GetComponent<Health>().Damage(11);
					break;
			}
		}
		
	}
}