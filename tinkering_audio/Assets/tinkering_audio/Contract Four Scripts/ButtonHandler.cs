using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonHandler : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void PlayAudioClip()
    {
        audioSource.PlayOneShot(audioClip);
    }

}

