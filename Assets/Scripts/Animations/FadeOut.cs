using System.Collections;
using System.Collections.Generic;
using Library;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace Animations
{
    [RequireComponent(typeof(Units.Unit))]
    public class FadeOut : MonoBehaviour
    {
        //SpriteRenderer loadingBackground;
        public float Duration;

        public void Start()
        {
            var unit = GetComponent<Unit>();
            unit.onDead.AddListener(
                () => Game.Ctx.AnimationOperator.PushAction(
                    BlockingFade(), true)
            );
        }

        public IEnumerator Fade()
        {
            //
            var sprites = GetComponentsInChildren<SpriteRenderer>();
            var  images = GetComponentsInChildren<Image>();
            var texts = GetComponentsInChildren<TMPro.TextMeshPro>();
            List<IColorable> colorables = new List<IColorable>();
            foreach (var sprite in sprites)
            {
                colorables.Add(new SpriteRendererC(sprite));
            }
            foreach (var image in images)
            {
                colorables.Add(new ImageC(image));
            }
            foreach (var t in texts)
            {
                colorables.Add(new TMP_C(t));
            }
            yield return ImageUtilities.FadeToTransparent(colorables, 1f/Duration);
            Game.Ctx.EnemyOperator.Destroy(GetComponent<Units.Enemies.Enemy>());
        }

        public IEnumerator BlockingFade()
        {
            yield return StartCoroutine(Fade());
        }


    }
}
