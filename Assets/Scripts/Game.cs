using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public delegate void OnGoingMethod();

    public OnGoingMethod onGoingMove;


    private void Start()
    {
        Ctx = this;
        
        CardOperator = new CardManager();
        BattleOperator = new BattleManager();

        Player = GameObject.Find("Player").GetComponent<Player>();
        Enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        
        Continue();
        
    }
    
    public bool IsBattleEnded()
    {
        return Mathf.Approximately(Game.Ctx.Player.GetComponent<Health>().health, 0) ||
               Mathf.Approximately(Game.Ctx.Enemy.GetComponent<Health>().health, 0);
    }
    
    public void Continue()
    {
        IEnumerator enumerator = BattleOperator.Continue();
    }
}