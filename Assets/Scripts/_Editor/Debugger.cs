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
                    Debug.Log(obj.ToString());
                else
                    Debug.Log(obj.ToString(), source);
        }
        
        public static void Warning<T>(T obj, Object source = null)
        {
            if (EnableDebugOutput)
                if (source == null)
                    Debug.LogWarning(obj.ToString());
                else
                    Debug.LogWarning(obj.ToString(), source);
        }
        
        public static void OneOnOneStat()
        {
            Debug.Log("player hp: " + Game.Ctx.Player.GetComponent<Health>().HitPoints);
            Debug.Log("enemy hp: " + Game.Ctx.Enemy.GetComponent<Health>().HitPoints);
        }
    }
}