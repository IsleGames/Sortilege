using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Cards;
using Effects;

[RequireComponent(typeof(Button))]
public class TestButton : MonoBehaviour
{
    CardFactory factory;

    private void Start()
    {
        factory = FindObjectOfType<CardFactory>();
    }


    public void SpawnCard() { 

        var effect = new Effects.Effect(Effects.UnitType.Enemy, 10);
        factory.MakeCard("Test", StrategyType.Berserker, AttributeType.Infernal, new List<Effect>() { effect });
    }

}
