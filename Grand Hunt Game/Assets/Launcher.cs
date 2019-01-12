using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For this script, we reference the photon documentation
/// Notice our usage of Script Organization (dividing importance by #region)
/// </summary>

// Namespace: while not mandatory, giving a proper namespace prevents clashes w/ other assets and developers.
// ie. if another dev makes a class with the same name as this. Especially if downloading from asset store.
namespace Com.MyCompany.MyGame
{
    // Deriving our class with MonoBehaviour (turns our class into a Unity Component)
    // I changed this to PunBehaviour
    public class Launcher : Photon.PunBehaviour
    {
        #region Public Variables

        /// <summary>
        /// PUN loglevel
        /// </summary>
        public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

        /// <summary>
        /// The maxmimum # of players per room.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players and so a new room will be created")]
        public byte MaxPlayersPerRoom = 4;

        #endregion
        
        #region Private Variables

        /// <summary>
        /// This client's version number. Users are separated from each other by game version
        /// (Allowing to make breaking changes)
        /// </summary>

        string _gameVersion = "1";

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        /// Also, Awake is called when the script object is initialized, regardless of whether or not the 
        /// script is enabled.
        void Awake()
        {
            // #Critical
            // We dont join the lobby. There is no need to join a lobby to get the list of rooms
            // Note: Lobby is a virtual container or "list" of rooms.
            // A client can only be in a lobby, a room, or neither
            PhotonNetwork.autoJoinLobby = false; // Set to false since we do not need the Lobby features

            // #Critical
            // This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same
            // room sync their level automatically.
            PhotonNetwork.automaticallySyncScene = true;

            // #NotImportant
            // Force LogLevel (dont force the script to be a certain type of LogLevel)
            PhotonNetwork.logLevel = Loglevel;
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            /* -- Changed so that Connect() is called on button click
            Connect();
            */
        }

        #endregion


        #region Public Methods
        
        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joing a random room
        /// - If not yet connected, connect this app instance to the photon cloud network.
        /// </summary>
        public void Connect()
        {
            if (PhotonNetwork.connected)
            {
                // #Critical we need this to attempt joining a Random Room.
                // If it fails, we get notified in OnPhotonRandomJoinFailed() and so we will create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, must first and foremost connect to the photon online server.
                PhotonNetwork.ConnectUsingSettings(_gameVersion); // Starting point for game to be networked and connec to cloud
            }
        }

        #endregion


        #region Photon.PunBehaviour CallBacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("Launcher: OnConnectedToMaster() was called by PUN");

            // #Critical: First we try to join a random room.
            // Success? Good | Failure? called back w/ OnPhotonRandomJoinFailed()
        }

        public override void OnDisconnectedFromPhoton()
        {
            Debug.LogWarning("Launcher: OnDisconnectedFromPhoton() was called by PUN");
        }
        
        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            Debug.Log("Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
            // #Critical: failed to join a random room. Either none exist or all are full. Thus, we should create a room.
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
        }
        
        public override void OnJoinedRoom()
        {
            Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        }
        #endregion
    }
}