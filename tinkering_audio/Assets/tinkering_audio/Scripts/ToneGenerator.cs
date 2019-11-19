using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{
    public AudioClip audioClip;
    public float frequency;

    public float[] samples;
    public int sampleRate;
    public int sampleDurationSecs;
    [HideInInspector]
    public int sampleLength;
    
}

[RequireComponent(typeof(AudioSource))]
public class ToneGenerator : MonoBehaviour
{
    [Header("Singleton Referencing")]
    public static ToneGenerator Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    [Header("Wave Attributes")]
    [SerializeField]
    private Sound generatedSound;
    [SerializeField]
    private Sound secondarySound;
    private AudioSource audioSource;

    public List<float> samplesList = new List<float>();
    public GameObject squarePrefab;

    private void Start()
    {
        generatedSound.sampleLength = generatedSound.sampleRate * generatedSound.sampleDurationSecs;

        audioSource = this.GetComponent<AudioSource>();

        Sound[] sounds = new Sound[2];
        generatedSound.audioClip = CreateToneAudioClip(generatedSound);
        secondarySound.audioClip = CreateToneAudioClip(secondarySound);

        sounds[0] = generatedSound;
        sounds[1] = secondarySound;

        //Instance.AddAudioClips(sounds, 1500);
        ToneWaves.Instance.ConvertWaveToSquareWave(generatedSound);
        RefactorSamplesInClip(generatedSound);
        SpawnSampleSquare(generatedSound.samples, 200);
        generatedSound.audioClip = ToneModifiers.Instance.ChangeVolume(generatedSound.audioClip, .01f);
        audioSource.PlayOneShot(generatedSound.audioClip);
    }


    /// <summary>
    /// A function that uses a variety of sound settings and 
    /// the frequency to generate a clip of sound
    /// </summary>
    /// <param name="soundSettings"></param>
    /// <param name="frequency"></param>
    /// <returns>
    /// A Unity AudioClip data type
    /// </returns>
    public AudioClip CreateToneAudioClip(Sound soundSettings)
    {
        soundSettings.sampleLength = soundSettings.sampleRate * soundSettings.sampleDurationSecs;
        float maxValue = 1f / 4f;

        AudioClip audioClip = AudioClip.Create("new_tone", soundSettings.sampleLength, 1, soundSettings.sampleRate, false);

        soundSettings.samples = new float[soundSettings.sampleLength];
        for (var i = 0; i < soundSettings.sampleLength; i++)
        {
            float s = ToneWaves.Instance.GetSinValue(soundSettings.frequency, i, soundSettings.sampleRate);
            float v = s * maxValue;
            soundSettings.samples[i] = v;
        }

        audioClip.SetData(soundSettings.samples, 0);
        return audioClip;
    }

    public void RefactorSamplesInClip(Sound soundSettings)
    {
        soundSettings.audioClip.SetData(soundSettings.samples, 0);
    }

    /// <summary>
    /// Uses the samples from the audioclip to generate a visual intepretation of the wave using squares.
    /// </summary>
    /// <remarks>
    /// It can and should be limited otherwise it will cause peformance issues.
    /// </remarks>
    /// <param name="samples"></param>
    /// <param name="amountToSpawn"</param>
    private void SpawnSampleSquare(float[] samples, int amountToSpawn)
    {
        if (amountToSpawn > samples.Length - 1) amountToSpawn = samples.Length - 1;

        Vector3 spawnPosition = Vector3.zero;
        float squareIntervals = 2.5f;

        for (int i = 0; i < amountToSpawn; i++)
        {
            spawnPosition = new Vector3(spawnPosition.x + squareIntervals, samples[i] * 100 , 0);
            Instantiate(squarePrefab, spawnPosition, Quaternion.identity);
        }
    }

    
}
