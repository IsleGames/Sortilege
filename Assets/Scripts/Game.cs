using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

using _Editor;
using Managers;
using Units;

// ReSharper disable InconsistentNaming

public class Game : MonoBehaviour
{
    public static Game Ctx;

    public CardManager CardOperator;
    public VfxManager VfxOperator;
    public AnimationManager AnimationOperator;

    public Player player;
    public Enemy enemy;

    public int turnCount;
    public Unit activeUnit;

    public delegate void RoutineMethod();

    public RoutineMethod RunningMethod;
        
    public IEnumerator BattleSeq;

    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        Random.InitState(42);
        Physics.queriesHitTriggers = true;
        
        Ctx = this;

        CardOperator = GetComponent<CardManager>();
        VfxOperator = GetComponent<VfxManager>();
        AnimationOperator = GetComponent<AnimationManager>();

        player = transform.GetComponentInChildren<Player>();
        enemy = transform.GetComponentInChildren<Enemy>();

        turnCount = 0;

        BattleSeq = NextStep();

        StartCoroutine(ContinueAfterLoadScene());
    }
    
    private IEnumerator ContinueAfterLoadScene()
    {
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();
        
        Continue();
    }
    
    private IEnumerator NextStep()
    {
        while (true)
        {
            turnCount += 1;
            
            Debugger.Log("player play");
            activeUnit = player;
            RunningMethod = player.StartTurn;
            yield return null;
            
            Debugger.Log("enemy play");
            activeUnit = enemy;
            RunningMethod = enemy.StartTurn;
            yield return null;
        }
    }
    
    public void Continue()
    {
        if (IsBattleEnded()) return;
        BattleSeq.MoveNext();
        RunningMethod();
    }
    
    public bool IsBattleEnded()
    {
        return player.GetComponent<Health>().IsDead() || enemy.GetComponent<Health>().IsDead();
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
                
// #if UNITY_EDITOR
//                 UnityEditor.EditorApplication.isPlaying = false;
// #else
//                 Application.Quit();
// #endif

            }
            else
            {
                Debugger.Log("player lost");
            }
        }
    }
}