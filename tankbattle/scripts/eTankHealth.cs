using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eTankHealth : MonoBehaviour 
{
    public float m_StartingHealth = 30f;
    public GameObject CPowerCube;
    public GameObject RegenerateCube;
    public GameObject APFSDSCube;

    private float m_CurrentHealth;
    private bool m_Exploded;

    // Use this for initialization
    void Start()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Exploded = false;
    }

    public void TakeDamage(float amount)
    {
        m_CurrentHealth -= amount;
        if (m_CurrentHealth <= 0 && !m_Exploded)
        {
            OnExplode();
        }
        //Debug.Log(m_CurrentHealth);
    }

    public void RegenerateHealth(float amount)
    {
        m_CurrentHealth += amount;
        if (m_CurrentHealth >= m_StartingHealth)
        {
            m_CurrentHealth = m_StartingHealth;
        }
        //Debug.Log("currentHP" + m_CurrentHealth);
    }

    public void AddHealth(float amount)
    {
        m_StartingHealth += amount;
        m_CurrentHealth += amount;
        //Debug.Log(m_CurrentHealth);
    }

    void OnExplode()
    {
        m_Exploded = true;
        float wheel = Random.Range(0f, 1f);
        if (wheel < 0.15f)
        {
            GameObject CmpowerCube = Instantiate(CPowerCube, transform.position, transform.rotation) as GameObject;
        }
        else if (wheel < 0.3f)
        {
            GameObject apfCube = Instantiate(APFSDSCube, transform.position, transform.rotation) as GameObject;
        }
        else if (wheel < 0.5f)
        {
            GameObject regCube = Instantiate(RegenerateCube, transform.position, transform.rotation) as GameObject;
        }
        Destroy(gameObject);
    }

    public float GetCurrentHP()
    {
        return m_CurrentHealth;
    }
}
