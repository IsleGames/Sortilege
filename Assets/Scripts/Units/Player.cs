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
			if (Game.Ctx.BattleOperator.activeUnit != this)
			{
				return;
			}
			
			onTurnBegin.Invoke();
			Game.Ctx.CardOperator.StartTurn();

			if (GetComponent<Health>().IsDead())
			{
				Game.Ctx.BattleOperator.CheckBattleEnd();
				return;
			}
			waitingForAction = true;
		}

		public void EndTurn(Unit target)
		{
			waitingForAction = false;

            // onAttack Event goes here
            if (!isUnitFlinched)
            {
                onAttack.Invoke();
                Game.Ctx.CardOperator.Apply(target);
            }
            else
            {
                // Some effect
            }
			isUnitFlinched = false;

			onTurnEnd.Invoke();
			Game.Ctx.CardOperator.EndTurn();

			beingDamagedSomewhere = false;
			if (Game.Ctx.BattleOperator.activeUnit == this)
				Game.Ctx.BattleOperator.Continue();
			else
				throw new InvalidOperationException("Ending player's turn in non-player round");
		}
	}
}