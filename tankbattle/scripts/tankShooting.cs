using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankShooting : MonoBehaviour 
{
    public int m_PlayerNumber = 1;
    //public Rigidbody m_Shell;
    public Rigidbody m_NormalShell;
    public Rigidbody m_APFSDS;
    public Transform m_FireTransform;
    public Transform m_FireTransformL;
    public Transform m_FireTransformR;
    public AudioSource m_ShootingAudio;
    public float m_LunchForce = 20f;
    public float m_TimeInterval = 1f;
    public float m_decreaseFactor = 1f;
    public float m_LRInterval = 0.2f;

    private float m_timeLeft;
    private float m_LRTimeLeft;
    private string m_FireButton;
    private bool m_isCommunismEnhancing;
    private bool m_LeftFired;
    private AudioClip m_ShootingClip;
    private AudioClip m_LShootingClip;

    private Rigidbody m_Shell;

    // Use this for initialization
    void Awake () 
    {
        m_timeLeft = 0;
        m_LRTimeLeft = 0;
        m_FireButton = "Fire" + m_PlayerNumber;
        m_isCommunismEnhancing = false;
        m_LeftFired = false;
        m_Shell = m_NormalShell;
	}

    public void Start()
    {
        InitializeShootingAudio();
    }

    // Update is called once per frame
    void Update ()
    {
        //Debug.Log(m_isCommunismEnhancing);
        if(!m_isCommunismEnhancing)
        {
            if (Input.GetButton(m_FireButton) && m_timeLeft <= 0f)
            {
                Fire();
            }
        }
        else
        {
            if (Input.GetButton(m_FireButton) && m_timeLeft <= 0f)
            {
                CommunismFireL();
            }

            if (m_LeftFired && m_LRTimeLeft <= 0f)
            {
                CommunismFireR();
            }
        }

	}

    private void FixedUpdate()
    {
        if (m_timeLeft > 0)
        {
            m_timeLeft -= m_decreaseFactor * Time.deltaTime;
        }
        if (m_LRTimeLeft > 0)
        {
            m_LRTimeLeft -= m_decreaseFactor * Time.deltaTime;
        }
    }

    private void Fire()
    {
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation * Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody;
        shellInstance.velocity = m_LunchForce * m_FireTransform.forward;
        m_timeLeft = m_TimeInterval;

        m_ShootingAudio.PlayOneShot(m_ShootingClip);
    }

    private void CommunismFireL()
    {
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransformL.position, m_FireTransformL.rotation * Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody;
        shellInstance.velocity = m_LunchForce * m_FireTransformL.forward;
        m_LeftFired = true;
        m_timeLeft = m_TimeInterval;
        m_LRTimeLeft = m_LRInterval;
        m_ShootingAudio.PlayOneShot(m_LShootingClip);
    }

    private void CommunismFireR()
    {
        //m_ShootingAudio.Stop();
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransformR.position, m_FireTransformR.rotation * Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody;
        shellInstance.velocity = m_LunchForce * m_FireTransformR.forward;
        m_LeftFired = false;
        m_ShootingAudio.PlayOneShot(m_ShootingClip);
    }

    public void SetCommunismTrue()
    {
        m_isCommunismEnhancing = true;
    }

    public void SetShellAPFSDS(float lforce)
    {
        m_Shell = m_APFSDS;
        m_LunchForce = lforce;
    }

    private void InitializeShootingAudio()
    {
        int sampleFreq = 44100;
        float frequency = 330;

        float[] samples = new float[44100];
        float[] samplesShot = new float[44100];
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = DecreaseingFactor(i, sampleFreq, 6f) * Mathf.Sin(Mathf.PI * 2 * i * frequency / sampleFreq);
            samplesShot[i] = DecreaseingFactor(i, sampleFreq, 12f) * Mathf.Sin(Mathf.PI * 2 * i * frequency / sampleFreq);
        }
        m_ShootingClip = AudioClip.Create("Test", samples.Length, 1, sampleFreq, false);
        m_LShootingClip = AudioClip.Create("LShooting", samplesShot.Length/4, 1, sampleFreq, false);
        m_ShootingClip.SetData(samples, 0);
        m_LShootingClip.SetData(samplesShot, 0);
    }

    private float DecreaseingFactor(long iter, long fre, float rate)
    {
        float n_iter = (float)iter;
        float n_fre = (float)fre;
        float factor = ((-rate * n_iter * n_iter) + n_fre * n_fre) / (n_fre * n_fre);
        if (factor < Mathf.Epsilon)
        {
            return 0;
        }
        //Debug.Log(factor);
        return factor;
    }
}
