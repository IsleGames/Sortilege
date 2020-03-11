using System.Collections;
using Units;
using UnityEngine;

namespace Animations
{
    public class IdleBob : MonoBehaviour
    {

        float Period;
        float Amplitude;
        bool Enabled = true;

        Vector3 initialPosition;


        // Start is called before the first frame update
        void Start()
        {
            Enabled = false;

            var unit = GetComponent<Unit>();
            unit.onTurnBegin.AddListener(() => Enabled = true);
            unit.onTurnEnd.AddListener(() => Enabled = false);
            initialPosition = transform.position;
            StartCoroutine(Bob());
            Period = 2f;
            Amplitude = 0.5f;
        }

        // Super hacky two-state FSM, with reset on transition
        public IEnumerator Bob() 
        {
            //Debug.Log("Begin");
            while (true)
            {
                float t = 0;
                while (Enabled)
                {
                    var delta = Amplitude * Mathf.Cos(2 * Mathf.PI / Period * t);
                    t += Time.fixedDeltaTime;
                    var position = transform.position;
                    position.y += delta;
                    transform.position = position;

                    yield return new WaitForFixedUpdate();
                    while (Game.Ctx.paused) yield return new WaitForFixedUpdate();
                };

                transform.position = initialPosition;
                while (!Enabled)
                {
                    yield return null;
                }

            }
        }
    }
}
