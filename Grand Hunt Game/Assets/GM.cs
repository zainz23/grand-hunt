using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    //Dictionary<string, PROPClass> database = new Dictionary<string, PROPClass>();
    public Dictionary<string, Quaternion> database = new Dictionary<string, Quaternion>();

    // Start is called before the first frame update
    void Awake()
    {
        database.Add("Cube.143 Instance", new Quaternion(0, 40, 0, 0));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
