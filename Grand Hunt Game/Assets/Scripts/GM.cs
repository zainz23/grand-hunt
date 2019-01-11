using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    //Dictionary<string, PROPClass> database = new Dictionary<string, PROPClass>();
    public Dictionary<string, Vector3> database = new Dictionary<string, Vector3>();

    // Start is called before the first frame update
    void Awake()
    {
        database.Add("Cube.143 Instance", new Vector3(0,90,0));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
