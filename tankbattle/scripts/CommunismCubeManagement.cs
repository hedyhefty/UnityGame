using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunismCubeManagement : MonoBehaviour 
{
    private void OnTriggerEnter(Collider other)
    {
        int playerMask = (int)Mathf.Floor(Mathf.Log(LayerMask.GetMask("PlayerLayer"), 2f));
        if(other.gameObject.CompareTag("Player"))
        {
            tankBuffControl tbControl = other.gameObject.GetComponent<tankBuffControl>();
            tbControl.CommunismEnhancement();
            Destroy(gameObject);
        }
    }
}
