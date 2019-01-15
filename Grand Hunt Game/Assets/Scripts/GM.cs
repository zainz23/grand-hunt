using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.MyCompany.MyGame
{
    // Photon.PunBehaviour allows Photon callbacks & defining of methods from PunBehaviour
    public class GM : Photon.PunBehaviour
    {
        #region Public Properties

        // This variable is available w/o having to hold a pointer to an instance of GM
        // ie. we can do GM.instance.xxx()
        static public GM Instance;

        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        #endregion

        #region Photon Messages

        /// <summary>
        /// Called when the local player leaves the room.
        /// We need to load the launcher scene (the lobby).
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // Not seen if you're the player connecting
            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


                LoadArena();
            }
        }
        public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // Seen when other disconnects

            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerDisconnected isMasterClient " + PhotonNetwork.isMasterClient); // Called before OnPhotonPlayerDisconnected


                LoadArena();
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// This method is called when we are going to load the appropriate room
        /// For now, this is based on player count (map 1 means 1 player only, while map 2 can have 2 players, etc.)
        /// </summary>
        void LoadArena()
        {
            if(!PhotonNetwork.isMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to load a level but we are not the master Client");
            }
            Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.room.PlayerCount);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Explicitily making the local player leave the Photon Network room
        /// * Later we will add features such as saving data, or inserting a confirmation step (for leaving)
        /// </summary>
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        #endregion

        #region MonoBehaviour Callbacks

#if !UNITY_5_4_OR_NEWER
        /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
#endif


        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
        }

        /// <summary>
        /// On Unity initialization
        /// </summary>
        void Start()
        {
            Instance = this;
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                // We're in a room, spawn a character for the local player.
                // It gets synced by using PhotonNetwork.Instantiate
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.Log("We are instantiating LocalPlayer from " + Application.loadedLevelName);

                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);

                }
                else
                {
                    Debug.Log("Ignoring scene load for " + Application.loadedLevelName);
                }   
            }
#if UNITY_5_4_OR_NEWER
            // What this new code does is watching for a level being loaded and raycast downwards the current player's position to see if we hit anything. 
            // If we don't, this is means we are not above the arena's ground and we need to be repositioned back to the center, 
            // exactly like when we are entering the room for the first time.
            // *Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
                    {
                        this.CalledOnLevelWasLoaded(scene.buildIndex);
                    };
#endif
        }

        #endregion




        // #Important Commented out until we figure out database of props...
        /* 
        //Dictionary<string, PROPClass> database = new Dictionary<string, PROPClass>();
        public Dictionary<string, Vector3> database = new Dictionary<string, Vector3>();

        // Start is called before the first frame update
        void Awake()
        {
            database.Add("Cube.143 Instance", new Vector3(0, 90, 0));

        }

        // Update is called once per frame
        void Update()
        {

        }
        */
    }
}
