using System;
using _Editor;
using Cards;
using Managers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Units
{
	public class Player : Unit
	{
		public int drawCount = 2;
		
		void Start()
		{
		}

		public override void StartTurn()
		{
			Game.Ctx.CardOperator.StartTurn();
		}
		
		public override void EndTurn()
		{
			// Something something coroutine + ienum
			
			Game.Ctx.CardOperator.Apply(Game.Ctx.Enemy);

			if (Game.Ctx.RunningMethod == Game.Ctx.Player.StartTurn)
				Game.Ctx.Continue();
			else
				throw new InvalidOperationException("Ending player's turn in non-player round");
		}
	}
}