using System.Collections;
using _Editor;
using Library;
using Units;
using UnityEngine;

namespace Animations
{
    [RequireComponent(typeof(Unit))]
    public class AttackMotion : MonoBehaviour
    {

        public enum Direction { Left, Right };

        public Direction direction = Direction.Left;
        private float Distance = 80f;
        private float k = 0.3f;
        private float PauseStart = 0.1f;
        // private float PauseEnd = 0.05f;

        private Vector3 initial_position;
        private Vector3 target_position;

        void Start()
        {
            GetComponent<Unit>().onAttack.AddListener( MoveAnimation );
        }

        private void MoveAnimation()
        {
            initial_position = transform.position;
        
            target_position = initial_position;
            target_position.x += (Distance * (direction == Direction.Right ? -1 : 1));
        
            Game.Ctx.AnimationOperator.PushAction(AttackMove(), true);
            Game.Ctx.AnimationOperator.PushAction(Utilities.MoveTo(gameObject, initial_position, k), false);
        }

        private IEnumerator AttackMove()
        {
            yield return new WaitForSeconds(PauseStart);
            
            GameObject obj = gameObject;
        
            Vector3 init = obj.transform.position;

            float p = 0.01f;
            while (p < 1f - 1e-3)
            {
                p += p * k;
            
                Vector3 current = target_position * p + init * (1 - p);
                obj.transform.position = current;
            
                yield return null;
            }

            obj.transform.position = target_position;

            Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
            
            yield return null;
        }
    }
}
