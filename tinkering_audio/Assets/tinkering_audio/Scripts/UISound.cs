using UnityEngine;

public class UISound : MonoBehaviour
{
    [SerializeField]
    private Sound soundSettings;
    private AudioSource audioSource;

    private void Start()
    {
        if (this.GetComponent<AudioSource>() == null)
        {
            audioSource = this.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        }

        else
        {
            audioSource = this.GetComponent<AudioSource>();
        }
    }
}
