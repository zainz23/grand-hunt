using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To add props
// Make a public GameObject designated for that prop
// In Start(), set the active of GameObject to false
// In OnTrigger, set actives in colliding triggers as necessary
//      False if you want it to disappear
//      True if you want it to appear

// TODO: Make this script easy to add more props.
public class PropHunt : MonoBehaviour
{
    public GameObject Player;   // The player that is being disappeared
    public GameObject Barrel;   // What you turn into

    /* Can add loads more here */
    public GameObject Yellow_Tree;
    public GameObject Wheel_Barrow;
    // Start is called before the first frame update
    void Start()
    {
        // On game start, only the player is visible.
        Player.SetActive(true);
        Barrel.SetActive(false);
        Yellow_Tree.SetActive(false);
        Wheel_Barrow.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Player.SetActive(true);
            Barrel.SetActive(false);
            Yellow_Tree.SetActive(false);
            Wheel_Barrow.SetActive(false);
        }
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
                Yellow_Tree.SetActive(false);
                Wheel_Barrow.SetActive(false);

            }
        }
        if (coll.gameObject.tag == "Yellow_Tree")
        {
            if (Input.GetKeyDown(KeyCode.E) )
            {
                // Player wants to become a Yellow_Tree
                Yellow_Tree.SetActive(true);
                // Player + Props become inactive
                Player.SetActive(false);
                Barrel.SetActive(false);
                Wheel_Barrow.SetActive(false);
            }
        }
        if (coll.gameObject.tag == "Wheel_Barrow")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Player wants to become a Wheel_Barrow
                Wheel_Barrow.SetActive(true);
                // Player + Props become inactive
                Player.SetActive(false);
                Barrel.SetActive(false);
                Yellow_Tree.SetActive(false);
            }
        }

    }
}
