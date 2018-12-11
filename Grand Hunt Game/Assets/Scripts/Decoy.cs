using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoy : MonoBehaviour
{
    public GameObject PROP;
    float X, Y, Z;

    // Update is called once per frame
    void Update()
    {
        X = PROP.transform.localScale.x;
        Y = PROP.transform.localScale.y;
        Z = PROP.transform.localScale.y;

        // Press C to make a copy in-game
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject decoy = Instantiate(PROP, transform.position, transform.rotation);
            decoy.transform.localScale = new Vector3(X - 3, Y - 3, Z- 3);
            decoy.transform.localPosition = this.gameObject.transform.localPosition;

        }

    }
}
