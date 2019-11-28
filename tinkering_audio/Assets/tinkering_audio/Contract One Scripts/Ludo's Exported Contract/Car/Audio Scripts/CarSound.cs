using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//-----------------------------------------------------------------------
// <copyright file="PickUpCoin.cs">
// MIT License Copyright (c) 2019.
// </copyright>
// <author> Ludovico Bitti
// <summary> generating the sound waves that increases it's frequency depending on car velocity
// <this program takes care of 
// </summary>
//----


public class CarSound : MonoBehaviour
{
    public static CarSound Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this);
        }
    }

    private AudioSource audioSource;
    private AudioClip outAudioClip;

    public int inputVelocity = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        outAudioClip = CreateAudioToneClip(400);
        audioSource.clip = outAudioClip;
        PlayOutAudio();
    }
    public void PlayOutAudio()
    {
        audioSource.Play();
    }

    public AudioClip CreateAudioToneClip(int frequency)
    {
        int sampleDurationSecs = 5;
        int sampleRate = 44100;
        int sampleLength = sampleRate * sampleDurationSecs;
        float maxValue = 1f;

        var audioClip = AudioClip.Create("Sound", sampleLength, 1, sampleRate, false); // creates empty audio clip

        float[] samples = new float[sampleLength]; //array of loats for the saple

        for (var i = 0; i < sampleLength; i++)
        {
            float s = GenerateAudioFrame((int)(frequency * inputVelocity), sampleRate, i); //cretes sample amplitude 
            float v = s * maxValue; //set resoult
            samples[i] = v;
        }

        audioClip.SetData(samples, 0);
        return audioClip;
    }

    //create wave behaviour 
    public float GenerateAudioFrame(int frequency, int sampleRate, int i)
    {
        float output = 4*(Mathf.Sin(2 * Mathf.PI * frequency * ((float)i / (float)sampleRate) / (3f*Mathf.PI)));
        
        return output;
    }

    public void Update()
    {
        GetComponent<AudioSource>().pitch = Mathf.Clamp(GetComponentInParent<Rigidbody>().velocity.magnitude/10f, 0, 2.5f); //setting pitch audio source

    }
}
