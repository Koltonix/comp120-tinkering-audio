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
// <summary>
// <this program takes care of 
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
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            carAudioClip = CreateAudioToneClip(250);
            carAudioClip = RefactorAudioClip(carAudioClip,frequency: 2500,sampleRate: 40100, sampleLength: 40100*2);
            audioSource.PlayOneShot(carAudioClip);
            SaveWavFile(carAudioClip);
        }
    }

    public AudioClip CreateAudioToneClip(int frequency)
    {
        int sampleDurationSecs = 2;
        int sampleRate = 40100;
        int sampleLength = sampleRate * sampleDurationSecs;
        float maxValue = 1f;

        var audioClip = AudioClip.Create("Sound", sampleLength, 1, sampleRate, false);

        return audioClip;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="frequency"></param>
    /// <param name="sampleRate"></param>
    /// <param name="sampleLength"></param>
    /// <returns></returns>
    /// <remarks>
    /// Christopher pair programmed this by helping me understand refactoring, he also suggested to use the for loop.
    /// </remarks>
    private AudioClip RefactorAudioClip(AudioClip audioClip, int frequency, int sampleRate, int sampleLength)
    {
        float[] samples = new float[sampleLength];
        audioClip.GetData(samples, 0);

        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = GenerateAudioFrame(frequency, sampleRate, i);
            samples[i] += GenerateAudioFrame(frequency/2, sampleRate, i);
            samples[i] += GenerateAudioFrame(frequency / 3, sampleRate, i);

            samples[i] = samples[i] * 0.3f;
            //can put if statment
        }

        audioClip.SetData(samples, 0);
        return audioClip;
    }

    // Start is called before the first frame update

    private static float GenerateAudioFrame(int frequency, int sampleRate, int i)
    {

        float output = Mathf.Sin(2* Mathf.PI * frequency * ((float)i / (float)sampleRate) / (3f * Mathf.PI));
        
        return output;

    }

    public void SaveWavFile(AudioClip audioClip)
    {
        SaveWavUtil.Save("C:\\Users\\Ludovico Bitti\\Desktop\\SoundCoin.wav", audioClip);
    }

    

} 