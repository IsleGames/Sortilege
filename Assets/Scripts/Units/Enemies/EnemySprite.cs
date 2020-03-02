using System;
using _Editor;
using UI;
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
            if (Game.Ctx.BattleOperator.inSelectEnemyMode)
            {
                OnSelectZoom(onSelectZoomScale);
            }
        }

        public void OnMouseExit()
        {
            if (Game.Ctx.BattleOperator.inSelectEnemyMode)
            {
                OnSelectZoom(1f);
            }
        }

        public void OnMouseDown()
        {
            if (Game.Ctx.BattleOperator.inSelectEnemyMode)
            {
				Game.Ctx.transform.GetComponentInChildren<CommandButton>().SetStatus(false);
                    
                Game.Ctx.BattleOperator.player.EndTurn(GetComponentInParent<Enemy>());
            }
        }
        
        
        private void OnSelectZoom(float scale)
        {
            Vector3 newLocalScale = _originalScale * scale;
            transform.localScale = newLocalScale;
        }
    }
}