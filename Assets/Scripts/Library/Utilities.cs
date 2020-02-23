using System;
using System.Collections;
using System.Collections.Generic;
using _Editor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Library
{
	public static class Utilities
	{
		public static T Draw<T>(this IList<T> list)  
		{
			int k = Random.Range(0, list.Count);

			T ret = list[k];
			list.RemoveAt(k);

			return ret;
		}  

		// Source: https://stackoverflow.com/questions/273313/randomize-a-listt
		public static void Shuffle<T>(this IList<T> list)  
		{  
			int n = list.Count;  

			while (n > 1) {  
				int k = Random.Range(0, n);
				n--;  

				T value = list[k];  
				list[k] = list[n];  
				list[n] = value;  
			}
		}
		public static IEnumerator MoveTo(GameObject obj, Vector3 pos, float time)
        {
            // Todo: P-Controller
            
            Vector3 init = obj.transform.position;
            float elapsed = 0;
            while (elapsed < time)
            {
                float t = elapsed / time;
                
                Vector3 current = pos * t + init * (1 - t);
                obj.transform.position = current;
                
                elapsed += Time.deltaTime;
                yield return null;
            }

            obj.transform.position = pos;
            
            Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            yield return null;
        }
		
		public static IEnumerator MoveAndScaleTo(GameObject obj, Vector3 targetPos, Vector3 targetScale, float time)
        {
            // Todo: P-Controller
            
            Vector3 initPos = obj.transform.position;
            Vector3 initScale = obj.transform.localScale;
            
            // Debugger.Log(obj.name + " " + targetPos);
            
            float elapsed = 0;
            while (elapsed < time)
            {
                float t = elapsed / time;
                
                Vector3 curPos = targetPos * t + initPos * (1 - t);
                obj.transform.position = curPos;
                
                Vector3 curScale = targetScale * t + initScale * (1 - t);
                obj.transform.localScale = curScale;
                
                elapsed += Time.deltaTime;
                yield return null;
            }

            obj.transform.position = targetPos;
            obj.transform.localScale = targetScale;
            
            Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            yield return null;
        }
	}
}