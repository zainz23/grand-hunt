using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoy : MonoBehaviour
{
    public GameObject PROP;
    public MeshFilter MESHFILTER;
    float X, Y, Z;

    void Update()
    {

        // Press C to make a copy in-game
        if (Input.GetKeyDown(KeyCode.C))
        {
            X = PROP.transform.localScale.x;
            Y = PROP.transform.localScale.y;
            Z = PROP.transform.localScale.y;

            // rotation.eulerAngles = new Vector3(0, 90, 0);
            // Sets its local orientation rather than its global orientation.
            PROP.transform.SetParent(PROP.transform.parent, false);
            GameObject decoy = Instantiate(PROP, PROP.transform.position, PROP.transform.rotation);

            // Since our character is small, calculate new scale.

            decoy.transform.localScale = new Vector3(X * 0.25f, Y * 0.25f, Z * 0.25f);
            // decoy.transform.localScale = new Vector3(X, Y, Z );
            decoy.transform.localPosition = this.gameObject.transform.localPosition;

        }

    }
}
