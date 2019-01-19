using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To add props
// Make a public GameObject designated for that prop
// In Start(), set the active of GameObject to false
// In OnTrigger, set actives in colliding triggers as necessary
//      False if you want it to disappear
//      True if you want it to appear

// TODO: Make this script easy to add more props.
public class PropHunt : Photon.PunBehaviour, IPunObservable
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject Player;

    [Tooltip("While the Player is the local instance. PlayerObj is the 'body' ")]
    public Transform PlayerObj;

    public GameObject PROP;   // What you turn into
    public MeshFilter MESHFILTER; // Set this to PROP <MESHFILTER> Component [the mesh we are changing]
    public Vector3 resetScale;

    public float originalPROPSIZE;
    public GameObject currentCollision;

    public Transform Sender;    // Mine
    public Transform Receiver;    // Others


    #region Private Variables
    // Boolean used to let others and self know that the player is trying to prop into something
    bool IsPropping;
    // Boolean used to let others and self know that the play is trying to turn back to human form
    bool IsHumaning = true;

    int SendViewID = -1;              // View we are sending
    int ReceiveViewID = -1;    // View we are receiving    
    

    public float propID;    // temporarily public
    #endregion


    private void Awake()
    {
        if (photonView.isMine)
        {
            Player = this.gameObject;
            PlayerObj = Player.transform.GetChild(1);
            SendViewID = this.gameObject.GetComponent<PhotonView>().ownerId;
            // Sender = PhotonView.Find(SendViewID).transform;
        }
        if (PROP == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PROP Reference.", this);
        }
        else
        {
            PROP.SetActive(false);
        }
    }

    void Update()
    {
        if (photonView.isMine)
        {
            TransformInputs();
        }

        // Trigger Prop active state
        if (PROP != null && IsPropping != PROP.GetActive())
        {
            // PhotonView.Find(PlayerPropID);
            PROP.SetActive(IsPropping);
            Debug.Log(this.gameObject.GetComponent<PhotonView>().ownerId);
        }
        if (PlayerObj != null && IsHumaning != PlayerObj.gameObject.GetActive() )
        {
            PlayerObj.gameObject.SetActive(IsHumaning);
            PROP.SetActive(false);
            PROP.transform.localScale = resetScale;
        }
        

    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerObj == null)
        {
            PlayerObj = this.transform.GetChild(1);
        }
        // On game start, only the player is visible.
        PlayerObj.gameObject.SetActive(true);
        PROP.SetActive(false);
        MESHFILTER = PROP.GetComponent<MeshFilter>();
        resetScale = PROP.transform.localScale;
        originalPROPSIZE = resetScale.z;
    }

    void TransformInputs()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            Vector3 scale = MESHFILTER.transform.localScale;
            if (scale.x > 1 && scale.x <= 20)
            {
                scale.y -= 1;
                scale.x -= 1;
                scale.z -= 1;
                MESHFILTER.transform.localScale = scale;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            Vector3 scale = MESHFILTER.transform.localScale;
            if (scale.x >= 1 && scale.x < 20)
            {
                scale.y += 1;
                scale.x += 1;
                scale.z += 1;
                MESHFILTER.transform.localScale = scale;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!IsPropping)
            {
                IsPropping = true;
                IsHumaning = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Z) )
        {
            if (!IsHumaning)
            {
                IsHumaning = true;
                IsPropping = false;
            }
        }
    }

    // While player is colliding with object...
    void OnTriggerStay(Collider coll)
    {

        if (!photonView.isMine)
        {
            return;
        }
        currentCollision = coll.gameObject;
        if (currentCollision != null && SendViewID != -1)
        {
            PhotonView.Get(this).RPC("TriggerDat", PhotonTargets.All, SendViewID);
        }
        
        
        /*
        // Tag the object in game
        // Note: Children of Player are left untagged.
        if (coll.gameObject.tag == "PROP")
        {
            if (Input.GetKeyDown(KeyCode.E) )
            {
                IsPropping = true;
                IsHumaning = false;
                MESHFILTER.mesh = coll.GetComponent<MeshFilter>().sharedMesh;

                // Assign an ID to serialize to all other players
                
                // propID = SetMeshID(MESHFILTER.mesh);
                

                Debug.Log("Mesh name: " + MESHFILTER.mesh.name);

                if (coll.GetComponent<PropValues>())
                {
                    Vector3 rotation = coll.GetComponent<PropValues>().rotation;
                    MESHFILTER.transform.localEulerAngles = rotation;

                    float X, Y, Z;
                    // TAKES PROPS LOCAL TRANSFORM AND MULTIPLIES WITH PLAYERS LOCAL TRANSFORM
                    X = coll.transform.localScale.x * originalPROPSIZE;
                    Y = coll.transform.localScale.y * originalPROPSIZE;
                    Z = coll.transform.localScale.z * originalPROPSIZE;
                    MESHFILTER.transform.localScale = new Vector3(X, Y, Z);
                }
            }
        }
        */
        
    }
    

    [PunRPC]
    void TriggerDat(int viewID, PhotonMessageInfo info)
    {
        

        if (Input.GetKeyDown(KeyCode.E)) 
        {
            
            if (currentCollision.gameObject.tag == "PROP")
            {
                IsPropping = true;
                IsHumaning = false;
                if (!photonView.isMine)
                {
                    Sender = PhotonView.Find(viewID).transform;
                    
                    GameObject meshTest = Resources.Load<GameObject>("Mesh/Prop_Stone_3");
                    Mesh m = meshTest.gameObject.GetComponent<MeshFilter>().sharedMesh;
                    Sender.gameObject.GetComponent<MeshFilter>().sharedMesh = m;
                }

                MESHFILTER.mesh = currentCollision.GetComponent<MeshFilter>().sharedMesh;
                // Assign an ID to serialize to all other players

                // propID = SetMeshID(MESHFILTER.mesh);
                Debug.Log("Mesh name: " + MESHFILTER.mesh.name);

                if (currentCollision.GetComponent<PropValues>())
                {
                    Vector3 rotation = currentCollision.GetComponent<PropValues>().rotation;
                    MESHFILTER.transform.localEulerAngles = rotation;

                    float X, Y, Z;
                    // TAKES PROPS LOCAL TRANSFORM AND MULTIPLIES WITH PLAYERS LOCAL TRANSFORM
                    X = currentCollision.transform.localScale.x * originalPROPSIZE;
                    Y = currentCollision.transform.localScale.y * originalPROPSIZE;
                    Z = currentCollision.transform.localScale.z * originalPROPSIZE;
                    MESHFILTER.transform.localScale = new Vector3(X, Y, Z);
                }
            }
        }
        
    }

    float SetMeshID(Mesh mesh)
    {
        if (string.Equals(mesh.name, "Icosphere.073 Instance") )
        {
            // Assets/Resources/Mesh/Prop_Stone_3.fbx
            GameObject meshTest = Resources.Load<GameObject>("Mesh/Prop_Stone_3");
            if (meshTest == null)
            {
                Debug.LogError("WE GOT NOTHING. Object type: ");
            }
            else
            {
                Debug.Log(meshTest.name + " is the name baby!!!!");
                Mesh m = meshTest.gameObject.GetComponent<MeshFilter>().sharedMesh;
                Debug.Log("THY MESHNAME = " + m.name);
            }
            return 1f;
        }
        return 0f;
    }

    #region IPunObservable implementation
    // Need a way to synchronize the firing across the network.
    // Manually synchronize the IsFiring boolean value
    // Since this is very specific to the game, we do this manually.
    // We add an observation in photon view and drag PlayerManager into the slot
    // Stream is what's going to be sent over the network, can only write when we are the localPlayer
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player; send the others our data
            stream.SendNext(IsPropping);
            stream.SendNext(IsHumaning);
            // stream.SendNext(propID);
            // stream.SendNext(MyViewID);
        }
        else
        {
            // Network Player, receive data
            this.IsPropping = (bool)stream.ReceiveNext();
            this.IsHumaning = (bool)stream.ReceiveNext();
            // this.propID = (float)stream.ReceiveNext();
            // this.ReceiveViewID = (int)stream.ReceiveNext();
        }
    }
    #endregion
}
