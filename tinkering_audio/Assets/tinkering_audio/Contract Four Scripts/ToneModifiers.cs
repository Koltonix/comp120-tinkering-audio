using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------------------------------------
// <copyright file="ToneModifiers.cs">
// MIT License Copyright (c) 2019.
// </copyright>
// <author>Christopher Philip Robertson</author>
// <author>Ludovico Bitti</author>
// <summary>
// Handles the modificatino of the samples and sound clips to provide a new
// and unique sound.
// </summary>
//----

public class ToneModifiers : MonoBehaviour
{
    #region Singleton Instance
    public static ToneModifiers Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    #endregion

    #region Volume
    /// <summary>
    /// Uses a list of float samples that change the volume of the wave
    /// </summary>
    /// <param name="samples"></param>
    /// <param name="amplitude"></param>
    /// <returns>
    /// A list of floats that represent the samples in an AudioClip
    /// </returns>
    public float[] ChangeVolume(float[] samples, float amplitude)
    {
        for (int i = 0; i < samples.Length - 1; i++)
        {
            samples[i] *= amplitude;
        }

        return samples;
    }

    /// <summary>
    /// Uses an audio clip to get the samples if the samples are not available elsewhere
    /// </summary>
    /// <param name="audioClip"></param>
    /// <remarks>
    /// https://docs.unity3d.com/ScriptReference/AudioClip.GetData.html
    /// Documentation used to be able to access the data from the AudioClip
    /// </remarks>
    /// <returns>
    /// An updated AudioClip with the changed volume
    /// </returns>
    public AudioClip ChangeVolume(AudioClip audioClip, float amplitude)
    {
        float[] samples = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(samples, 0);

        for (int i = 0; i < samples.Length - 1; i++)
        {
            samples[i] = samples[i] * amplitude;
        }

        audioClip.SetData(samples, 0);
        return audioClip;
    }

    #endregion

    #region Multiplying Audio
    /// <summary>
    /// Uses the samples from a variety of audioclips to produce a combination
    /// of all of the sounds
    /// </summary>
    /// <param name="sounds"></param>
    /// <remarks>
    /// This can be later used in an echo sort of method where I call this in
    /// conjunction with a clip slicer
    /// </remarks>
    /// <returns>
    /// Returns a new instance of the Sound class with all of the associated
    /// varibles assigned to it
    /// </returns>
    public Sound MultiplyAudioClips(Sound[] sounds)
    {
        List<float> addedSamples = new List<float>();
        Sound combinedSettings = new Sound();
        combinedSettings.waveType = WaveType.DYNAMIC;

        for (int i = 0; i < sounds.Length; i++)
        {
            combinedSettings.frequency += sounds[i].frequency;

            for (int j = 0; j < sounds[i].samples.Length; j++)
            {
                if (addedSamples.Count - 1 < j)
                {
                    addedSamples.Add(sounds[i].samples[j]);
                }

                else
                {
                    addedSamples[j] += sounds[i].samples[j];
                }
            }
        }

        combinedSettings.sampleDurationSecs = GetLongestSoundDuration(sounds);
        combinedSettings.samples = addedSamples.ToArray();

        combinedSettings.sampleLength = Mathf.RoundToInt(combinedSettings.samples.Length * combinedSettings.sampleDurationSecs);
        combinedSettings.sampleRate = sounds[0].sampleRate;

        combinedSettings.audioClip = AudioClip.Create("multiplied_tone", combinedSettings.sampleLength, 1, combinedSettings.sampleRate, false);

        ToneGenerator.Instance.RefactorSamplesInClip(combinedSettings);

        return combinedSettings;
    }


    /// <summary>
    /// Used to compared all of the sample rates of the sounds to combine
    /// and chooses the largest to be used on the new sound.
    /// </summary>
    /// <param name="sounds"></param>
    /// <returns>
    /// Return an integer that is the largest sample rate out of all of the
    /// sounds compared with
    /// </returns>
    private int GetLargestSoundSampleRate(Sound[] sounds)
    {
        int largestSampleRateLength = 0;
        foreach(Sound setting in sounds)
        {
            if (setting.samples.Length > largestSampleRateLength)
            {
                largestSampleRateLength = setting.samples.Length;
            }
        }

        return largestSampleRateLength;
    }

    /// <summary>
    /// Used to get the longest sound duration period since when I'm adding
    /// the clips together I want the longest length possible to avoid any 
    /// sort of data loss.
    /// </summary>
    /// <param name="sounds"></param>
    /// <returns>
    /// A float that is the longest amount of time of any of the audioclips
    /// </returns>
    private float GetLongestSoundDuration(Sound[] sounds)
    {
        float longestSound = 0;
        foreach (Sound setting in sounds)
        {
            if (setting.sampleDurationSecs > longestSound)
            {
                longestSound = setting.sampleDurationSecs;
            }
        }

        return longestSound;
    }

    private float AddFrequencies(Sound[] sounds)
    {
        float frequency = 0;
        foreach(Sound setting in sounds)
        {
            frequency += setting.frequency;
        }

        return frequency;
    }

    private float GetLongestSamplesLength(Sound[] sounds)
    {
        float longestSampleLength = 0;
        foreach (Sound setting in sounds)
        {
            if (setting.sampleLength > longestSampleLength)
            {
                longestSampleLength = setting.sampleDurationSecs;
            }
        }

        return longestSampleLength;
    }

    #endregion

    #region Inserting Audio

    public Sound InsertAudioClip(Sound originalSound, Sound soundToInsert, int insertingPosition)
    {
        Sound[] sounds = { originalSound, soundToInsert };
        Sound newInsertedSound = new Sound();

        //Needs to be rounded to the Ceiling otherwise it might round down and lose sample data
        newInsertedSound.sampleLength = Mathf.CeilToInt((originalSound.sampleDurationSecs * originalSound.sampleRate) + 
                                                        (soundToInsert.sampleDurationSecs * soundToInsert.sampleRate));
        newInsertedSound.samples = new float[newInsertedSound.sampleLength];
        newInsertedSound.frequency = AddFrequencies(sounds);
        newInsertedSound.sampleRate = originalSound.sampleRate;
        newInsertedSound.sampleDurationSecs = originalSound.sampleDurationSecs + soundToInsert.sampleDurationSecs;

        newInsertedSound.audioClip = AudioClip.Create("inserted_tone", newInsertedSound.sampleLength, 1, newInsertedSound.sampleRate, false);

        List<float> samples = ConvertFloatArrayToList(originalSound.samples);
        

        for (int i = 0; i < soundToInsert.samples.Length; i++)
        {
            samples.Insert(insertingPosition, soundToInsert.samples[i]);
        }

        newInsertedSound.samples = samples.ToArray();
        newInsertedSound.audioClip.SetData(newInsertedSound.samples, 0);
       
        return newInsertedSound;
    }

    private List<float> ConvertFloatArrayToList(float[] samples)
    {
        List<float> list = new List<float>();
        for (int i = 0; i < samples.Length; i++)
        {
            list.Add(samples[i]);
        }

        return list;
    }

    #endregion

    #region Pitch
    /// <summary>
    /// Increases the pitch of an audioclip where it uses a modifier
    /// to duplicate each element a certain amount of times
    /// </summary>
    /// <param name="soundSettings"></param>
    /// <param name="tempoModifier"></param>
    /// <remarks>
    /// This will only increase, decreasing will be a separate function.
    /// Note that this will currently keep the maximum amount of samples
    /// and will therefore remove any extras on the end that do not fit
    /// </remarks>
    public void IncreasePitch(Sound soundSettings, int tempoModifier)
    {
        float[] alteredSamples = new float[Mathf.FloorToInt(soundSettings.samples.Length * tempoModifier)];

        int samplesIndex = 0;
        int alteredSamplesIndex = 0;
        while (samplesIndex < alteredSamples.Length - 1)
        {
            if (samplesIndex < soundSettings.samples.Length - 1)
            {
                alteredSamples[alteredSamplesIndex] = soundSettings.samples[samplesIndex];
            }

            if (alteredSamplesIndex % tempoModifier == 0)
            {
                samplesIndex++;
            }

            alteredSamplesIndex++;
        }

        soundSettings.samples = alteredSamples;
    }
    #endregion
}
