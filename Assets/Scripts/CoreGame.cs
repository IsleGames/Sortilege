using System;
using UnityEngine;
using Managers;
using Units;

public class CoreGame : MonoBehaviour
{
    public static CoreGame Ctx;

    public static CardManager CardOperator;
    public static BattleManager BattleOperator;

    public static Player Player;

    private void Start()
    {
        Ctx = this;
        
        CardOperator = new CardManager();
        BattleOperator = new BattleManager();

        Player = GameObject.Find("Player").GetComponent<Player>();
    }
}