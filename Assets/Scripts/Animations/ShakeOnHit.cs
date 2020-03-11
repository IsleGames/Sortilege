using System.Collections;
using UnityEngine;

// [RequireComponent(typeof(Units.Unit))]
namespace Animations
{
    public class ShakeOnHit : MonoBehaviour
    {
        float Duration = 0.5f;
        float Period = 0.2f;
        float Amplitude = 4f;
        Units.Unit unit;

        public void Start()
        {
            unit = GetComponent<Units.Unit>();
            unit.onDamage.AddListener(() => Game.Ctx.AnimationOperator.PushAction(Shake(), false));
        }

        public IEnumerator Shake()
        {
            Vector3 initialposition = transform.position;
            float t = 0;
            var unit = GetComponent<Units.Unit>();


            if (Game.Ctx.BattleOperator.activeUnit != unit)
            {
                while (t < Duration)
                {
                    var delta = Amplitude * Mathf.Cos(2 * Mathf.PI / Period * t);
                    t += Time.deltaTime;
                    var position = transform.position;
                    position.y += delta;
                    transform.position = position;
                    yield return new WaitForFixedUpdate();
                }
                transform.position = initialposition;
            }
            yield return null;
        }
    }
}
