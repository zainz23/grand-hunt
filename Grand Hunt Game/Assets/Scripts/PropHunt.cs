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
    public MeshFilter MESHFILTER; // Set this to PROP <MESHFILTER> Component [the mesh we are changing]

    public GameObject currentCOL;
    // Start is called before the first frame update
    void Start()
    {
        // On game start, only the player is visible.
        Player.SetActive(true);
        PROP.SetActive(false);
        MESHFILTER = PROP.GetComponent<MeshFilter>();
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Player.SetActive(true);
            PROP.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            Debug.Log("DOWN");
            Vector3 scale = MESHFILTER.transform.localScale;
            if (scale.x > 1 && scale.x <= 20) {
                scale.y -= 1;
                scale.x -= 1;
                scale.z -= 1;
                MESHFILTER.transform.localScale = scale;
            }
            

        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            Debug.Log("UP");
            Vector3 scale = MESHFILTER.transform.localScale;
            if (scale.x >= 1 && scale.x < 20) {
                scale.y += 1;
                scale.x += 1;
                scale.z += 1;
                MESHFILTER.transform.localScale = scale;
            }

        }
    }

    // While player is colliding with object...
    void OnTriggerStay(Collider coll)
    {
        currentCOL = coll.gameObject;
        
        // Tag the object in game
        // Note: Children of Player are left untagged.
        if (coll.gameObject.tag == "PROP")
        {
            if (Input.GetKeyDown(KeyCode.E) )
            {
                MESHFILTER.mesh = coll.GetComponent<MeshFilter>().mesh;

                if (coll.GetComponent<PropValues>()) {
                    Vector3 rotation = coll.GetComponent<PropValues>().rotation;
                    MESHFILTER.transform.localEulerAngles = rotation;

                    Vector3 scale = coll.transform.localScale;
                    scale.y += 3;
                    scale.x += 3;
                    scale.z += 3;

                    MESHFILTER.transform.localScale = scale;
                }

                // Player wants to become a barrel
                PROP.SetActive(true);

                // Player no longer exists
                Player.SetActive(false);

            }

        }
    }
}
