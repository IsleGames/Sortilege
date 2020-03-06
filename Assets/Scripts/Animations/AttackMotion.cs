using System.Collections;
using UnityEngine;
using Library;
using Units;
[RequireComponent(typeof(Unit))]
public class AttackMotion : MonoBehaviour
{

    public enum Direction { Left, Right };

    public Direction direction = Direction.Left;
    float Distance = 100f;
    float Speed = 0.5f;
    float PauseStart = 0.5f;
    float PauseEnd = 0.5f;

    void Start()
    {
        var unit = GetComponent<Unit>();
        unit.onAttack.AddListener(
            delegate { Game.Ctx.AnimationOperator.PushAction(MoveAnimation(), true); }
            );
    }

    public IEnumerator Move()
    {
        yield return StartCoroutine(MoveAnimation());

    }

    private IEnumerator MoveAnimation()
    {
        yield return null;
        yield return new WaitForSeconds(PauseStart);
        Vector3 initial_position = transform.position;
        Vector3 target_position = transform.position;
        target_position.x += (Distance * (direction == Direction.Right ? -1 : 1));

        yield return StartCoroutine(
            Utilities.MoveTo(gameObject, target_position, Speed / 2));
        yield return StartCoroutine(
            Utilities.MoveTo(gameObject, initial_position, Speed / 2));
        yield return new WaitForSeconds(PauseEnd);
        //Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
        yield return null;
    }
}
