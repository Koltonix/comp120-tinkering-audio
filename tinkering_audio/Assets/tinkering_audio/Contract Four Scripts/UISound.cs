using UnityEngine;
using UnityEngine.UI;
using TMPro;

//-----------------------------------------------------------------------
// <copyright file="UISound.cs">
// MIT License Copyright (c) 2019.
// </copyright>
// <author>Christopher Philip Robertson</author>
// <summary>
// Handles the UI behaviour and input of the user to generate sounds
// </summary>
//----

public class UISound : MonoBehaviour
{
    [SerializeField]
    private Sound currentSoundSettings;
    private float clipVolume;
    private AudioSource audioSource;

    [Header("UI References")]
    [SerializeField]
    private Slider frequencySlider;
    [SerializeField]
    private TextMeshProUGUI frequencyText;
    [Space]
    [SerializeField]
    private Slider sampleRateSlider;
    [SerializeField]
    private TextMeshProUGUI sampleRateText;
    [Space]
    [SerializeField]
    private Slider durationSlider;
    [SerializeField]
    private TextMeshProUGUI durationText;
    [Space]
    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private TextMeshProUGUI volumeText;
    [SerializeField]
    private TMP_Dropdown waveDropDown;

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

    public void GenerateSoundClip()
    {
        currentSoundSettings.audioClip = ToneGenerator.Instance.CreateToneAudioClip(currentSoundSettings);
        ChangeAudioClipWave();
        currentSoundSettings.audioClip = ToneModifiers.Instance.ChangeVolume(currentSoundSettings.audioClip, clipVolume);
    }

    public void SaveAudioClip()
    {
        GenerateSoundClip();
        SaveWavUtil.Save("new_audio_clip", currentSoundSettings.audioClip);
        print(Application.dataPath + "/Generated Clips");
    }

    private void ChangeAudioClipWave()
    {
        if (currentSoundSettings.waveType == WaveType.SINE)
        {
            //Returns since all default waves are Sine waves in this instance
            return;
        }

        ToneWaves.Instance.RefactorAudioClipWave(currentSoundSettings);
    }

    #region UI Functions to Invoke
    public void PlaySound()
    {
        GenerateSoundClip();
        audioSource.PlayOneShot(currentSoundSettings.audioClip);
    }

    public void ChangeFrequency()
    {
        currentSoundSettings.frequency = frequencySlider.value;
        frequencyText.text = "Frequency\n" + currentSoundSettings.frequency;
    }

    public void ChangeSampleRate()
    {
        currentSoundSettings.sampleRate = Mathf.RoundToInt(sampleRateSlider.value);
        sampleRateText.text = "Sample Rate\n" + currentSoundSettings.sampleRate;
    }

    public void ChangeDuration()
    {
        currentSoundSettings.sampleDurationSecs = (float)System.Math.Round(durationSlider.value, 2);
        durationText.text = "Duration\n" + currentSoundSettings.sampleDurationSecs;
    }

    public void ChangeVolume()
    {
        clipVolume = (float)System.Math.Round(volumeSlider.value, 1);
        volumeText.text = "Volume\n" + clipVolume;
    }

    /// <summary>
    /// A function to be used by the UI which changes the current
    /// sound setting to either be a particular type of wave
    /// </summary>
    /// <param name="waveID"></param>
    /// <remarks>
    /// Has to be an int paramteter since Unity does not support
    /// enums to be used in their delegate system
    /// </remarks>
    public void ChangeWave()
    {
        currentSoundSettings.waveType = (WaveType)waveDropDown.value;
    }

    #endregion
}
