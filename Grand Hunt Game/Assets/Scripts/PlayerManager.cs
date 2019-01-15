using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;

/// <summary>
/// We turned on IsTrigger property in beams left to true because we want to be informed of
/// beams touching players and not collisions.
/// </summary>

namespace Com.MyCompany.MyGame
{
    /// <summary>
    /// Player Manager.
    /// Handles fire input & laser beams.
    /// Photon.PunBehaviour to expose the PhotonView component
    /// </summary>
    public class PlayerManager : Photon.PunBehaviour, IPunObservable
    {
        #region Public Variables

        [Tooltip("The locla player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        [Tooltip("The current health (HP) of our player")]
        public float Health = 1f;

        [Tooltip("The Beams GameObject to control")]
        public GameObject Beams;

        [Tooltip("The Prop GameObject that will change")]
        public GameObject Prop;

        #endregion

        #region Private Variables

        // True, when the user is firing
        bool IsFiring;

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on Game Object by Unity during early initializaation phase.
        /// </summary>
        void Awake()
        {
            // #Important
            // Used in GameManager.cs: Keep track of the localPlayer instance to prevent instantiation when levels
            // are synchronized
            if ( photonView.isMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load  so that instance survives level synchronization, thus gives a seamless
            // experience on levels load.
            DontDestroyOnLoad(this.gameObject);

            if (Beams == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            }
            else
            {
                Beams.SetActive(false);
            }
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {
            
            if (photonView.isMine)
            {
                ProcessInputs();
                
                // Need to also see when other isntance fires
            }
            
            // Trigger Beams active state
            if (Beams != null && IsFiring != Beams.GetActive() )
            {
                Beams.SetActive(IsFiring);
            }

            if (Health <= 0f)
            {
                GM.Instance.LeaveRoom();
            }
        }

        /// <summary>
        /// MonoBehaviour method called when the Collider 'other' enters the trigger.
        /// Affect HP of player if the collider is a beam.
        /// </summary>
        /// <param name="other"></param>
        void OnTriggerEnter(Collider other)
        {


            if (!photonView.isMine)
            {
                return;
            }


            // We are only interested in Beams
            // We will just check for name lol too lazy
            if (!other.name.Contains("Beam"))
            {
                return;
            }

            // Nerf this damage!!!
            Health -= 0.1f;

        }

        // Basically when the beam is constantly being fired at the player
        // We want consistency (delta time guarantees this)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other">Other.</param>
        void OnTriggerStay(Collider other)
        {
            
            // Dont do anything if were not the local player

            if (! photonView.isMine)
            {
                return;
            }


            // Only interested in beamers
            if (!other.name.Contains("Beam"))
            {
                return;
            }

            Health -= 0.1f * Time.deltaTime;

        }

        #endregion

        #region Custom

        /// <summary>
        /// Processes the inputs. Maintains a flag representing when the user is pressing Fire.
        /// </summary>
        void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!IsFiring)
                {
                    IsFiring = true;
                }
            }
            
            if (Input.GetButtonUp("Fire1"))
            {
                if (IsFiring)
                {
                    IsFiring = false;
                }
            }
        }
        #endregion

        #region IPunObservable implementation
        // Need a way to synchronize the firing across the network.
        // Manually synchronize the IsFiring boolean value
        // Since this is very specific to the game, we do this manually.
        // We add an observation in photon view and drag PlayerManager into the slot
        // Stream is what's going to be sent over the network, can only write when we are the localPlayer
        void IPunObservable.OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                // We own this player; send the others our data
                stream.SendNext(IsFiring);
                stream.SendNext(Health);
            }
            else
            {
                // Network Player, receive data
                this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }
        }
        #endregion
    }
}

