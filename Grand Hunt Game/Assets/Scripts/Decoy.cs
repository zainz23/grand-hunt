using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoy : MonoBehaviour
{
    public GameObject PROP;
    public MeshFilter MESHFILTER;

    void Start()
    {
        MESHFILTER = PROP.GetComponent<MeshFilter>();
    }

    void Update()
    {

        // Press C to make a copy in-game
        if (Input.GetKeyDown(KeyCode.C) && PROP.activeSelf)
        {
            
            // Sets its local orientation rather than its global orientation.
            PROP.transform.SetParent(PROP.transform.parent, false);
            GameObject decoy = Instantiate(PROP, PROP.transform.position, PROP.transform.rotation);
            decoy.AddComponent<BoxCollider>();
            decoy.tag = "PROP";

            // Since our character is small, calculate new scale.
            float X, Y, Z;
            // TAKES PROPS LOCAL TRANSFORM AND MULTIPLIES WITH PLAYERS LOCAL TRANSFORM
            X = PROP.transform.localScale.x * transform.localScale.x;
            Y = PROP.transform.localScale.y * transform.localScale.y;
            Z = PROP.transform.localScale.z * transform.localScale.z;
            decoy.transform.localScale = new Vector3(X, Y, Z);

            // THIS IS ORIGINAL SIZE OF THE OBJECTS
            //decoy.transform.localScale = new Vector3(1, 1, 1);

            decoy.transform.localPosition = this.gameObject.transform.localPosition;

        }

    }
}
