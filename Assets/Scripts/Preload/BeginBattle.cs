using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BeginBattle : MonoBehaviour
{
    SpriteRenderer loadingBackground;
    public float Duration = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        loadingBackground = GetComponent<SpriteRenderer>();
    }
    

    public void LoadUITestScene()
    {
        StartCoroutine(LoadScene(1));
    }

    private IEnumerator LoadScene(int idx)
    {
        // 
        float t = 0;
        while(t < Duration)
        {
            var pct = 1-t / Duration;
            loadingBackground.color = new Color(pct, pct, pct);
            t += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(idx);
        
    }


}
