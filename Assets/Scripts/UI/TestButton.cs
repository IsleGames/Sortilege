using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Cards;
using Effects;

[RequireComponent(typeof(Button))]
public class TestButton : MonoBehaviour
{


    public void SpawnCard() { 

        var effect = new Effects.Effect(Effects.UnitType.Enemy, 10);
        CardFactory.MakeCard("Test", StrategyType.Berserker, AttributeType.Infernal, new List<Effect>() { effect });
    }

}
