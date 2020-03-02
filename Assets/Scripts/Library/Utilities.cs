using System;
using System.Collections;
using System.Collections.Generic;
using _Editor;
using Cards;
using Units;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
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

		public static T DrawNoShuffle<T>(this IList<T> list)
		{
			T ret = list[0];
			list.RemoveAt(0);
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
		
		public static IEnumerator RectTransMoveAndScaleTo(
			RectTransform rectTrans, SpriteRenderer sp, Vector2 targetSize, Vector3 targetPos, float k)
        {
	        Vector2 initSize = sp.size;
	        Vector3 initPos = rectTrans.anchoredPosition;
	        
	        // Debugger.Log(initPos + " " + initSize); 

            float p = 0;
            while (p < 1f - 5e-3)
            {
                p += (1 - p) * k;
                
                Vector2 curSize = targetSize * p + initSize * (1 - p);
                sp.size = curSize;
	                
                Vector3 curPos = targetPos * p + initPos * (1 - p);
                rectTrans.anchoredPosition = curPos;
                
                yield return null;
            }
            
	        sp.size = targetSize;
	        rectTrans.anchoredPosition = targetPos;
	        
            Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            yield return null;
        }
		
		public static IEnumerator MoveTo(GameObject obj, Vector3 pos, float k)
        {
            // Todo: P-Controller
            
            Vector3 init = obj.transform.position;

            float p = 0;
            while (p < 1f - 1e-3)
            {
                p += (1 - p) * k;
                
                Vector3 current = pos * k + init * (1 - k);
                obj.transform.position = current;
                
                yield return null;
            }

            obj.transform.position = pos;
            
            Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            yield return null;
        }
		
		public static IEnumerator MoveAndScaleCardTo(GameObject obj, Vector3 targetPos, Vector3 targetScale, float k)
        {
	        CardEvent cardEvent = obj.GetComponent<CardEvent>();
	        if (cardEvent) cardEvent.animationLock = true;
	        
	        Vector3 initPos = obj.transform.position;
            Vector3 initScale = obj.transform.localScale;

            float p = 0;
            while (p < 1f - 5e-3)
            {
                p += (1 - p) * k;
                
                Vector3 curPos = targetPos * p + initPos * (1 - p);
                obj.transform.position = curPos;
                
                Vector3 curScale = targetScale * p + initScale * (1 - p);
                obj.transform.localScale = curScale;
                
                yield return null;
            }

            obj.transform.position = targetPos;
            obj.transform.localScale = targetScale;

            if (cardEvent) cardEvent.animationLock = false;

	        Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            yield return null;
        }
		
		public static IEnumerator WaitForSecs(float time)
		{
			yield return new WaitForSeconds(time);
	
            Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            yield return null;
		}
		public static IEnumerator PlaceholderIEnum()
		{
            Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            yield return null;
		}
	}
}