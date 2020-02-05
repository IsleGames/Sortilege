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

    private void Start()
    {
        Ctx = this;
        
        CardOperator = GetComponent<CardManager>();
        BattleOperator = GetComponent<BattleManager>();

        Player = gameObject.transform.GetComponentInChildren<Player>();
        Enemy = gameObject.transform.GetComponentInChildren<Enemy>();
        
        Continue();
    }
    
    public bool IsBattleEnded()
    {
        return Mathf.Approximately(Game.Ctx.Player.GetComponent<Health>().health, 0) ||
               Mathf.Approximately(Game.Ctx.Enemy.GetComponent<Health>().health, 0);
    }
    
    public void Continue()
    {
        Debugger.Log(BattleOperator);
        IEnumerator enumerator = BattleOperator.Continue();
    }
}