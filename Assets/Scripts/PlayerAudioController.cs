using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstepSound()
    {
        audioSource.Play();
    }
    void Update()
    {
        
    }
}
