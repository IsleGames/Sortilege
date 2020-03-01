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
        public bool stoppingTillDone = false;

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
            Debugger.Log("Now adding to queue: " + move + "");
            
            EventQueue.Add(move);
            StopQueue.Add(stopTillDone);
            
            TryRunEverything();
        }
        
        private void TryRunEverything() 
        {
            Debugger.Log("Try Run with stoppingTillDone as " + stoppingTillDone);
            
            if (stoppingTillDone) return;
            
            // Debugger.Log("Forced Running in try run");
            
            RunEverything();
        }

        private void RunEverything()
        {
            // Debugger.Log("Forced Running");
            
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
                Debugger.Log("OnIEnumRunningEnd activated with remaining count " + runningAnimationCount  + " - 1");
                
                runningAnimationCount -= 1;

                if (runningAnimationCount == 0)
                {
                    stoppingTillDone = false;
                    
                    // Debugger.Log("Forced Running on event ends");
            
                    RunEverything();
                }
            }
            else
                throw new InvalidOperationException("OnIEnumRunningEnd called when remaining Event is empty");
        }

        private bool PopNextEvent()
        {
            IEnumerator move = EventQueue[0];
            bool ret = StopQueue[0];
            
            Debugger.Log("Now running: " + move + " with remaining count " + runningAnimationCount + " + 1");
            
            runningAnimationCount += 1;
            
            EventQueue.RemoveAt(0);
            StopQueue.RemoveAt(0);
            
            StartCoroutine(move);

            return ret;
        }
    }
}