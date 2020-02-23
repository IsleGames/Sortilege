using System;
using _Editor;
using Cards;
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
        }

        private void Update()
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            float totHitPoints = pHealth.GetMaximumDisplayHP();
            
            float hpRatio = pHealth.hitPoints / totHitPoints;
            AdjustBar(hpRatio, _redBar);
            _barText.text = $"{(int)pHealth.hitPoints + pHealth.barrierHitPoints} / {(int)totHitPoints}";

            float barrierRatio = (pHealth.hitPoints + pHealth.barrierHitPoints) / totHitPoints;
            AdjustBar(barrierRatio, _blueBar);
        }

        private void AdjustBar(float ratio, RectTransform bar)
        {
            float newWidth = ratio * _thisRect.width;
            float xShift = (-1 + ratio) * _thisRect.width * .5f;

            var sp = bar.GetComponent<SpriteRenderer>();
            sp.enabled = true;
            sp.size = new Vector2(
                newWidth,
                _thisRect.height * .98f
                );
            
            bar.anchoredPosition = new Vector3(xShift, 0f, 0f);
            // Debugger.Log(bar.position);
        }
    }
}