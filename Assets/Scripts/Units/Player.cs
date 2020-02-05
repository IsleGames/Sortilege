namespace Units
{
	public class Player : Unit
	{
		public int chainStreak;
		void Start()
		{
		}

		public override void Initialize()
		{
			
		}

		public override void Play()
		{
			chainStreak = 0;
		}

		public void EndTurn()
		{
			Game.Ctx.Continue();
		}
	}
}