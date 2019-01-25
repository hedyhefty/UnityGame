using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nblockHealth : MonoBehaviour
{
    public float m_StartingHealth = 30f;
    public float m_ShakeAmount = 0.2f;
    public float m_ShakeTime = 0.15f;
    public float m_DecreaseFactor = 1f;
    public float m_CPpro = 0.03f;
    public float m_AFpro = 0.05f;
    public float m_RGpro = 0.1f;
    public GameObject CPowerCube;
    public GameObject RegenerateCube;
    public GameObject APFSDSCube;

    private float m_CurrentHealth;
    private bool m_Exploded;
    private Vector3 m_Position;
    private bool m_TakingDamage;


    // Use this for initialization
    void Start()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Exploded = false;
        m_Position = transform.position;
        m_TakingDamage = false;
    }

    public void TakeDamage(float amount)
    {
        m_CurrentHealth -= amount;
        m_TakingDamage = true;

        if (m_CurrentHealth <= 0f && !m_Exploded)
        {
            OnExplode();
        }
    }

    private void Update()
    {
        if (m_TakingDamage == true)
        {
            OnShake();
        }
    }

    private void OnShake()
    {
        transform.position = m_Position + Random.insideUnitSphere * Random.Range(0f, m_ShakeAmount);
        m_ShakeTime -= m_DecreaseFactor * Time.deltaTime;

        if (m_ShakeTime < 0)
        {
            m_TakingDamage = false;
            m_ShakeTime = Random.Range(0.1f, 0.2f);
            transform.position = m_Position;
        }
    }

    private void OnExplode()
    {
        float wheel = Random.Range(0f, 1f);
        if (wheel < m_CPpro)
        {
            GameObject CmpowerCube = Instantiate(CPowerCube, transform.position, transform.rotation) as GameObject;
        }
        else if (wheel < m_AFpro+m_CPpro)
        {
            GameObject apfCube = Instantiate(APFSDSCube, transform.position, transform.rotation) as GameObject;
        }
        else if (wheel < m_AFpro+m_CPpro+m_RGpro)
        {
            GameObject regCube = Instantiate(RegenerateCube, transform.position, transform.rotation) as GameObject;
        }

        m_Exploded = true;
        Destroy(gameObject);
    }
}
