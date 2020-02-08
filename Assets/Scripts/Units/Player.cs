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
			// Load all existing cards from 
			Game.Ctx.Continue();
		}

		public override void Play()
		{
			chainStreak = 0;
		}

		public override void EndTurn()
		{
			Game.Ctx.Continue();
		}
	}
}