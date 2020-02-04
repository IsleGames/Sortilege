using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class BattleManager
    {
        public BattleManager()
        {
            
        }
        public IEnumerator NextStep()
        {
            while (true)
                yield return null;
        }
    }
}