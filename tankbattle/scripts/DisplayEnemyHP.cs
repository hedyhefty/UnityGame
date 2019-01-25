using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayEnemyHP : MonoBehaviour {

    public Text ai_CurrentHP;

    private eTankHealth tkHP;
    private float ai_tankCurrentHP;
    // Use this for initialization
    void Start()
    {
        tkHP = gameObject.transform.parent.gameObject.GetComponent<eTankHealth>();
        ai_tankCurrentHP = tkHP.GetCurrentHP();
        ai_CurrentHP.text = ai_tankCurrentHP.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        ai_tankCurrentHP = tkHP.GetCurrentHP();
        ai_CurrentHP.text = ai_tankCurrentHP.ToString();
        Vector3 HPposition = Camera.main.WorldToScreenPoint(transform.position - new Vector3(0, 0, 0.2f));
        ai_CurrentHP.transform.position = HPposition;
    }
}
