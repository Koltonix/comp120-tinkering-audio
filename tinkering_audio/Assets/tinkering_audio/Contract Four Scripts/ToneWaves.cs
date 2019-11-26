using System.Collections;
using System.Collections.Generic;
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
    #region Singleton Instance
    public static ToneWaves Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    #endregion

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

    #region Triangle Wave
    //https://www.fxsolver.com/browse/formulas/Sawtooth+wave
    //https://www.fxsolver.com/browse/formulas/Triangle+wave+%28in+trigonometric+terms%29
    #endregion
}
