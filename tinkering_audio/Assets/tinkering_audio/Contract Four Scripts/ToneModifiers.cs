using UnityEngine;

//-----------------------------------------------------------------------
// <copyright file="ToneModifiers.cs">
// MIT License Copyright (c) 2019.
// </copyright>
// <author>Christopher Philip Robertson</author>
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
    public Sound MultiplyAudioClips(Sound[] sounds)
    {
        Sound combinedSoundSettings = GetLargestSoundValues(sounds);
        combinedSoundSettings.audioClip = ToneGenerator.Instance.CreateToneAudioClip(combinedSoundSettings);
        return combinedSoundSettings;
    }

    private Sound GetLargestSoundValues(Sound[] soundSettings)
    {
        Sound newSettings = new Sound();
        newSettings.waveType = WaveType.Dynamic;

        foreach(Sound setting in soundSettings)
        {
            if (setting.frequency > newSettings.frequency) newSettings.frequency = setting.frequency;

            if (setting.sampleLength > newSettings.sampleLength)
            {
                newSettings.sampleLength = setting.sampleLength;
                newSettings.samples = new float[newSettings.sampleLength];
            }

            if (setting.sampleRate > newSettings.sampleRate) newSettings.sampleRate = setting.sampleRate; 

            if (setting.sampleDurationSecs > newSettings.sampleDurationSecs) newSettings.sampleDurationSecs = setting.sampleDurationSecs;
        }

        return newSettings;
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
