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

            GameObject decoy = Instantiate(PROP, transform.position, transform.rotation);
            // Since our character is small, calculate new scale.
            decoy.transform.localScale = new Vector3(X * 0.25f, Y * 0.25f, Z * 0.25f);

            decoy.transform.localPosition = this.gameObject.transform.localPosition;

        }

    }
}
