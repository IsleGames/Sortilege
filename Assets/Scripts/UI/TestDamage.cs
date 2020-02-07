using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TestDamage : MonoBehaviour
{

    public HealthBar healthbar;
    // Start is called before the first frame update
    
        
    public void Damage()
    {
        healthbar.StartCoroutine("takeDamage", 10);
    }

}
