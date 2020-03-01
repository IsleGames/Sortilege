using System;
using System.Collections;
using System.Text.RegularExpressions;
using _Editor;
using Cards;
using Library;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        private RectTransform _bgBar, _redBar, _blueBar, _whiteBar;
        private TextMeshProUGUI _barText;
        public Health pHealth;

        [NonSerialized]
        public float animationKFactor = 0.06f;

        private Rect _thisRect;

        private void Start()
        {
            foreach (Transform tr in GetComponentsInChildren<Transform>())
                switch (tr.name)
                {
                    case "Background":
                        _bgBar = tr.GetComponent<RectTransform>();
                        break;
                    case "RedBar":
                        _redBar = tr.GetComponent<RectTransform>();
                        break;
                    case "BlueBar":
                        _blueBar = tr.GetComponent<RectTransform>();
                        break;
                    case "WhiteBar":
                        _whiteBar = tr.GetComponent<RectTransform>();
                        break;
                    case "BarTMPText":
                        _barText = tr.GetComponent<TextMeshProUGUI>();
                        // Debugger.Log(tr.gameObject + "'s GUI is " + _barText);
                        break;
                }

            _whiteBar.GetComponent<SpriteRenderer>().enabled = false;
            // _blueBar.GetComponent<SpriteRenderer>().enabled = false;
            
            var sp = _bgBar.GetComponent<SpriteRenderer>();
            _thisRect = GetComponent<RectTransform>().rect;
            sp.size = new Vector2(
                _thisRect.width,
                _thisRect.height
                );
            
            if (!pHealth)
            {
                pHealth = GetComponentInParent<Health>();
                if (!pHealth)
                {
                    pHealth = GetComponent<Health>();
                    if (!pHealth) throw new EntryPointNotFoundException("Health Component Not found");
                }
            }
            
            GetComponentInParent<Unit>().onHealthChange.AddListener(delegate { UpdateStatus(); });
        }
        
		public static IEnumerator HitPointNumberPController(
            TextMeshProUGUI tmp,
            float initHitPoints,
            float targetHitPoints,
            float totalHitPoints,
            float k)
        {
            float p = 0;
            while (p < 1f - 5e-3)
            {
                p += (1 - p) * k;
                
                float curHP = targetHitPoints * p + initHitPoints * (1 - p);
                tmp.text = $"{Mathf.RoundToInt(curHP)} / {Mathf.RoundToInt(totalHitPoints)}";
                
                yield return null;
            }
            
            tmp.text = $"{Mathf.RoundToInt(targetHitPoints)} / {Mathf.RoundToInt(totalHitPoints)}";
	        
            Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            yield return null;
        }

        public void UpdateStatus(bool animated = true)
        {
            float totHitPoints = pHealth.GetMaximumDisplayHP();
            float hpRatio = pHealth.hitPoints / totHitPoints;
            float barrierRatio = (pHealth.hitPoints + pHealth.barrierHitPoints) / totHitPoints;
            
            
            AdjustBar(hpRatio, _redBar, animated);
            AdjustBar(barrierRatio, _blueBar, animated);

            if (animated)
            {
                float initHitPoints = Int32.Parse(Regex.Match(_barText.text, @"^\d+").ToString());
                Game.Ctx.AnimationOperator.PushAction(
                    HitPointNumberPController(
                        _barText,
                        initHitPoints,
                        pHealth.hitPoints + pHealth.barrierHitPoints,
                        totHitPoints,
                        animationKFactor
                        ),
                    true
                    );
            }
            else
                _barText.text = $"{(int)pHealth.hitPoints + pHealth.barrierHitPoints} / {(int)totHitPoints}";
        }

        private void AdjustBar(float ratio, RectTransform bar, bool animated = true)
        {
            float newWidth = ratio * _thisRect.width;
            float xShift = (-1 + ratio) * _thisRect.width * .5f;

            var sp = bar.GetComponent<SpriteRenderer>();
            sp.enabled = true;
            if (animated)
            {
                Vector2 targetSize = new Vector2(
                newWidth,
                _thisRect.height * .98f
                );
            
                Vector2 targetAnchoredPosition = new Vector3(xShift, 0f, 0f);
                
                Game.Ctx.AnimationOperator.PushAction(
                    Utilities.RectTransMoveAndScaleTo(bar, sp, targetSize, targetAnchoredPosition, animationKFactor)
                );
            }
            else
            {
                sp.size = new Vector2(
                    newWidth,
                    _thisRect.height * .98f
                    );
                
                bar.anchoredPosition = new Vector3(xShift, 0f, 0f);
            }
        }
    }
}