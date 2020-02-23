using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FadeToNewScene : MonoBehaviour
{
    //SpriteRenderer loadingBackground;
    public float Duration = 1f;
    public string SceneName;

    // Start is called before the first frame update
    private void Start()
    {
        //loadingBackground = GetComponent<SpriteRenderer>();
    }
    

    public void FadeAndLoadScene()
    {
        StartCoroutine(LoadScene(SceneName));
    }

    private IEnumerator LoadScene(string scene_name)
    {
        //
        var sprites = FindObjectsOfType<SpriteRenderer>();
        var  images = FindObjectsOfType<Image>();
        float t = 0;
        while(t < Duration)
        {
            var pct = 1-t / Duration;
            foreach (var sprite in sprites)
            {
                sprite.color = new Color(pct, pct, pct);
            }
            foreach (var image in images)
            {
                image.color = new Color(pct, pct, pct);
            }
            t += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(scene_name);
        
    }


}
