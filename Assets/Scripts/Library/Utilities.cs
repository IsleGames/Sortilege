using System;
using System.Collections.Generic;
using _Editor;
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

	}
}