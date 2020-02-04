namespace Units
{
	public class Player : Unit
	{
		public int chainStreak;
		void Start()
		{
		}

		public void NewTurn()
		{
			chainStreak = 0;
		}
	}
}