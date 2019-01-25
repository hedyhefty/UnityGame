using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcubeRotation : MonoBehaviour 
{
    private float m_TimeLeft;
    private Vector3 m_RanAxis;
    //nblockHealth EcubeHealth;
	// Use this for initialization
	void Start () 
    {
        m_TimeLeft = 0f;
        m_RanAxis = Vector3.zero;
        //EcubeHealth = gameObject.GetComponent<nblockHealth>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(m_TimeLeft<=0)
        {
            m_RanAxis = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), Random.Range(-50f, 50f));
            m_TimeLeft = Random.Range(500f,1000f);
        }
        transform.Rotate(Vector3.up + m_RanAxis, Time.deltaTime * 40);
	}

    private void FixedUpdate()
    {
        if(m_TimeLeft>0)
        {
            m_TimeLeft--;
        }
    }
}
