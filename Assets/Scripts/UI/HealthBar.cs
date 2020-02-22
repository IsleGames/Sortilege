using System;
using Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        private RectTransform _redBar, _blueBar, _whiteBar;
        private Health _pHealth;

        private void Start()
        {
            foreach (Transform tr in GetComponentsInChildren<Transform>())
                switch (tr.name)
                {
                    case "Background":
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
                }

            _whiteBar.GetComponent<SpriteRenderer>().enabled = false;
            _blueBar.GetComponent<SpriteRenderer>().enabled = false;

            // pHealth = GetComponentInParent<Health>();
            
            _pHealth = GetComponent<Health>();
        }

        private void Update()
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            float hpRatio = _pHealth.hitPoints / _pHealth.maximumHitPoints;
            AdjustBar(hpRatio, _redBar);
        }

        private void AdjustBar(float ratio, RectTransform bar)
        {
            Rect thisRect = GetComponent<RectTransform>().rect;
            float newWidth = ratio * thisRect.width;
            float xShift = -ratio * .5f * thisRect.width;
            bar.GetComponent<SpriteRenderer>().size = new Vector2(
                newWidth,
                thisRect.height * .98f
                );
            bar.position = new Vector3(xShift, 0f, 0f);
        }
    }
}