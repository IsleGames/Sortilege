using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Library
{
    public interface IColorable
    {
        void SetColor(Color c);
        Color GetColor();
    }

    public class SpriteRendererC : IColorable
    {
        public SpriteRenderer renderer { get; private set; }
        public SpriteRendererC(SpriteRenderer sprite)
        {
            renderer = sprite;
        }

        public void SetColor(Color c)
        {
            renderer.color = c;
        }

        public Color GetColor()
        {
            return renderer.color;
        }
    }

    public class ImageC: IColorable
    {
        public Image image;
        public ImageC(Image i)
        {
            image = i;
        }
        
        public void SetColor(Color c)
        {
            image.color = c;
        }

        public Color GetColor()
        {
            return image.color;
        }
    }

    public class TMP_C: IColorable
    {
        public TMPro.TextMeshPro text;
        public TMP_C(TextMeshPro t)
        {
            text = t;
        }

        public void SetColor(Color c)
        {
            text.faceColor = c;
        }
        public Color GetColor()
        {
            return text.faceColor;
        }
    }

    public static class ImageUtilities
    {
        
        public static IEnumerator FadeToBlack(IEnumerable<IColorable> images, float k)
        {
            float p = 0f;
            while (p < 1f - 1e-3)
            {
                p += (1 - p) * k;
                var color = new Color(p, p, p);
                foreach (var image in images)
                {
                    image.SetColor(color);
                }
                yield return null;
            }
        }

        public static IEnumerator FadeToTransparent(IEnumerable<IColorable> images, float k)
        { 
            float p = 0f;
            while (p < 1f - 1e-3)
            {
                p += (1 - p) * k;
                foreach (var image in images)
                {
                    var color = image.GetColor();
                    color.a = (1-p);
                    image.SetColor(color);
                }
                yield return null;
            }
        }
    }
}