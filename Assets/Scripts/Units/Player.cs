using System;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

using _Editor;
using Cards;
using Managers;

namespace Units
{
	public class Player : Unit
	{
		public int drawCount = 2;
		
		public UnityEvent onDraw = new UnityEvent();
		
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
			
			Game.Ctx.CardOperator.Apply(Game.Ctx.enemy);

			if (Game.Ctx.RunningMethod == Game.Ctx.player.StartTurn)
				Game.Ctx.Continue();
			else
				throw new InvalidOperationException("Ending player's turn in non-player round");
		}
	}
}