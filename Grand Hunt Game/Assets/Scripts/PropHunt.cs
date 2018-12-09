using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To add props
// Make a public GameObject designated for that prop
// In Start(), set the GameObject to false
public class PropHunt : MonoBehaviour
{
    public GameObject Player;   // The player that is being disappeared
    public GameObject Barrel;   // What you turn into
    /* Can add loads more here */

    // Start is called before the first frame update
    void Start()
    {
        Player.SetActive(true);
        Barrel.SetActive(false);
    }

    // While player is colliding with object...
    void OnTriggerStay(Collider coll)
    {
        // Tag the object in game
        // Note: Children of Player are left untagged.
        if (coll.gameObject.tag == "Barrel")
        {
            if (Input.GetKeyDown(KeyCode.E) )
            {
                // Player wants to become a barrel
                Barrel.SetActive(true);
                // Player no longer exists
                Player.SetActive(false);
                /* Set any other props to false aswell */

            }
        }
    }
}
