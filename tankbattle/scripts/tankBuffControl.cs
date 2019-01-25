using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankBuffControl : MonoBehaviour 
{
    public int m_PlayerNumber = 1;
    public Transform m_Tank;
    public GameObject m_Chassis;
    public GameObject m_Turret;
    public GameObject m_Barrel;
    public Material m_OriginalMaterial;
    public Material m_CloakMaterial;
    public float m_CMAddAmount = 20f;
    public float m_APFSDSLunchForce = 30f;
    public float m_CMSpeed = 8f;
    public float m_CloakAlpha = 25f;
    public float m_CloakTime = 5f;
    public float m_CMIncreaseMass = 2f;
    private GameObject m_SecondBarrel;
    private GameObject[] e_Tanks;
    private bool m_IsCommunismed;

    private void Start()
    {
        m_IsCommunismed = false;
    }

    public void CommunismEnhancement()
    {
        if(!m_IsCommunismed)
        {
            m_Chassis.transform.localScale += new Vector3(0.6f, 0, 0.4f);
            m_Turret.transform.localScale += new Vector3(0.4f, 0, 0.4f);
            m_Barrel.transform.localPosition += new Vector3(0.35f, 0, 0.2f);
            m_SecondBarrel = Instantiate(m_Barrel, m_Tank) as GameObject;
            m_SecondBarrel.transform.localPosition = m_Barrel.transform.localPosition - new Vector3(0.7f, 0, 0);
            tankShooting TKScript = gameObject.GetComponent<tankShooting>();
            TKScript.SetCommunismTrue();

            tankControl TKCcontroller = gameObject.GetComponent<tankControl>();
            TKCcontroller.SetSpeed(m_CMSpeed);

            tankHealth THScript = gameObject.GetComponent<tankHealth>();
            THScript.AddHealth(m_CMAddAmount);
            Rigidbody m_rigidbody = GetComponent<Rigidbody>();
            m_rigidbody.mass += m_CMIncreaseMass;

            m_IsCommunismed = true;
        }
        else
        {
            tankHealth tkHp = gameObject.GetComponent<tankHealth>();
            tkHp.RegenerateHealth(20f);
        }
    }

    public void APFSDSEnhancement()
    {
        tankShooting TKScript = gameObject.GetComponent<tankShooting>();
        TKScript.SetShellAPFSDS(m_APFSDSLunchForce);
    }

    public IEnumerator CloakEngage()
    {
        SetInvisible(m_Chassis);
        SetInvisible(m_Turret);
        SetInvisible(m_Barrel);
        if (m_SecondBarrel != null)
        {
            SetInvisible(m_SecondBarrel);
        }
        //DecreaseEnemyRange();
        SetIsCloaking();
        gameObject.layer = 2;
        //Debug.Log("layer: " + gameObject.layer);

        yield return new WaitForSeconds(m_CloakTime);

        SetVisible(m_Chassis);
        SetVisible(m_Turret);
        SetVisible(m_Barrel);
        if (m_SecondBarrel != null)
        {
            SetVisible(m_SecondBarrel);
        }
        //IncreaseEnemyRange();
        SetNotCloaking();
        gameObject.layer = 9;
        //Debug.Log("layer: " + gameObject.layer);
    }

    public void SetInvisible(GameObject gObj)
    {
        MeshRenderer m_Rend = gObj.GetComponent<MeshRenderer>();
        Material[] materials = m_Rend.materials;
        materials[0] = m_CloakMaterial;
        m_Rend.materials = materials;
    }

    public void SetVisible(GameObject gObj)
    {
        MeshRenderer m_Rend = gObj.GetComponent<MeshRenderer>();
        Material[] materials = m_Rend.materials;
        materials[0] = m_OriginalMaterial;
        m_Rend.materials = materials;
    }

    void SetIsCloaking()
    {
        e_Tanks = GameObject.FindGameObjectsWithTag("Enemy");
        if (e_Tanks != null)
        {
            foreach(GameObject e_Tank in e_Tanks)
            {
                AIControlMP aIControl = e_Tank.GetComponent<AIControlMP>();
                aIControl.Cloaking(m_PlayerNumber);
            }
        }
    }

    void SetNotCloaking()
    {
        e_Tanks = GameObject.FindGameObjectsWithTag("Enemy");
        if (e_Tanks != null)
        {
            foreach (GameObject e_Tank in e_Tanks)
            {
                AIControlMP aIControl = e_Tank.GetComponent<AIControlMP>();
                aIControl.OutFromCloak(m_PlayerNumber);
            }
        }
    }

    //void DecreaseEnemyRange()
    //{
    //    e_Tanks = GameObject.FindGameObjectsWithTag("Enemy");
    //    Debug.Log(e_Tanks);
    //    if(e_Tanks!=null)
    //    {
    //        foreach(GameObject e_Tank in e_Tanks)
    //        {
    //            AIControl aIControl = e_Tank.GetComponent<AIControl>();
    //            aIControl.CloakPlayerRange();
    //        }
    //    }
    //}

    //void IncreaseEnemyRange()
    //{
    //    e_Tanks = GameObject.FindGameObjectsWithTag("Enemy");
    //    if (e_Tanks != null)
    //    {
    //        foreach (GameObject e_Tank in e_Tanks)
    //        {
    //            AIControl aIControl = e_Tank.GetComponent<AIControl>();
    //            aIControl.VisiblePayerRange();
    //        }
    //    }
    //}
}
