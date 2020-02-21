using System;
using System.Collections;
using System.Collections.Generic;
using _Editor;
using UnityEngine;
using Managers;
using Units;
using Random = System.Random;

// ReSharper disable InconsistentNaming

public class Game : MonoBehaviour
{
    public static Game Ctx;

    public CardManager CardOperator;
    public VfxManager VfxOperator;

    public Player Player;
    public Enemy Enemy;

    public int turnCount;

    public delegate void RoutineMethod();

    public RoutineMethod RunningMethod;
        
    public IEnumerator BattleSeq;

    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        UnityEngine.Random.InitState(42);
        
        Ctx = this;

        Player = transform.GetComponentInChildren<Player>();
        Enemy = transform.GetComponentInChildren<Enemy>();

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
            RunningMethod = Player.StartTurn;
            yield return null;
            
            Debugger.Log("enemy play");
            RunningMethod = Enemy.StartTurn;
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
        return Player.GetComponent<Health>().IsDead() || Enemy.GetComponent<Health>().IsDead();
    }
    
    public bool HasPlayerLost()
    {
        return Player.GetComponent<Health>().IsDead();
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