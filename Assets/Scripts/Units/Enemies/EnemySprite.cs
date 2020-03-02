using System;
using _Editor;
using UnityEngine;

namespace Units.Enemies
{
    public class EnemySprite : MonoBehaviour
    {
        private Vector3 _originalScale;
        public float onSelectZoomScale = 1.1f;

        private void Start()
        {
            _originalScale = transform.localScale;
        }

        public void OnMouseEnter()
        {
            if (Game.Ctx.inSelectEnemyMode)
            {
                OnSelectZoom(onSelectZoomScale);
            }
        }

        public void OnMouseExit()
        {
            if (Game.Ctx.inSelectEnemyMode)
            {
                OnSelectZoom(1f);
            }
        }

        public void OnMouseDown()
        {
            if (Game.Ctx.inSelectEnemyMode)
            {
                Game.Ctx.inSelectEnemyMode = false;
                Game.Ctx.player.EndTurn(GetComponentInParent<Enemy>());
            }
        }
        
        
        private void OnSelectZoom(float scale)
        {
            Vector3 newLocalScale = _originalScale * scale;
            transform.localScale = newLocalScale;
        }
    }
}