using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

//-----------------------------------------------------------------------
// <copyright file="PickUpCoin.cs">
// MIT License Copyright (c) 2019.
// </copyright>
// <author> Ludovico Bitti
// <author> Christopher Philip Robertson
// <summary> in program i am generating the sound waves that will be playid once the coin is hit by the player
// </summary>
//----

[RequireComponent(typeof(AudioSource))]
public class PickUpCoin : MonoBehaviour
{ 
    public AudioClip carAudioClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        carAudioClip = CreateAudioToneClip(250);
        carAudioClip = RefactorAudioClip(carAudioClip, frequency: 2500, sampleRate: 40100, sampleLength: 80000);

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            audioSource.PlayOneShot(carAudioClip);
        }
    }

    public AudioClip CreateAudioToneClip(int frequency)
    {
        int sampleDurationSecs = 2;
        int sampleRate = 40100;
        int sampleLength = sampleRate * sampleDurationSecs;
        float maxValue = 2f;

        var audioClip = AudioClip.Create("Sound", sampleLength, 1, sampleRate, false);

        return audioClip;
    }

    /// <summary>
    /// the for loop generate audio with different frequency and overlaying them ontop of eachother
    /// </summary>
    /// <param name="audioClip">clip where audio is applied to</param>
    /// <param name="frequency">frequanzy of the generated audio</param> used 
    /// <param name="sampleRate">how many samples per second </param>
    /// <param name="sampleLength">how long is the sample per second</param>
    /// <returns></returns>
    /// <remarks>
    /// Christopher pair programmed this by helping me understand for loop.
    /// </remarks>
    /// 
    private AudioClip RefactorAudioClip(AudioClip audioClip, int frequency, int sampleRate, int sampleLength)
    {
        float[] samples = new float[sampleLength];
        audioClip.GetData(samples, 0);

        //create 
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = GenerateAudioFrame(frequency    , sampleRate, i) + 
                         GenerateAudioFrame(frequency / 2, sampleRate, i) + 
                         GenerateAudioFrame(frequency / 3, sampleRate, i);

            samples[i] *= 0.3f;
        }

        audioClip.SetData(samples, 0);
        return audioClip;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="frequency">frequency of the sound generated</param>
    /// <param name="sampleRate"> how long the sound is plaid for </param>
    /// <param name="i"></param>
    /// <returns></returns>
    private float GenerateAudioFrame(int frequency, int sampleRate, int i)
    {
        return Mathf.Sin(2* Mathf.PI * frequency * ((float)i / (float)sampleRate) / (3f * Mathf.PI));
    }

} 