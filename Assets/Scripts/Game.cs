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

public enum SceneType : int
{
	Battle,
    AfterBattleReward
}

public class Game : MonoBehaviour
{
    public static Game Ctx;

    public GameObject UICanvas;

    public CardManager CardOperator;
    public VfxManager VfxOperator;
    public EnemyManager EnemyOperator;
    public AnimationManager AnimationOperator;
    public SortOrderManager SortOrderOperator;
    
    public BattleManager BattleOperator;
    public AfterBattleRewardManager AfterBattleRewardOperator;

    public SceneType sceneType;
    
    public bool isTutorial;
    public bool fixRandomSeed;

    public delegate void DelegateMethod();

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
        SortOrderOperator = GetComponent<SortOrderManager>();

        BattleOperator = GetComponent<BattleManager>();
        AfterBattleRewardOperator = GetComponent<AfterBattleRewardManager>();

        switch (sceneType)
        {
            case SceneType.Battle:
                StartCoroutine(BattleOperator.ContinueAfterLoadScene());
                break;
            case SceneType.AfterBattleReward:
                StartCoroutine(AfterBattleRewardOperator.ContinueAfterLoadScene());
                break;
        }
    }

}