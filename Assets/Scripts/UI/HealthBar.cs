using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    // defaults
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float rate = 1f; // health units per frame

    public Slider healthSlider;
    

    public void Init(float health, float maxHealth = 100) {
        currentHealth = health;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        /// set width;
    }

    public void Update()
    {
        healthSlider.value = currentHealth;

    }


    public IEnumerator takeDamage(float damage)
    {
        while (damage > 0 && currentHealth > 0) {
            currentHealth -= rate;
            damage = Mathf.Clamp(damage - rate, 0, damage);
            yield return null;
        }

        yield return null;
    }

     
}
