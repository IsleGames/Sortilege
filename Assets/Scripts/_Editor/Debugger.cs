using UnityEngine;
using System.Collections;
using Units;
using Object = UnityEngine.Object;

namespace _Editor
{
    public class Debugger : MonoBehaviour
    {
        public static bool EnableDebugOutput = true;

        public static void Log<T>(T obj, Object source = null)
        {
            if (EnableDebugOutput)
                if (source == null)
                    Debug.Log(Time.time + " " + obj);
                else
                    Debug.Log(Time.time + " " + obj, source);
        }
        
        public static void Warning<T>(T obj, Object source = null)
        {
            if (EnableDebugOutput)
                if (source == null)
                    Debug.LogWarning(obj.ToString());
                else
                    Debug.LogWarning(obj.ToString(), source);
        }
        
        // public static void OneOnOneStat()
        // {
        //     Debug.Log("player hp: " + Game.Ctx.player.GetComponent<Health>().hitPoints);
        //     Debug.Log("enemy hp: " + Game.Ctx.enemy.GetComponent<Health>().hitPoints);
        // }
    }
}