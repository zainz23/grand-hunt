using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoy : MonoBehaviour
{
    public GameObject PROP;
    public MeshFilter MESHFILTER;
    public GameObject smokeprefab;

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
            //PROP.transform.SetParent(PROP.transform.parent, false);

            //SPAWNS OBJECT BEHIND PLAYER

            // RAYCAST TO INSPECT IF OBJECT EXIST BEFORE CREATING OBJECT
            RaycastHit hit;

            float distance = (0.35f);
            Vector3 spawnPosition = transform.position;
            spawnPosition.y += 0.02f;

            // IF THERE IS ALREADY AN OBJECT THAT EXIST DONT SPAWN OBJECT
            if (Physics.Raycast(spawnPosition, transform.TransformDirection(Vector3.forward) * -distance, out hit, distance))
            {
                Debug.DrawRay(spawnPosition, transform.TransformDirection(Vector3.forward) * -distance, Color.red);
                Debug.Log("HIT SO NO SPAWN" + hit.collider.name);
            
            // IF OBJECT IS NOT IN WAY SPAWN DECOY OBJECT
            } else {
                Debug.DrawRay(spawnPosition, transform.TransformDirection(Vector3.forward) * -5f, Color.cyan);
                Debug.Log("NO HIT, OBJECT SPAWN");

                GameObject decoy = Instantiate(PROP, transform.position + transform.forward * -(0.25f), PROP.transform.rotation);
                //decoy.transform.localPosition = this.gameObject.transform.localPosition;
                decoy.AddComponent<Rigidbody>();
                decoy.AddComponent<BoxCollider>();

                decoy.AddComponent<DecoySmoke>();
                decoy.GetComponent<DecoySmoke>().smoke = smokeprefab;

                decoy.tag = "PROP";


                // Since our character is small, calculate new scale.
                float X, Y, Z;
                // TAKES PROPS LOCAL TRANSFORM AND MULTIPLIES WITH PLAYERS LOCAL TRANSFORM
                X = PROP.transform.localScale.x * transform.localScale.x;
                Y = PROP.transform.localScale.y * transform.localScale.y;
                Z = PROP.transform.localScale.z * transform.localScale.z;
                decoy.transform.localScale = new Vector3(X, Y, Z);
                //decoy.transform.localScale = new Vector3(1, 1, 1);


            }



        }

    }
}
