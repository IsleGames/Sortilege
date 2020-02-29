using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using _Editor;

namespace Managers
{
    public class AnimationManager : MonoBehaviour
    {
		public UnityEvent onAnimationEnd = new UnityEvent();
        
        // All IEnumerators should call
        //
        //     Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
        //
        // before the last yield return null;

        public List<IEnumerator> EventQueue;
        public List<bool> StopQueue;

        [SerializeField]
        private int runningAnimationCount = 0;
        [SerializeField]
        private bool stoppingTillDone = false;

        private void Awake()
        {
            EventQueue = new List<IEnumerator>();
            StopQueue = new List<bool>();
        }

        private void Start()
        {
            onAnimationEnd.AddListener(OnIEnumRunningEnd);
        }

        public void PushAction(IEnumerator move, bool stopTillDone = false)
        {
            EventQueue.Add(move);
            StopQueue.Add(stopTillDone);
            
            TryRunEverything();
        }
        
        private void TryRunEverything() 
        {
            if (stoppingTillDone) return;
            RunEverything();
        }

        private void RunEverything()
        {
            if (EventQueue.Count == 0) return;
            
            stoppingTillDone = PopNextEvent();
            while (EventQueue.Count > 0 && !stoppingTillDone)
            {
                stoppingTillDone = PopNextEvent();
            } 
        }

        private void OnIEnumRunningEnd()
        {
            if (runningAnimationCount > 0)
            {
                runningAnimationCount -= 1;
                if (runningAnimationCount == 0)
                {
                    stoppingTillDone = false;
                    RunEverything();
                }
            }
            else
                throw new InvalidOperationException("OnIEnumRunningEnd called when remaining Event is empty");
        }

        private bool PopNextEvent()
        {
            runningAnimationCount += 1;
            
            IEnumerator move = EventQueue[0];
            bool ret = StopQueue[0];
            
            EventQueue.RemoveAt(0);
            StopQueue.RemoveAt(0);
            
            StartCoroutine(move);

            return ret;
        }
    }
}