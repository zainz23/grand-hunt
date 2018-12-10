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
    public GameObject PROP;   // What you turn into
    // Start is called before the first frame update
    void Start()
    {
        // On game start, only the player is visible.
        Player.SetActive(true);
        PROP.SetActive(false);
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Player.SetActive(true);
            PROP.SetActive(false);
        }
    }

    // While player is colliding with object...
    void OnTriggerStay(Collider coll)
    {
        
        // Tag the object in game
        // Note: Children of Player are left untagged.
        if (coll.gameObject.tag == "PROP")
        {
            if (Input.GetKeyDown(KeyCode.E) )
            {
                Debug.Log(coll.GetComponent<MeshFilter>().mesh.name);
                MeshFilter mf = this.GetComponentInChildren<MeshFilter>();
                mf.mesh = coll.GetComponent<MeshFilter>().mesh;

                mf.GetComponent<Transform>().rotation = coll.transform.rotation;

                if (coll.GetComponent<PropValues>()) {
                    Debug.Log("TAKE");
                    Quaternion rotation = this.GetComponentInParent<Transform>().localRotation;
                    mf.GetComponent<Transform>().localRotation = rotation;
                }

                // Player wants to become a barrel
                PROP.SetActive(true);

                // Player no longer exists
                Player.SetActive(false);
               

            }
        }
        

    }
}
