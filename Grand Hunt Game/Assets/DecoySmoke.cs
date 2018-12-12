using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoySmoke : MonoBehaviour
{

    public GameObject smoke;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player") {
            GameObject decoy = Instantiate(smoke, transform.position, transform.rotation);
            Destroy(decoy, decoy.GetComponent<ParticleSystem>().main.duration + 5f);
            Destroy(this.gameObject, 0.25f);
        }


    }
}
