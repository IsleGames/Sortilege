using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using _Editor;

public class CardZone : MonoBehaviour
{

    public Vector3 start;
    List<GameObject> objects;
    public float moveSpeed = 0.1f;


    // Start is called before the first frame update
  

    void Awake()
    {
        start = transform.position;
        start.z = 0;
        objects = new List<GameObject>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator MoveTo(GameObject obj, Vector3 pos, float time)
    {
        Vector3 init = obj.transform.position;
        Debugger.Log("Start: " + init.ToString());
        Debugger.Log("Going to: " + pos.ToString());
        float elapsed = 0;
        while (elapsed < time)
        {
            float t = elapsed / time;
            var current = (pos * t + init * (1 - t));
            obj.transform.position = current;
            Debugger.Log(obj.name + "@"+obj.transform.position.ToString());
            elapsed += Time.deltaTime;
            Debugger.Log(elapsed.ToString());
            yield return null;
        }
        yield return null;
        

    }

    public void AddObject(GameObject obj, float speed = 0)
    {
        speed = speed > 0 ? speed : moveSpeed;
        StartCoroutine(MoveTo(obj, start, moveSpeed));
        Bounds bounds = obj.GetComponent<BoxCollider2D>().bounds;
        start.x += bounds.extents.x * 2.1f;
        Debugger.Log("New start: " + start.ToString());
        objects.Add(obj);
    }

    public void Reset()
    {
        start = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void RemoveObject(GameObject obj)
    {
        int i = objects.IndexOf(obj);
        int k = i + 1;
        var remaining = objects.GetRange(k, objects.Count - k);
        var start = obj.GetComponent<BoxCollider2D>()?.bounds.center;
        objects.RemoveRange(i, objects.Count - i);
        foreach (var remainder in remaining) {
            AddObject(remainder);
        }
    }

    
}
