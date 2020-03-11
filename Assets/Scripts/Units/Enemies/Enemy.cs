using System.Collections.Generic;
using Data;
using Effects;
using Library;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Units.Enemies
{
	public enum EnemyActionType : int
	{
		Sequential,
		Randomized
	}

	public class Enemy : Unit
	{
		public EnemyData enemyData;

		public string title;
		public string description;
		
        public List<Ability> abilityList;
		
		public void Initialize(EnemyData enemyData)
		{
			this.enemyData = enemyData;
            
            // For inspector visualization
            gameObject.name = enemyData.title;
            
            title = enemyData.title;
            description = enemyData.description;

            GetComponent<Health>().maximumHitPoints = enemyData.maximumHealth;
            GetComponent<EnemyRender>().imgSprite = enemyData.displayImage;
            onDead.AddListener(() => GetComponent<FadeOut>().BlockingFade());
            onDead.AddListener(() => Debug.Log("Died"));

            foreach (EnemyAbilityData moveData in enemyData.abilityList)
            {
	            Ability move = new Ability
	            {
		            effectList = new List<Effect>(moveData.effectList),
		            buffEffectList = new List<BuffEffect>(moveData.buffList)
	            };
	            abilityList.Add(move);
            }
            
			base.Initialize();
		}

		public override void StartTurn()
		{
			if (Game.Ctx.BattleOperator.activeUnit != this)
			{
				return;
			}

			onTurnBegin.Invoke();
			Game.Ctx.VfxOperator.ChangeMultiplierText(false);
			// onAttack Event goes here

            onAttack.Invoke();
            isUnitFlinched = false;
            if (!isUnitFlinched) 
            {
	            Attack();
            }
            else
            {
	            // Some effect
            }

			EndTurn();
		}

		private void Attack()
		{
			// Randomize a skill
            // Todo: add enemyActionType

            int choice = 0;
            for (int i = 0; i < 40; i++)
            {
	            choice = Random.Range(0, abilityList.Count);
	            if (abilityList[choice].activateTurnCount <= Game.Ctx.BattleOperator.turnCount) break;
            }

            abilityList[choice].ApplyAsEnemy(this);
		}
		
		public void EndTurn()
		{
			if (Game.Ctx.BattleOperator.activeUnit != this)
			{
				return;
			}

			onTurnEnd.Invoke();

			
            Game.Ctx.AnimationOperator.PushAction(Utilities.WaitForSecs(0.8f), true);

            beingDamagedSomewhere = false;
			Game.Ctx.BattleOperator.Continue();
		}
	}
}