using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    public class Select : MonoBehaviour
    {
        public enum Direction { Up, Down };

        private Direction dire = Direction.Up;


        private void OnMouseUp()
        {
            if (Game.Ctx.paused) return;
            
            dire = Direction.Down;
            Game.Ctx.AfterBattleRewardOperator.OnCardSelect(GetComponent<Card>());
        }


        public IEnumerator Abscond()
        {
            float t = 0.5f;
            float v = 0;
            while (t < 1.5f)
            {
                t += Time.deltaTime;
                v += 150 * t * t * t;
                
                var position = transform.position;
                if (dire == Direction.Down)
                {
                    position.y += -v * Time.deltaTime;
                }
                else
                {
                    position.y += v * Time.deltaTime;
                }
                
                transform.position = position;
                yield return null;
            }
        }
    }
}
