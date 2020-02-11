using System;
using Units;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TestDamage : MonoBehaviour
{

    public HealthBar playerHealthBar;
    public HealthBar enemyHealthBar;
    // Start is called before the first frame update

    public void Start()
    { 
        playerHealthBar.Init("Player");
        enemyHealthBar.Init("Enemy");
    }

    public void Damage()
    {
        GameObject.Find("Player").GetComponent<Health>().Damage(10);
    }

}
