using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class Select : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        
    }


    private void OnMouseUpAsButton()
    {
        DeckList deck = GameObject.Find("DeckList").GetComponent<DeckList>();
        deck.Add(GetComponent<CardData>());
    }

}
