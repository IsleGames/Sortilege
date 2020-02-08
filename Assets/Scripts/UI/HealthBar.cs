using System.Collections;
using System;
using Units;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    // defaults
    public float maxHealth;
    public float currentHealth;

    public float rate = 1f; // health units per frame
    public bool isplayer;

    public Slider healthSlider;
    public Text healthText;

    public void Init(bool isitplayer, float health = 100, float maxHealth = 100)
    {
        isplayer = isitplayer;
        if (isplayer)
        {
            health = GameObject.Find("Player").GetComponent<Health>().health;
            maxHealth = GameObject.Find("Player").GetComponent<Health>().maximumHealth;
        }
        
        currentHealth = health;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        /// set width;
    }

    public void Update()
    {
        if (isplayer)
        {
            currentHealth = GameObject.Find("Player").GetComponent<Health>().health;
            maxHealth = GameObject.Find("Player").GetComponent<Health>().maximumHealth;
        }
        healthSlider.value = currentHealth;
        healthText.text = $"{(int)currentHealth} / {(int)maxHealth}";

    }
    
}
