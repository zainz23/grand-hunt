using UnityEngine;

public class AudioRpc : Photon.MonoBehaviour
{

    // The logic behind the audio is unsure yet
    // As in, should we auto play audio every 30s
    // Or allow users to ping. Maybe even both
    // Later we can have customizable pings
    // That players can unlock
    [Tooltip("Sound for hiders/proppers")]
    public AudioClip ping;

    AudioSource p_Source;

    // Start is called before the first frame update
    void Awake()
    {
        p_Source = GetComponent<AudioSource>();
    }

    [PunRPC]
    void Ping()
    {
        if (!this.enabled)
        {
            return;
        }
        p_Source.clip = ping;
        p_Source.Play();
    }

    private void OnApplicationFocus(bool focus)
    {
        this.enabled = focus;
    }
}
