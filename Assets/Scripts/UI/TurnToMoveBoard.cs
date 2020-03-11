using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using _Editor;
using Managers;

namespace UI
{
    public class TurnToMoveBoard : MonoBehaviour
    {
        private TextMeshProUGUI boardTMPText;
        private Image boardImage;
        
        public float fadeTime = .5f;
        public float holdTime = 1f;

        private void Awake()
        {
            boardTMPText = transform.Find("BoardTMPText").GetComponent<TextMeshProUGUI>();
            // Debugger.Log("start " + boardTMPText);
            boardImage = transform.Find("BoardImage").GetComponent<Image>();
            
            SetAlpha(0f);
        }

        public void SetText(string text)
        {
            // Debugger.Log("settext" + boardTMPText.text);
            boardTMPText.text = text;
        }

        public void StartAnimation(bool dontDisappear)
        {
            if (!dontDisappear)
                Game.Ctx.AnimationOperator.PushAction(FadeInOut(), true);
            else
                Game.Ctx.AnimationOperator.PushAction(FadeInOnly(), true);
        }

        private void SetAlpha(float a)
        {
            Color newColor = boardImage.color;
            newColor.a = a;
            boardImage.color = newColor;
            
            newColor = boardTMPText.color;
            newColor.a = a;
            boardTMPText.color = newColor;
        }

        private IEnumerator FadeInOnly()
        {
            // Debugger.Log("FadeInOut working");
            // Debugger.Log("stoppingTillDone: " + Game.Ctx.AnimationOperator.stoppingTillDone);
            
            yield return null;

            float p = 0f;
            float totalTime = 0;
            while (p < 1f)
            {
                totalTime += Time.deltaTime;
                p = totalTime / fadeTime;
                if (p >= 1f) p = 1f;

                SetAlpha(p);

                yield return null;
            }
        }
        
        private IEnumerator FadeInOut()
        {
            // Debugger.Log("FadeInOut working");
            // Debugger.Log("stoppingTillDone: " + Game.Ctx.AnimationOperator.stoppingTillDone);
            
            yield return null;

            float p = 0f;
            float totalTime = 0;
            while (p < 1f)
            {
                totalTime += Time.deltaTime;
                p = totalTime / fadeTime;
                if (p >= 1f) p = 1f;

                SetAlpha(p);

                yield return null;
            }

            p = 0f;
            totalTime = 0;
            while (p < 1f)
            {
                totalTime += Time.deltaTime;
                p = totalTime / holdTime;
                if (p >= 1f) p = 1f;
                
                yield return null;
            }
            
            p = 0f;
            totalTime = 0;
            while (p < 1f)
            {
                totalTime += Time.deltaTime;
                p = totalTime / fadeTime;
                if (p >= 1f) p = 1f;

                SetAlpha(1f - p);

                yield return null;
            }
            
            Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            Destroy(gameObject);
            
            yield return null;
        }
    }
}