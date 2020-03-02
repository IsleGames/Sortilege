using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

using _Editor;
using Managers;
using TMPro;
using Units;
using Units.Enemies;
using Object = UnityEngine.Object;

// ReSharper disable InconsistentNaming

public class Game : MonoBehaviour
{
    public static Game Ctx;

    public GameObject UICanvas;

    public CardManager CardOperator;
    public VfxManager VfxOperator;
    public EnemyManager EnemyOperator;
    public AnimationManager AnimationOperator;

    public Player player;

    public bool isTutorial;
    public bool fixRandomSeed;
    
    public int turnCount;
    public Unit activeUnit;
    
    public bool inSelectEnemyMode;

    public delegate void DelegateMethod();

    public DelegateMethod RunningMethod;
        
    public IEnumerator BattleSeq;


    private void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Physics.queriesHitTriggers = true;
        
        Ctx = this;
    }

    private void Start()
    {
        if (fixRandomSeed) Random.InitState(42);
        
        UICanvas = GameObject.Find("UICanvas");

        CardOperator = GetComponent<CardManager>();
        VfxOperator = GetComponent<VfxManager>();
        EnemyOperator = GetComponent<EnemyManager>();
        AnimationOperator = GetComponent<AnimationManager>();

        player = transform.GetComponentInChildren<Player>();
        player.Initialize();
        
        // enemy = isTutorial ? transform.GetComponentInChildren<Avocado>() : transform.GetComponentInChildren<Enemy>();
        // EnemyOperator.Initialize();

        turnCount = 0;
        inSelectEnemyMode = false;

        BattleSeq = NextStep();

        StartCoroutine(ContinueAfterLoadScene());
    }
    
    private IEnumerator ContinueAfterLoadScene()
    {
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();
        
        CardOperator.pileDeck.AdjustAllPositions();

        Continue();
    }
    
    private IEnumerator NextStep()
    {
        while (true)
        {
            turnCount += 1;
            
            // Debugger.Warning("player play");
            VfxOperator.ShowTurnText("Player Turn");
            
            activeUnit = player;
            RunningMethod = activeUnit.StartTurn;
            yield return null;
            
            // Debugger.Warning("enemy play");
            VfxOperator.ShowTurnText("Enemy Turn");
            
            EnemyOperator.InitEnemy();
            activeUnit = EnemyOperator.GetNextEnemy();

            while (activeUnit != null)
            {
                RunningMethod = activeUnit.StartTurn;
                yield return null;
                
                activeUnit = EnemyOperator.GetNextEnemy();
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
        AnimationOperator.PushAction(StartNextTurn());
    }

    private IEnumerator StartNextTurn()
    {
        // Wait a frame so everything remains in the queue is popped out
        yield return new WaitForEndOfFrame();
        
        AnimationOperator.onAnimationEnd.Invoke();
        
        BattleSeq.MoveNext();

        AnimationOperator.PushAction(ActivateNextTurn());
        yield return null;
    }

    private IEnumerator ActivateNextTurn()
    {
        AnimationOperator.onAnimationEnd.Invoke();
        
        RunningMethod();
        yield return null;
    }

    public bool IsBattleEnded()
    {
        return player.GetComponent<Health>().IsDead() || EnemyOperator.IsAllEnemyDead();
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
                VfxOperator.ShowTurnText("Battle Complete");
                
// #if UNITY_EDITOR
//                 UnityEditor.EditorApplication.isPlaying = false;
// #else
//                 Application.Quit();
// #endif

            }
            else
            {
                Debugger.Log("player lost");
                VfxOperator.ShowTurnText("Battle Lost");
                if (isTutorial)
                {
                    transform.GetComponentInChildren<TextMeshPro>().text = "oops.. You died.. Let's do it again.";
                }
            }
        }
    }
}