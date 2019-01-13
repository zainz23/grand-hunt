using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is our namespace so we dont get mixups with assets
using Com.MyCompany.MyGame;

public class PropValues : MonoBehaviour
{
    
    public Vector3 rotation;
    public GameObject gm;
    public string key;
    /*
    // Start is called before the first frame update
    void Start()
    {
        
        key = GetComponent<MeshFilter>().mesh.name;
        gm = GameObject.FindGameObjectWithTag("GM");

        if (gm.GetComponent<GM>().database.ContainsKey(key)) {
            rotation = gm.GetComponent<GM>().database[key];
        } else {
            rotation = new Vector3(0, 90, 0);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
