using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // When playing with UI in code, always add this

public class HealthDisplay : MonoBehaviour
{
    public int health = 10;
    public Text healthText;

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health : " + health;
        // Button to deplete hp
        if (Input.GetKeyDown(KeyCode.K) )
        {
            health--;
        }
    }
}
