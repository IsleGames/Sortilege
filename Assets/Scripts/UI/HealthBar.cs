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

    private Health health;

    // Temperal Solution
    public GameObject linkedObject;
    
    /*
     * Initializes a healthbar from a named GameObject
     */
    public void Start()
    {
        health = linkedObject.GetComponent<Health>();
    }

    public void Update()
    {
        currentHealth = health.HitPoints;
        maxHealth = health.maximumHealth;

        // set width
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        healthText.text = $"{(int)currentHealth} / {(int)maxHealth}";

    }

    // TODO: animate changes in health
    
}
