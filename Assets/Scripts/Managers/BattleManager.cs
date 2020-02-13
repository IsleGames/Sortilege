using System;
using System.Collections;
using System.Collections.Generic;
using _Editor;
using Units;
using UnityEngine;

namespace Managers
{
    public class BattleManager : MonoBehaviour
    {
        public bool IsBattleEnded()
        {
            return Mathf.Approximately(Game.Ctx.Player.GetComponent<Health>().HitPoints, 0) ||
                   Mathf.Approximately(Game.Ctx.Enemy.GetComponent<Health>().HitPoints, 0);
        }

    }
}