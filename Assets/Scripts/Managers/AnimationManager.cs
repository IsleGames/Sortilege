using System;
using System.Collections;
using System.Collections.Generic;
using _Editor;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class AnimationManager : MonoBehaviour
    {
		public UnityEvent onAnimationEnd = new UnityEvent();

        public List<(IEnumerator, bool)> EventQueue;

        public int runningAnimationCount = 0;

        private void Start()
        {
            EventQueue = new List<(IEnumerator, bool)>();
            
            onAnimationEnd.AddListener(OnIEnumRunningEnd);
        }

        public void RunAnimation(IEnumerator move, bool instant = true)
        {
            if (runningAnimationCount == 0 || runningAnimationCount != 0 && instant)
            {
                runningAnimationCount += 1;
                StartCoroutine(move);
            }
            else
            {
                Debugger.Log((move, instant));
                EventQueue.Add((move, instant));
            }
        }

        private void OnIEnumRunningEnd()
        {
            if (runningAnimationCount > 0)
            {
                runningAnimationCount -= 1;
                if (runningAnimationCount == 0 && EventQueue.Count > 0)
                {
                    RunNextEvent();
                    while (EventQueue.Count > 0 && EventQueue[0].Item2)
                        RunNextEvent();
                }
            }
            else
                throw new InvalidOperationException("OnIEnumRunningEnd called when remaining Event is empty");
        }

        private void RunNextEvent()
        {
            runningAnimationCount += 1;
            
            IEnumerator move = EventQueue[0].Item1;
            EventQueue.RemoveAt(0);
            StartCoroutine(move);

        }
    }
}