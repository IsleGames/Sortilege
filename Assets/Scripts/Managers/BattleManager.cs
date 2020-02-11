using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using _Editor;
using Units;
using UnityEngine;

namespace Managers
{
    public class BattleManager : MonoBehaviour
    {
        public bool IsBattleEnded()
        {
            return Mathf.Approximately(Game.Ctx.Player.GetComponent<Health>().health, 0) ||
                   Mathf.Approximately(Game.Ctx.Enemy.GetComponent<Health>().health, 0);
        }

        // Ref: https://stackoverflow.com/questions/12932306/how-does-startcoroutine-yield-return-pattern-really-work-in-unity
        // And a bunch of notes from CS376
        public IEnumerator Continue()
        {
            Debugger.Log("player init");
            Game.Ctx.RunningMethod = Game.Ctx.Player.Initialize;
            yield return null;
            
            Debugger.Log("deck init");
            Game.Ctx.RunningMethod = Game.Ctx.CardOperator.Initialize;
            yield return null;
            
            Debugger.Log("enemy init");
            Game.Ctx.RunningMethod = Game.Ctx.Enemy.Initialize;
            yield return null;
            
            while (true)
            {
                Game.Ctx.turnCount += 1;
                
                Debugger.Log("player play");
                Game.Ctx.RunningMethod = Game.Ctx.Player.Play;
                yield return null;
                
                Debugger.Log("enemy play");
                Game.Ctx.RunningMethod = Game.Ctx.Enemy.Play;
                yield return null;
            }
        }
    }
}