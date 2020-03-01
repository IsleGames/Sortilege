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
		public bool waitingForAction;

		protected void Start()
		{
			waitingForAction = false;
		}
		

		public override void StartTurn()
		{
			if (Game.Ctx.activeUnit != this)
			{
				return;
			}
			
			onTurnBegin.Invoke();
			Game.Ctx.CardOperator.StartTurn();

			waitingForAction = true;
		}

		
		public void TryEndTurn()
		{
			if (waitingForAction)
				EndTurn();
			else
			{
				Debugger.Log("Nope, you can't end your turn now");
				return;
			}
		}
		
		public override void EndTurn()
		{
			waitingForAction = false;
			
			// onAttack Event goes here
            isUnitFlinched = false;
            onAttack.Invoke();
            if (!isUnitFlinched) 
				Game.Ctx.CardOperator.Apply(Game.Ctx.EnemyOperator.EnemyList[0]);
            else
            {
	            // Some effect
            }
			
			onTurnEnd.Invoke();
			Game.Ctx.CardOperator.EndTurn();

			beingDamagedSomewhere = false;
			if (Game.Ctx.activeUnit == this)
				Game.Ctx.Continue();
			else
				throw new InvalidOperationException("Ending player's turn in non-player round");
		}
	}
}