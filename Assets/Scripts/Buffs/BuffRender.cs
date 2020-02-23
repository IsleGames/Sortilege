using System;
using System.Data;
using UnityEngine;
using TMPro;

using Effects;
using Managers;

namespace Buffs
{
    public class BuffRender : MonoBehaviour
    {
        private TextMeshProUGUI _buffText;
        private void Start()
        {
            Buff buff = GetComponent<Buff>();
            
            // Set attribute 
            var buffIconRenderer = transform.Find("BuffSprite").GetComponent<SpriteRenderer>();
            buffIconRenderer.sprite = Resources.Load<Sprite>(VfxManager.BuffSpritePaths[buff.type]);
            
            // Set Text
            _buffText = transform.Find("BuffTMPText").GetComponent<TextMeshProUGUI>();
            _buffText.text = $"{buff.amount}";
        }

        private void Update()
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            Buff buff = GetComponent<Buff>();
            _buffText.text = $"{buff.amount}";
        }
    }
}