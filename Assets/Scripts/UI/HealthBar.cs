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

    public Slider healthSlider;
    public Text healthText;

    // Temporal Solution; change it later
    public GameObject unitObject;
    
    private Health health;

    /*
     * Initializes a healthbar from a Units.Health object
     */
    
    // TBH I don't think overloading methods is a good habit; It messes up your code even more
    
    // public void Init(Health healthObj)
    // {
    //     health = healthObj;
    // }

    /*
     * Initializes a healthbar from a named GameObject
     */
    public void Start()
    {
        // Cm'on, don't initialize this under TestDamage.cs!
        // Create a general enemy Prefab and hook the healthbar as a sub-object would be the best choice
        
        if (unitObject != null)
        {
            health = unitObject.GetComponent<Health>();
        }
        else
        {
            throw new NullReferenceException("unitObject not defined");
        }
    }

    public void Update()
    {
        currentHealth = health.health;
        maxHealth = health.maximumHealth;

        // set width
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        healthText.text = $"{(int)currentHealth} / {(int)maxHealth}";

    }

    // TODO: animate changes in health
    
}
