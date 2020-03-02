using System;
using System.Collections;
using _Editor;
using TMPro;
using UnityEngine;

using Units;

namespace Managers
{
    public class BattleManager : MonoBehaviour
    {
        public Player player;
    
        public int turnCount;
        public Unit activeUnit;
        
        [NonSerialized]
        public bool inSelectEnemyMode;
        
        public IEnumerator BattleSeq;

        public Game.DelegateMethod RunningMethod;

        public IEnumerator ContinueAfterLoadScene()
        {
            player = transform.GetComponentInChildren<Player>();
            player.Initialize();
            
            BattleSeq = NextStep();
            turnCount = 0;
            inSelectEnemyMode = false;
        
            // Wait a frame so every Awake and Start method is called
            yield return new WaitForEndOfFrame();
    
            Game.Ctx.CardOperator.pileDeck.AdjustAllPositions();
    
            Continue();
        }
        
        private IEnumerator NextStep()
        {
            while (true)
            {
                turnCount += 1;
                
                // Debugger.Warning("player play");
                Game.Ctx.VfxOperator.ShowTurnText("Player Turn");
                
                activeUnit = player;
                RunningMethod = activeUnit.StartTurn;
                yield return null;
                
                // Debugger.Warning("enemy play");
                Game.Ctx.VfxOperator.ShowTurnText("Enemy Turn");
                
                Game.Ctx.EnemyOperator.InitEnemy();
                activeUnit = Game.Ctx.EnemyOperator.GetNextEnemy();
    
                while (activeUnit != null)
                {
                    RunningMethod = activeUnit.StartTurn;
                    yield return null;
                    
                    activeUnit = Game.Ctx.EnemyOperator.GetNextEnemy();
                }
            }
        }
        
        public void Continue()
        {
            if (IsBattleEnded())
            {
                EndGame();
                return;
            }
            
            // Push StartNextTurn into the queue to make it run after all animations
            // And pause the system-level calculation till everything before is done
            Game.Ctx.AnimationOperator.PushAction(StartNextTurn());
        }
    
        private IEnumerator StartNextTurn()
        {
            // Wait a frame so everything remains in the queue is popped out
            yield return new WaitForEndOfFrame();
            
            Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            
            BattleSeq.MoveNext();
    
            Game.Ctx.AnimationOperator.PushAction(ActivateNextTurn());
            yield return null;
        }
    
        private IEnumerator ActivateNextTurn()
        {
            Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            
            RunningMethod();
            yield return null;
        }
    
        public bool IsBattleEnded()
        {
            return player.GetComponent<Health>().IsDead() || Game.Ctx.EnemyOperator.IsAllEnemyDead();
        }
        
        public bool HasPlayerLost()
        {
            return player.GetComponent<Health>().IsDead();
        }
    
        public void EndGame()
        {
            if (IsBattleEnded())
            {
                if (!HasPlayerLost())
                {
                    Debugger.Log("player wins");
                    Game.Ctx.VfxOperator.ShowTurnText("Battle Complete");
                    
    // #if UNITY_EDITOR
    //                 UnityEditor.EditorApplication.isPlaying = false;
    // #else
    //                 Application.Quit();
    // #endif
    
                }
                else
                {
                    Debugger.Log("player lost");
                    Game.Ctx.VfxOperator.ShowTurnText("Battle Lost");
                    if (Game.Ctx.isTutorial)
                    {
                        transform.GetComponentInChildren<TextMeshPro>().text = "oops.. You died.. Let's do it again.";
                    }
                }
            }
        }
    }
}