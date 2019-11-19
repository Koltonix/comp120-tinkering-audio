using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float GetSinValue(float frequency, float indexPosition, float sampleRate)
    {
        return Mathf.Sin(2.0f * Mathf.PI * frequency * (indexPosition / sampleRate));
    }
    #endregion

    #region Square Wave
    public float GetSquareSinValue(float frequency, float indexPosition, float sampleRate)
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


    #endregion
}
