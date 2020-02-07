using System;
using Units;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TestDamage : MonoBehaviour
{

    public HealthBar healthbar;
    // Start is called before the first frame update

    public void Start()
    {
        healthbar.Init(true);
    }

    public void Damage()
    {
        GameObject.Find("Player").GetComponent<Health>().Damage(10);
    }

}
