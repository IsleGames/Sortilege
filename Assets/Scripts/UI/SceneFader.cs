using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SceneFader : MonoBehaviour
    {
        public void FadeAndLoadScene(IEnumerator IEnumAfter, float duration = 2.0f)
        {
            StartCoroutine(LoadScene(IEnumAfter, duration));
        }

        private IEnumerator LoadScene(IEnumerator IEnumAfter, float duration)
        {
            Image img = GetComponent<Image>();
        
            float t = 0;
            while(t < duration)
            {
                float prog = t /duration;

                var color = img.color;
                color = new Color(color.r, color.g, color.b, prog);
                img.color = color;

                t += Time.deltaTime;
                yield return null;
            }

            StartCoroutine(IEnumAfter);
        }
    }
}
