using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is for setting up name the of player over our network
/// Additionally, it saves their name for the next time they open their device. (cool!)
/// </summary>
namespace Com.MyCompany.MyGame
{
    /// <summary>
    /// Player name input field. Allow user to input his name/nickname
    /// We will/can display this above the player in the game.
    /// RequireComponent -> This enforces the inputField; guarantees trouble-free usage of this script.
    /// </summary>
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Variables
        // Store the PlayerPref Key to avoid typos
        static string playerNamePrefKey = "PlayerName"; // Wont change over time

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// PlayerPrefs.HasKey(), PlayerPrefs.GetString() and PlayerPrefs.SetString() is a simple lookup list of paired entries
        /// (Arbitrary String Key : Value)
        /// </summary>
        // Start is called before the first frame update
        void Start()
        {
            string defaultName = " ";
            InputField _inputField = this.GetComponent<InputField>();
            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            // Main point of this script (setting up name of player over the network)
            PhotonNetwork.playerName = defaultName;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the name of the player and saves it in the PlayerPrefs for future sessions.
        /// </summary>
        /// <param name="value">The name of the Player</param>
        
        public void SetPlayerName(string value)
        {
            // #Important
            PhotonNetwork.playerName = value + " "; // Force a trailing space in case value is an empty string, else playerName wouldnt be updated.

            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
        /*
        public void EndEditOnEnter(string value)
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                PlayerPrefs.SetString(playerNamePrefKey, value);
            }
        }
        */

        #endregion
    }
}
