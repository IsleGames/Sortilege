using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public enum UnitType : int
{
    Player,
    Enemy,
    Unknown
}

public abstract class Unit : MonoBehaviour
{

	public UnitType type;
	public bool initialized = false;

	public float hitPoint;
	public float maximumHitPoint;

	// Calling the Effect list BuffList makes it clearer
	public List<Effect> buffList;

	private void Start()
	{

	   buffList = new List<Effect>();

	}

	// public void Init()
	// {
		
	// }

//
//    private void LoadData()
//    {
//        
//    }
    
    
}
