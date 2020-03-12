using System.Collections.Generic;
using _Editor;
using Animations;
using Data;
using Effects;
using Library;
using TMPro;
// using UnityEditor.UI;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.UI;

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

		private int currentChoice;
		
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
            
            foreach (EnemyAbilityData moveData in enemyData.abilityList)
            {
	            Ability move = new Ability
	            {
		            title = moveData.title,
		            description = moveData.description,
		            effectList = new List<Effect>(moveData.effectList),
		            buffEffectList = new List<BuffEffect>(moveData.buffList)
	            };
	            abilityList.Add(move);
            }
            
            DisableDisplay();
			base.Initialize();

			onDead.AddListener(() => GetComponent<FadeOut>().BlockingFade());
            onDead.AddListener(() => Debug.Log("Died"));
		}

		public override void StartTurn()
		{
			if (Game.Ctx.BattleOperator.activeUnit != this)
			{
				return;
			}

			onTurnBegin.Invoke();
			Game.Ctx.VfxOperator.ChangeMultiplierText(false);

			if (!GetComponent<Health>().IsDead())
			{
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
			}

			EndTurn();
		}

		public void MakeChoiceAndDisplay()
		{
			Debugger.Log(gameObject.name + " displaying");
			// Randomize a skill
            // Todo: add enemyActionType

            if (abilityList.Count == 0) return;

            int choice = 0;
            for (int i = 0; i < 40; i++)
            {
	            choice = Random.Range(0, abilityList.Count);
	            if (abilityList[choice].activateTurnCount <= Game.Ctx.BattleOperator.turnCount) break;
            }

            transform.Find("Intent").GetComponent<TextMeshPro>().enabled = true;
            transform.Find("Intent").Find("IntentImage").GetComponent<Image>().enabled = true;
            transform.Find("Intent").GetComponent<TextMeshPro>().text = abilityList[choice].Info();

            currentChoice = choice;
		}

		public void DisableDisplay()
		{
            transform.Find("Intent").Find("IntentImage").GetComponent<Image>().enabled = false;
            transform.Find("Intent").GetComponent<TextMeshPro>().enabled = false;
		}

		protected virtual void Attack()
		{
			MakeChoiceAndDisplay();
            abilityList[currentChoice].ApplyAsEnemy(this);
            
			if (GetComponent<Health>().IsDead())
			{
				Game.Ctx.BattleOperator.CheckBattleEnd();
			}
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