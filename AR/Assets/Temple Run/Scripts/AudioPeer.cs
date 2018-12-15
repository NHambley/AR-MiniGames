using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{

    /// <summary>
    /// Separating out the sound samples into four groups
    /// - Low: 0 - 15
    /// - Low Medium: 16 - 31
    /// - High Medium: 32 - 47
    /// - High: 48 - 63
    ///     public float[] low = new float[16];
    ///     public float[] lowM = new float[16];
    ///     public float[] highM = new float[16];
    ///     public float[] high = new float[16];
    /// **most likely not using this anymore
    /// </summary>
    



    AudioSource audioSource;
    public float[] samples = new float[512];
    public float[] freqBand = new float[8];
    public float[] bandBuffer = new float[8];
    float[] bufferDecrease = new float[8];


    float[] spectrum = new float[64];

    // Use this for initialization
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
	}


    void GetSpectrumAudioSource()
    {
        // retrieve the spectrum data from the music
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);

       
    }

    void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if(freqBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = freqBand[i];
                bufferDecrease[i] = 0.005f;
            }

            if(freqBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= freqBand[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }
    void MakeFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if(i == 7)
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;
            freqBand[i] = average * 10;
        }
    }
}
//audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
// fill the four audio level arrays with the data
/*
for (int i = 0; i < samples.Length; i++)
{
    if(i < 16)// low 0-15
    {
        low[i] = samples[i];
    }
    else if (i >= 16 && i < 32)// low medium 16-31
    {
        lowM[i - 16] = samples[i];
    }
    else if (i >= 32 && i < 48)// high medium 32-47
    {
        highM[i - 32] = samples[i];
    }
    else if (i >= 48 && i < 64)// high 48-63
    {
        high[i - 48] = samples[i];
    }
}
*/
