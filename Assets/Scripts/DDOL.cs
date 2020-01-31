using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once InconsistentNaming
public class DDOL : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
