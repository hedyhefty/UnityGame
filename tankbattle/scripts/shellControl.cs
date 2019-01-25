using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shellControl : MonoBehaviour 
{
    public float m_Damage = 10f;
    public float m_MaxLifeTime = 2f;


	// Use this for initialization
	void Start () 
    {
        Destroy(gameObject, m_MaxLifeTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        //nblockHealth targetnBlockHealth = other.GetComponent<nblockHealth>();
        //targetnBlockHealth.TakeDamage(m_Damage);
        //Destroy(other.gameObject);

        //get target layer
        int blockMask = (int)Mathf.Floor(Mathf.Log(LayerMask.GetMask("NormalBlockLayer"), 2f));
        int playerMask = (int)Mathf.Floor(Mathf.Log(LayerMask.GetMask("PlayerLayer"), 2f));
        int enemyMask = (int)Mathf.Floor(Mathf.Log(LayerMask.GetMask("EnemyLayer"), 2f));
        int EcubeMask = (int)Mathf.Floor(Mathf.Log(LayerMask.GetMask("EnergyCubeLayer"), 2f));


        if (other.gameObject.layer == blockMask) 
        {
            nblockHealth blockHealth = other.gameObject.GetComponent<nblockHealth>();
            blockHealth.TakeDamage(m_Damage);
            Destroy(gameObject);
        }

        if(other.gameObject.layer==enemyMask)
        {
            eTankHealth etkHealth = other.gameObject.GetComponent<eTankHealth>();
            etkHealth.TakeDamage(m_Damage);
            Destroy(gameObject);
        }

        if(other.gameObject.CompareTag("Player"))
        {
            tankHealth tkHealth = other.gameObject.GetComponent<tankHealth>();
            tkHealth.TakeDamage(m_Damage);
            Destroy(gameObject);
        }

        if(other.gameObject.layer==EcubeMask)
        {
            nblockHealth blockHealth = other.gameObject.GetComponent<nblockHealth>();
            blockHealth.TakeDamage(m_Damage);
            Destroy(gameObject);
        }

        if(other.gameObject.layer==2&&other.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }

    }

}
