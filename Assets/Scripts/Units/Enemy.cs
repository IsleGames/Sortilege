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
		}
	}
}