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

        public Queue<(IEnumerator, bool)> EventQueue;
        public List<bool> StopQueue;

        [SerializeField]
        private int runningAnimationCount = 0;
        [SerializeField]
        public bool stoppingTillDone = false;

        private void Awake()
        {
            EventQueue = new Queue<(IEnumerator, bool)>();
            StopQueue = new List<bool>();
        }

        private void Start()
        {
            onAnimationEnd.AddListener(OnIEnumRunningEnd);
            StartCoroutine(RunEverything());
        }

        public void PushAction(IEnumerator move, bool stopTillDone = false)
        {
            // Debugger.Log("Now adding to queue: " + move + "");
            
            EventQueue.Enqueue((move, stopTillDone));
            StopQueue.Add(stopTillDone);
            
            TryRunEverything();
        }
        
        private void TryRunEverything() 
        {
            // Debugger.Log("Try Run with stoppingTillDone as " + stoppingTillDone);
            
            if (stoppingTillDone) return;
            
            // Debugger.Log("Forced Running in try run");
            
            RunEverything();
        }

        private IEnumerator RunEverything()
        {
            // Debugger.Log("Forced Running");
            while (true)
            {

                if (EventQueue.Count == 0)
                {
                    yield return null;
                }
                else
                {
                    yield return PopNextEvent();
                }
            }
        }

        private void OnIEnumRunningEnd()
        {

            if (runningAnimationCount > 0)
            {
                // Debugger.Log("OnIEnumRunningEnd activated with remaining count " + runningAnimationCount  + " - 1");

                runningAnimationCount -= 1;

                if (runningAnimationCount == 0)
                {
                    stoppingTillDone = false;

                    // Debugger.Log("Forced Running on event ends");

                    RunEverything();
                }
            }
            else { };
                //throw new InvalidOperationException("OnIEnumRunningEnd called when remaining Event is empty");
        }

        private IEnumerator PopNextEvent()
        {
            (IEnumerator move, bool stopTilDone) = EventQueue.Dequeue();
            
            //Debugger.Log("Now running: " + move + " with remaining count " + runningAnimationCount + " + 1");
            
            
            if (stopTilDone)
            {
                yield return StartCoroutine(move);
            }
            else
            {
                StartCoroutine(move);
                yield return null;
            }

        }
    }
}