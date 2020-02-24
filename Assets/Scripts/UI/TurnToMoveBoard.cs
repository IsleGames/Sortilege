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
        public float holdTime = 1.2f;

        private void Awake()
        {
            boardTMPText = transform.Find("BoardTMPText").GetComponent<TextMeshProUGUI>();
            Debugger.Log("start " + boardTMPText);
            boardImage = transform.Find("BoardImage").GetComponent<Image>();
        }

        public void SetText(string text)
        {
            Debugger.Log("settext" + boardTMPText.text);
            boardTMPText.text = text;
        }

        public void StartAnimation()
        {
            Game.Ctx.AnimationOperator.RunAnimation(FadeInOut(), true);
        }

        private IEnumerator FadeInOut()
        {
            yield return null;

            float p = 0f;
            float totalTime = 0;
            while (p < 1f)
            {
                totalTime += Time.deltaTime;
                p = totalTime / fadeTime;
                if (p >= 1f) p = 1f;

                Color newColor = boardImage.color;
                newColor.a = p;
                boardImage.color = newColor;
                
                newColor = boardTMPText.color;
            // Debugger.Log(newColor);
                newColor.a = p;
                boardTMPText.color = newColor;

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

                Color newColor = boardImage.color;
                newColor.a = 1f - p;
                boardImage.color = newColor;
                
                newColor = boardTMPText.color;
                newColor.a = 1f - p;
                boardTMPText.color = newColor;

                yield return null;
            }
            
            // Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            
            yield return null;
        }
    }
}