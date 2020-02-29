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
using Object = UnityEngine.Object;

// ReSharper disable InconsistentNaming

public class Game : MonoBehaviour
{
    public static Game Ctx;

    public GameObject UICanvas;

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

    public bool tutorial;

    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        Random.InitState(42);
        Physics.queriesHitTriggers = true;
        
        Ctx = this;

        UICanvas = GameObject.Find("UICanvas");

        CardOperator = GetComponent<CardManager>();
        VfxOperator = GetComponent<VfxManager>();
        AnimationOperator = GetComponent<AnimationManager>();

        player = !tutorial? transform.GetComponentInChildren<Player>() : transform.GetComponentInChildren<TutorialPlayer>();
        player.Initialize();
        enemy = tutorial ? transform.GetComponentInChildren<Avocado>() : transform.GetComponentInChildren<Enemy>();
        enemy.Initialize();

        turnCount = 0;

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
            
            Debugger.Log("player play");
            VfxOperator.ShowTurnText("Player Turn");
            
            activeUnit = player;
            RunningMethod = player.StartTurn;
            yield return null;
            
            Debugger.Log("enemy play");
            VfxOperator.ShowTurnText("Enemy Turn");
            
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
                if (tutorial)
                {
                    transform.GetComponentInChildren<TextMeshPro>().text = "oops.. You died.. Let's do it again.";
                }
            }
        }
    }
}