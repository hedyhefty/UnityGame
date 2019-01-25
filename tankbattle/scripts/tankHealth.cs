using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankHealth : MonoBehaviour 
{
    public float m_StartingHealth = 30f;

    private float m_CurrentHealth;
    private bool m_Exploded;

	// Use this for initialization
	void Start () 
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
        if(m_CurrentHealth>=m_StartingHealth)
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
        gameObject.transform.position = new Vector3(100, 100, 100);
        gameObject.SetActive(false);
    }

    public float GetCurrentHP()
    {
        return m_CurrentHealth;
    }
}
