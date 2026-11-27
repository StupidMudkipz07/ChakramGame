using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    AudioSource footStepAudioSource;
    void Start()
    {
        footStepAudioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstepSound()
    {
        footStepAudioSource.Play();
    }
    void Update()
    {
        
    }
}
