using System;
using System.Xml.Linq;
using UnityEngine;

namespace Library
{    
     public class Constant: MonoBehaviour
     {
         public int testVar;
         private void Awake()
         {
             testVar = 2;
         }
     }

}