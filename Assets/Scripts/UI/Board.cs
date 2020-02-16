using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public static Board Ctx;
    public CardZone Hand;
    public CardZone Queue;

    // Start is called before the first frame update
    void Start()
    {
        Ctx = this;

    }

    
}
