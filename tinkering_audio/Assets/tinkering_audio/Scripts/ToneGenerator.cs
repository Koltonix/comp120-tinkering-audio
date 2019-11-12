using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SoundSettings
{
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
    private SoundSettings soundSettings;
    private AudioSource audioSource;

    public List<float> samplesList = new List<float>();
    public GameObject squarePrefab;

    private void Start()
    {
        soundSettings.sampleLength = soundSettings.sampleRate * soundSettings.sampleDurationSecs;

        audioSource = this.GetComponent<AudioSource>();
        AudioClip newClip = CreateToneAudioClip(soundSettings, 1500);
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
    public AudioClip CreateToneAudioClip(SoundSettings soundSettings, int frequency)
    {
        soundSettings.sampleLength = soundSettings.sampleRate * soundSettings.sampleDurationSecs;
        float maxValue = 1f / 4f;

        AudioClip audioClip = AudioClip.Create("new_tone", soundSettings.sampleLength, 1, soundSettings.sampleRate, false);

        float[] samples = new float[soundSettings.sampleLength];
        for (var i = 0; i < soundSettings.sampleLength; i++)
        {
            float s = GetSinValue(frequency, i, soundSettings.sampleRate);
            float v = s * maxValue;
            samples[i] = v;
            

        }

        SpawnSampleSquare(samples, 200);

        audioClip.SetData(samples, 0);
        return audioClip;
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

    /// <summary>
    ///  A sine function that produces a sine wave based on
    ///  a variety of variables
    /// </summary>
    /// <param name="frequency"></param>
    /// <param name="indexPosition"></param>
    /// <param name="sampleRate"></param>
    /// <returns></returns>
    private float GetSinValue(float frequency, float indexPosition, float sampleRate)
    {
        return Mathf.Sin(2.0f * Mathf.PI * frequency * (indexPosition / sampleRate));
    }
}
