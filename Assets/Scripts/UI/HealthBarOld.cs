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
    
    // public Slider blockSlider;
    // public Text blockText;
    // public TMPro.TextMeshProUGUI title;
    

    public void Update()
    {
        Health health = transform.parent.GetComponent<Health>();

        healthSlider.maxValue = health.hitPoints;
        healthSlider.value = health.hitPoints;

        healthText.text = $"{(int)currentHealth} / {(int)maxHealth}";

        // blockSlider.maxValue = maxHealth;
        // blockSlider.value = health.block;
        // blockText.text = $"{(int)health.block}";

    }

    // TODO: animate changes in health
    
}
