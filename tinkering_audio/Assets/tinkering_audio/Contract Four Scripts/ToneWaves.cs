using UnityEngine;

//-----------------------------------------------------------------------
// <copyright file="ToneWaves.cs">
// MIT License Copyright (c) 2019.
// </copyright>
// <author>Christopher Philip Robertson</author>
// <summary>
// Handles the creation of different types of waves as well as altering
// current waves.
// </summary>
//----

public class ToneWaves : MonoBehaviour
{
    [Header("Random Sound")]
    [SerializeField]
    private Sound randomSoundSettings;

    [Header("Perlin Noise")]
    [SerializeField]
    private int perlinNoiseHeight = 256;
    [SerializeField]
    private int perlinNoiseWidth = 256;
    [SerializeField]
    private float perlinNoiseScale = 20f;
    private Vector2 perlinNoiseOffset;

    #region Singleton Instance
    public static ToneWaves Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    #endregion

    /// <summary>
    /// Used in conjunction with the enums to refactor an audioclip automatically 
    /// with the new wave type rather than me manually assigning a function to each one
    /// </summary>
    /// <param name="soundSetting"></param>
    public void RefactorAudioClipWave(Sound soundSetting)
    {
        if (soundSetting.waveType == WaveType.STATIC) return;

        if (soundSetting.waveType == WaveType.SINE)
        {
            soundSetting.audioClip = ConvertClipToSquareWave(soundSetting);
        }

        if (soundSetting.waveType == WaveType.PERLIN_NOISE)
        {
            soundSetting.audioClip = ConvertClipToPerlinNoise(soundSetting);
        }
    }

    #region Sine Wave
    /// <summary>
    ///  A sine function that produces a sine wave based on
    ///  a variety of variables
    /// </summary>
    /// <param name="frequency"></param>
    /// <param name="indexPosition"></param>
    /// <param name="sampleRate"></param>
    /// <returns>
    /// A float that represents a point on a wave
    /// </returns>
    public float GetSinValue(float frequency, int indexPosition, float sampleRate)
    {
        return Mathf.Sin(2.0f * Mathf.PI * frequency * (indexPosition / sampleRate));
    }
    #endregion

    #region Square Wave
    /// <summary>
    /// Produces a square wave which provides a more unique
    /// sound to that of say a sine wave
    /// </summary>
    /// <param name="frequency"></param>
    /// <param name="indexPosition"></param>
    /// <param name="sampleRate"></param>
    /// <returns>
    /// Returns a float which is either a 1
    /// or a -1 which creates the square sound
    /// </returns>
    public float GetSquareSinValue(float frequency, int indexPosition, float sampleRate)
    {
        if (Mathf.Sin(2.0f * Mathf.PI * frequency * (indexPosition / sampleRate)) > 0)
        {
            return 1;
        }

        else
        {
            return -1;
        }
    }

    /// <summary>
    /// Converts any audioclip samples provided to become a square wave
    /// with samples either being 1 or -1
    /// </summary>
    /// <param name="soundSettings"></param>
    /// <remarks>
    /// Not to be confused with GetSquareSinValue(...) which instead provides
    /// the value directly rather than going through the entire list
    /// </remarks>
    /// <returns>
    /// Returns a list of floats to replace the samples that were 
    /// just adjusted
    /// </returns>
    public float[] ConvertWaveToSquareWave(Sound soundSettings)
    {
        for (int i = 0; i < soundSettings.samples.Length - 1; i++)
        {
            if (soundSettings.samples[i] > 0)
            {
                soundSettings.samples[i] = 1;
            }

            else
            {
                soundSettings.samples[i] = -1;
            }
        }

        return soundSettings.samples;
    }

    /// <summary>
    /// Converts any audioclip samples provided to become a square wave
    /// with samples either being 1 or -1 
    /// </summary>
    /// <param name="soundSettings"></param>
    /// <remarks>
    /// This is separate from ConvertWaveToSquareWaves(...) since it returns
    /// an audioclip rather than the samples.
    /// </remarks>
    /// <returns>
    /// An audioclip file
    /// </returns>
    public AudioClip ConvertClipToSquareWave(Sound soundSettings)
    {
        for (int i = 0; i < soundSettings.samples.Length - 1; i++)
        {
            if (soundSettings.samples[i] > 0)
            {
                soundSettings.samples[i] = 1;
            }

            else
            {
                soundSettings.samples[i] = -1;
            }
        }

        soundSettings.audioClip.SetData(soundSettings.samples, 0);
        return soundSettings.audioClip;
    }


    #endregion

    #region Perlin Noise Converter
    public AudioClip ConvertClipToPerlinNoise(Sound soundSettings)
    {
        float[] samples = new float[soundSettings.sampleLength];
        perlinNoiseOffset = GetRandomOffset();

        int index = 0;
        for (int x = 0; x < soundSettings.sampleLength; x++)
        {
            for (int y = 0; y < soundSettings.sampleLength; y++)
            {
                if (index > soundSettings.sampleLength - 1)
                {
                    soundSettings.audioClip.SetData(soundSettings.samples, 0);
                    return soundSettings.audioClip;
                }

                soundSettings.samples[index] = GetRandomNoiseValue(x, y);
                index++;
            }
        }

        soundSettings.waveType = WaveType.PERLIN_NOISE;
        soundSettings.audioClip.SetData(soundSettings.samples, 0);
        return soundSettings.audioClip;
    }

    /// <summary>
    /// Returns a random offset value to be used in the perlin noise generation
    /// to ensure that it is random each time it is played
    /// </summary>
    /// <remarks>
    /// Sourced by https://www.youtube.com/watch?v=bG0uEXV6aHQ
    /// </remarks>
    /// <returns>
    /// Returns a Vector2 which holds the X and Y offset values
    /// </returns>
    private Vector2 GetRandomOffset()
    {
        Vector2 offset = Vector2.zero;
        offset.x = Random.Range(0, 9999f);
        offset.y = Random.Range(0, 9999f);
        return offset;
    }


    /// <summary>
    /// Uses an X and Y determine a perlin noise value at a given coordinate 
    /// which I then later assign to the samples
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <remarks>
    /// Sourced by https://www.youtube.com/watch?v=bG0uEXV6aHQ
    /// </remarks>
    /// <returns>
    /// Returns a perlin noise value depending on the colour of the position
    /// of the coordinate
    /// </returns>
    private float GetRandomNoiseValue(int x, int y)
    {
        float xCoord = (float)x / perlinNoiseWidth * perlinNoiseScale + perlinNoiseOffset.x;
        float yCoord = (float)y / perlinNoiseHeight * perlinNoiseScale + perlinNoiseOffset.y;

        float perlinSample = Mathf.PerlinNoise(xCoord, yCoord);
        return perlinSample;
    }

    #endregion

    #region Triangle Wave
    //https://www.fxsolver.com/browse/formulas/Sawtooth+wave
    //https://www.fxsolver.com/browse/formulas/Triangle+wave+%28in+trigonometric+terms%29
    #endregion
}
