using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;
using Effects;

using _Editor;

namespace Cards
{
    // public enum CardStatus : int
    // {
    //     Unknown,
    //     Stored,
    //     Decked,
    //     Held,
    //     Discarded,
    // }

    public class Card : MonoBehaviour
    {
        public void Apply(Enemy enemy)
        {
            enemy.GetComponent<Health>().Damage(2f);
            Debugger.OneOnOneStat();
            if (Game.Ctx.IsBattleEnded()) Game.Ctx.EndGame();
        }
    }
}