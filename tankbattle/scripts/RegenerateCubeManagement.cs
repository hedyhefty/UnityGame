using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerateCubeManagement : MonoBehaviour 
{
    public float m_RegenerateAmount=20f;

    private void OnTriggerEnter(Collider other)
    {
        int playerMask = (int)Mathf.Floor(Mathf.Log(LayerMask.GetMask("PlayerLayer"), 2f));
        if (other.gameObject.CompareTag("Player"))
        {
            tankHealth tkHealth = other.gameObject.GetComponent<tankHealth>();
            tkHealth.RegenerateHealth(m_RegenerateAmount);
            Destroy(gameObject);
        }
    }
}
