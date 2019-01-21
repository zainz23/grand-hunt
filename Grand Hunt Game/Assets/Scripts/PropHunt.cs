using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script takes care of the mechanics/logic of prop transformations for players
/// By making use of serialization Prop IDs with the PropValues.cs script
/// </summary>

public class PropHunt : Photon.PunBehaviour, IPunObservable
{
    #region Public Variables

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject Player;

    [Tooltip("While the Player is the local instance. PlayerObj is the 'body' ")]
    public Transform PlayerObj;

    [Tooltip("Prop GameObject")]
    public GameObject PROP;   // What you turn into
    [Tooltip("Prop's mesh filter")]
    public MeshFilter MESHFILTER; // Set this to PROP <MESHFILTER> Component [the mesh we are changing]
    [Tooltip("Vector3 to reset Transform of playerObj")]
    public Vector3 resetScale;

    public float originalPROPSIZE;
    [Tooltip("Current object the player is colliding with")]
    public GameObject currentCollision;

    #endregion

    #region Private Variables

    // Boolean used to let others and self know that the player is trying to prop into something
    [SerializeField]
    bool IsPropping;
    // Boolean used to let others and self know that the play is trying to turn back to human form
    [SerializeField]
    bool IsHumaning = true;
    // This variable is just an ID for the prop
    [SerializeField]
    private int PropValue;

    #endregion
    


    private void Awake()
    {
        if (photonView.isMine)
        {
            Player = this.gameObject;
            PlayerObj = Player.transform.GetChild(1);
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

    /// <summary>
    /// Updates every frame
    /// </summary>
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
            // Prints this PhotonView's ID
            // Debug.Log(this.gameObject.GetComponent<PhotonView>().ownerId);

        }
        if (PlayerObj != null && IsHumaning != PlayerObj.gameObject.GetActive() )
        {
            PlayerObj.gameObject.SetActive(IsHumaning);
            PROP.SetActive(false);
            PROP.transform.localScale = resetScale;
        }

        // PropValue of ID = 0, gives a null from our PropValues table
        if (PropValue != 0)
        {
            string result = this.gameObject.GetComponent<PropValues>().values[PropValue];
            GameObject meshTest = Resources.Load<GameObject>(result);
            Mesh m = meshTest.gameObject.GetComponent<MeshFilter>().sharedMesh;

            // Testing...
            var photonViews = UnityEngine.Object.FindObjectsOfType<PhotonView>();
            foreach (var view in photonViews)
            {
                var player = view.owner;
                //Objects in the scene don't have an owner, its means view.owner will be null
                if (player != null)
                {
                    var playerPrefabObject = view.gameObject;
                    var p = playerPrefabObject.gameObject.transform.GetChild(2).gameObject;
                    MeshFilter mf = p.GetComponent<MeshFilter>();
                    mf.mesh = m;
                }
            }
        }
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
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

    /// <summary>
    /// Translates the user input and sets variables as necessary
    /// </summary>
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

    /// <summary>
    /// While player is colliding with an object
    /// </summary>
    /// <param name="coll"></param>
    void OnTriggerStay(Collider coll)
    {

        if (!photonView.isMine)
        {
            return;
        }
        currentCollision = coll.gameObject;

        // Tag the object in game
        // Note: Children of Player are left untagged.
        if (coll.gameObject.tag == "PROP")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                IsPropping = true;
                IsHumaning = false;
                MESHFILTER.mesh = coll.GetComponent<MeshFilter>().sharedMesh;

                // Assign an ID to serialize to all other players

                PropValue = SetMeshID(MESHFILTER.mesh);

                Debug.Log("Mesh name: " + MESHFILTER.mesh.name);
                /* Will use this when serializing rotations and scale
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
                */
            }
        }

    }

    /* RPC Function, just used as testing...
     * Called w/ PhotonView.Get(this).RPC("TriggerDat", PhotonTargets.All, SendViewID);
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
    */



    #region IPunObservable implementation
    // Need a way to synchronize the firing across the network.
    // Manually synchronize the IsFiring boolean value
    // Since this is very specific to the game, we do this manually.
    // We add an observation in photon view and drag PropHunt into the slot
    // Stream is what's going to be sent over the network, can only write when we are the localPlayer
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player; send the others our data
            stream.SendNext(IsPropping);
            stream.SendNext(IsHumaning);
            stream.SendNext(PropValue); // Sends the prop ID
        }
        else
        {
            // Network Player, receive data
            this.IsPropping = (bool)stream.ReceiveNext();
            this.IsHumaning = (bool)stream.ReceiveNext();
            this.PropValue = (int)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// Essentially makes the PV of the character have a way to obtain the gameObject
    /// </summary>
    /// <param name="info"></param>
    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //Assign this gameObject to player called instantiate the prefab
        info.sender.TagObject = this.gameObject;
    }
    #endregion

    #region Custom
    /*  Name                | Instance Name |       Path                |       ID
     * Prop_Axe             | Cube.146      | Mesh/Prop_Axe             |       1
     * Prop_Barrel          | Cylinder.121  | Mesh/Prop_Barrel          |       2
     * Prop_Crate           | Cube.144      | Mesh/Prop_Crate           |       3
     * Prop_Fence           | Cube.145      | Mesh/Prop_Fence           |       4
     * Prop_Mushroom        | Cube.149      | Mesh/Prop_Mushroom        |       5
     * Prop_Stone_1         | Icosphere.074 | Mesh/Prop_Stone_1         |       6
     * Prop_Stone_2         | Icosphere.072 | Mesh/Prop_Stone_2         |       7
     * Prop_Stone_3         | Icosphere.073 | Mesh/Prop_Stone_3         |       8
     * Prop_TreeTrunk_Cutoff| Cylinder.129  | Mesh/Prop_TreeTrunk_Cutoff|       9
     * Prop_Wood_1          | Cylinder.122  | Mesh/Prop_Wood_1          |       10
     * Prop_Wood_2          | Cylinder.126  | Mesh/Prop_Wood_2          |       11
     * Prop_WoodenWheelBarro| Cube.143      | Mesh/Prop_WoodenWheelBarrow|      12
     * Trees_Green          | Cylinder.118  | Mesh/Trees_Green          |       13
     * Trees_Pink           | Cylinder.119  | Mesh/Trees_Pink           |       14
     * Trees_Yellow         | Cylinder.120  | Mesh/Trees_Yellow         |       15
     * 
     * Use GameObject meshTest = Resources.Load<GameObject>("/Filepath");
     */
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mesh">ID to be set to PropValue for the mesh changed</param>
    /// <returns>A PropValue ID</returns>
    int SetMeshID(Mesh mesh)
    {
        // Prop_Axe
        if (string.Equals(mesh.name, "Cube.146 Instance"))
        {
            return 1;
        }
        // Prop_Barrel
        if (string.Equals(mesh.name, "Cylinder.121 Instance"))
        {
            return 2;
        }
        // Prop_Crate
        if (string.Equals(mesh.name, "Cube.144 Instance"))
        {
            return 3;
        }
        // Prop_Fence
        if (string.Equals(mesh.name, "Cube.145 Instance"))
        {
            return 4;
        }
        // Prop_Mushroom
        if (string.Equals(mesh.name, "Cube.149 Instance"))
        {
            return 5;
        }
        // Prop_Stone_1
        if (string.Equals(mesh.name, "Icosphere.074 Instance"))
        {
            return 6;
        }
        // Prop_Stone_2
        if (string.Equals(mesh.name, "Icosphere.072 Instance"))
        {
            return 7;
        }
        // Prop_Stone_3
        if (string.Equals(mesh.name, "Icosphere.073 Instance"))
        {
            return 8;
        }
        // Prop_TreeTrunk_Cutoff
        if (string.Equals(mesh.name, "Cylinder.129 Instance"))
        {
            return 9;
        }
        // Prop_Wood_1
        if (string.Equals(mesh.name, "Cylinder.122 Instance"))
        {
            return 10;
        }
        // Prop_Wood_2
        if (string.Equals(mesh.name, "Cylinder.126 Instance"))
        {
            return 11;
        }
        // Prop_WoodenWheelBarrow
        if (string.Equals(mesh.name, "Cube.143 Instance"))
        {
            return 12;
        }
        // Trees_Green
        if (string.Equals(mesh.name, "Cylinder.118 Instance"))
        {
            return 13;
        }
        // Trees_Pink
        if (string.Equals(mesh.name, "Cylinder.119 Instance"))
        {
            return 14;
        }
        // Trees_Yellow
        if (string.Equals(mesh.name, "Cylinder.120 Instance"))
        {
            return 15;
        }

        return 0;
    }

    #endregion
}
