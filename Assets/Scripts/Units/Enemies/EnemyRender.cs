using UnityEngine;

namespace Units.Enemies
{
    public class EnemyRender : MonoBehaviour
    {
        public Sprite imgSprite;
                
        private SpriteRenderer imgSpriteRenderer;
        
        private void Start()
        {
            imgSpriteRenderer = transform.Find("EnemySprite").GetComponent<SpriteRenderer>();

            imgSpriteRenderer.sprite = imgSprite;
        }
    }
}