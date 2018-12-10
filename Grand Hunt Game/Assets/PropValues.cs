using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropValues : MonoBehaviour
{

    public Vector3 rotation;
    public GameObject gm;
    public string key;
    
    // Start is called before the first frame update
    void Start()
    {
        key = GetComponent<MeshFilter>().mesh.name;
        gm = GameObject.FindGameObjectWithTag("GM");
        rotation = gm.GetComponent<GM>().database[key];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
