using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ToneGenerator : MonoBehaviour
{
    [Header("Wave Attributes")]
    [SerializeField]
    private int sampleRate = 44100;
    [SerializeField]
    private int sampleDurationSecs = 5;
    private int sampleLength;

    private AudioSource audioSource;

    private void Start()
    {
        sampleLength = sampleRate * sampleDurationSecs;

        audioSource = this.GetComponent<AudioSource>();
        AudioClip newClip = CreateToneAudioClip(1500);
        audioSource.PlayOneShot(newClip);
    }

    private AudioClip CreateToneAudioClip(int frequency)
    {
        float maxValue = 1f / 4f;

        AudioClip audioClip = AudioClip.Create("new_tone", sampleLength, 1, sampleRate, false);

        float[] samples = new float[sampleLength];
        for (var i = 0; i < sampleLength; i++)
        {
            float s = Mathf.Sin(2.0f * Mathf.PI * frequency * ((float)i / (float)sampleRate));
            float v = s * maxValue;
            samples[i] = v;
        }

        audioClip.SetData(samples, 0);
        return audioClip;
    }
}
