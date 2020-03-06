using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Library;
using Units;

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
        List<IColorable> colorables = new List<IColorable>();
        foreach (var sprite in sprites)
        {
            colorables.Add(new SpriteRendererC(sprite));
        }
        foreach (var image in images)
        {
            colorables.Add(new ImageC(image));
        }
        yield return ImageUtilities.FadeToTransparent(colorables, 1f/Duration);

    }

    public IEnumerator BlockingFade()
    {
        yield return StartCoroutine(Fade());
        //Game.Ctx.AnimationOperator.onAnimationEnd.Invoke();
        //yield return null;
    }


}
