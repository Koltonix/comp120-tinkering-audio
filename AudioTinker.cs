using System.Collections;
using System.Collections.Generic;
//using NaughtyAttributes;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class AudioTinker : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip outAudioClip;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        outAudioClip = CreateToneAudioClip(5000);
        audioSource.clip = outAudioClip;
        PlayOutAudio();
        SaveWavFile();

    }
    public void PlayOutAudio()
    {
        audioSource.Play();
    }

    // Private 
    private AudioClip CreateToneAudioClip(int frequency)
    {
        int sampleDurationSecs = 1;
        int sampleRate = 1000;
        int sampleLength = sampleRate * sampleDurationSecs;
        float maxValue = -2f / 2f;

        var audioClip = AudioClip.Create("Sound", sampleLength, 1, sampleRate, false);

        float[] samples = new float[sampleLength];
        for (var i = 0; i < sampleLength; i++)

        {
            float s = GenerateAudioFrame(frequency, sampleRate, i);
            float v = s * maxValue;
            samples[i] = v;
        }

        audioClip.SetData(samples, 0);
        return audioClip;
    }

    //create wave behaviour 
    private static float GenerateAudioFrame(int frequency, int sampleRate, int i)
    {

        float output = Mathf.Sin(2 * Mathf.PI * frequency * ((float)i / (float)sampleRate)) / (3 * Mathf.PI);

        //float output = (4 * Mathf.Sin(30)) / (3 * Mathf.PI);

        //if (output < 0) { return -1; } else { return 1; }

        return output;

        /*  formula that i need to created the sound -> (4sin3θ) / pi

         -Sintassi
         float amplitude = 0.25f;
         float frequency = 1000;

         for (int n = 0; n < waveFileWriter.WaveFormat.SampleRate; n++)
         {
             float sample = (float)(amplitude * Math.Sin((2 * Math.PI * n * frequency) / waveFileWriter.WaveFormat.SampleRate));
             waveFileWriter.WriteSample(sample);
         }
         */

    }


    //save files
    private void SaveWavFile()
    {
        var audioClip = CreateToneAudioClip(5500);
        string[] filePaths = Directory.GetFiles(@"C:\Users\Ludovico Bitti\Desktop\UNI\Comp120 (programming)\2. TA\Git\ACM-COMP120-Tinker-Audio-Template", "*.wav");
        int BigNumber = 0;
        int AddNumber = 1;
        foreach (string path in filePaths)
        {
            int number = (int)path[path.Length + -5] - 48;
            BigNumber = Mathf.Max(BigNumber, number);

        }
        SaveWavUtil.Save("C:\\Users\\Ludovico Bitti\\Desktop\\UNI\\Comp120 (programming)\\2. TA\\Git\\ACM-COMP120-Tinker-Audio-Template\\Sound" + (BigNumber + AddNumber) + ".wav", audioClip);
    }

}
