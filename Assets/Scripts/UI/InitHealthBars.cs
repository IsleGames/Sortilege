using System;
using Units;
using UnityEngine;
using UnityEngine.UI;

public class InitHealthBars: MonoBehaviour
{

    public HealthBar playerHealthBar;
    public HealthBar enemyHealthBar;
    // Start is called before the first frame update

    public void Start()
    { 
        playerHealthBar.Init("Player");
        enemyHealthBar.Init("Enemy");
    }

}
