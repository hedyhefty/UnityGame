using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayShootClip : MonoBehaviour {

    private AudioClip ac;
    private AudioSource source;
    private int count;

    // Use this for initialization
    private void Start()
    {
        count = 0;
        int sampleFreq = 44100;
        float frequency = 330;

        float[] samples = new float[44100];
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = DecreaseingFactor(i,sampleFreq) * Mathf.Sin(Mathf.PI * 2 * i * frequency / sampleFreq);
        }
        ac = AudioClip.Create("Test", samples.Length, 1, sampleFreq, false);
        ac.SetData(samples, 0);
        source = gameObject.AddComponent<AudioSource>();
        Debug.Log(ac.length);
        source.PlayOneShot(ac, 1f);
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void FixedUpdate()
    {

    }

    float DecreaseingFactor(long iter,long fre)
    {
        float n_iter = (float)iter;
        float n_fre = (float)fre;
        float factor = ((-6f * n_iter * n_iter) + n_fre * n_fre) / (n_fre * n_fre);
        if(factor<Mathf.Epsilon)
        {
            return 0;
        }
        //Debug.Log(factor);
        return factor;
    }

}
