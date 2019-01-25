using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHP : MonoBehaviour 
{
    public Text m_CurrentHP;

    private tankHealth tkHP;
    private float m_tankCurrentHP;
	// Use this for initialization
	void Start () 
    {
        tkHP = gameObject.transform.parent.gameObject.GetComponent<tankHealth>();
        m_tankCurrentHP = tkHP.GetCurrentHP();
        m_CurrentHP.text = m_tankCurrentHP.ToString();
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_tankCurrentHP = tkHP.GetCurrentHP();
        m_CurrentHP.text = m_tankCurrentHP.ToString();
        Vector3 HPposition = Camera.main.WorldToScreenPoint(transform.position - new Vector3(0, 0, 0.2f));
        m_CurrentHP.transform.position = HPposition;
	}
}
