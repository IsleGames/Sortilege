using System;
using System.Collections;
using System.Collections.Generic;

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
                
                var current = pos * t + init * (1 - t);
                obj.transform.position = current;
                
                elapsed += Time.deltaTime;
                yield return null;
            }

            Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            
            yield return null;
        }
	}
}