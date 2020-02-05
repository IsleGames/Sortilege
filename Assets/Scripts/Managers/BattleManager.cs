using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Managers
{
    public class BattleManager : MonoBehaviour
    {
        public BattleManager()
        {
            
        }

        public bool IsBattleEnded()
        {
            return Mathf.Approximately(Game.Ctx.Player.GetComponent<Health>().health, 0) ||
                   Mathf.Approximately(Game.Ctx.Enemy.GetComponent<Health>().health, 0);
        }

        // Ref: https://stackoverflow.com/questions/12932306/how-does-startcoroutine-yield-return-pattern-really-work-in-unity
        // And a bunch of notes from CS376
        public IEnumerator Continue()
        {
            Game.Ctx.Player.Initialize();
            yield return null;
            
            Game.Ctx.Enemy.Initialize();
            yield return null;
            
            while (true)
            {
                // Game.Ctx.Player;
                yield return null;
                
                // Game.Ctx.Enemy;
                yield return null;
            }
        }
    }
}