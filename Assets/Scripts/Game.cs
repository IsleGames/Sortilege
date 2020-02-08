using System;
using System.Collections;
using System.Collections.Generic;
using _Editor;
using UnityEngine;
using Managers;
using Units;
// ReSharper disable InconsistentNaming

public class Game : MonoBehaviour
{
    public static Game Ctx;

    public CardManager CardOperator;
    public BattleManager BattleOperator;
    public VfxManager VfxOperator;

    public Player Player;
    public Enemy Enemy;

    public int turnCount;

    public delegate void RoutineMethod();

    public RoutineMethod RunningMethod;
        
    public IEnumerator BattleSeq;

    private void Start()
    {
        Ctx = this;

        Player = transform.GetComponentInChildren<Player>();
        Enemy = transform.GetComponentInChildren<Enemy>();

        BattleSeq = BattleOperator.Continue();

        turnCount = 0;

        Continue();
    }
    
    public bool IsBattleEnded()
    {
        return Player.GetComponent<Health>().IsDead() || Enemy.GetComponent<Health>().IsDead();
    }
    
    public bool HasPlayerLost()
    {
        return Player.GetComponent<Health>().IsDead();
    }
    
    public void Continue()
    {
        if (IsBattleEnded()) return;
            
        // The IEnumerator returns the next procedure back to Game.Ctx.RunningMethod
        // We don't use StartCoroutine since no separate thread(approx.) is required
        BattleSeq.MoveNext();
        
        // Since yield return cannot return a method, we let this method calls the next step
        // instead
        RunningMethod();
    }

    public void EndGame()
    {
        if (IsBattleEnded())
        {
            if (!HasPlayerLost())
            {
                Debugger.Log("player wins");
            }
            else
            {
                Debugger.Log("player lost");
            }
        }
    }
}