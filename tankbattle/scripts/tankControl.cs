using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankControl : MonoBehaviour 
{
    public int m_Playernumber = 1;
    public float m_Speed = 12f;
    public float m_CloakEngageCoolDown = 10f;
    public float m_DecreaseFactor = 1f;

    private string m_UDAxisName;
    private string m_LRAxisName;
    private float m_UDInputValue;
    private float m_LRInputValue;
    private string m_CloakButton;

    private Vector3 m_Direction;
    private Vector3 lastPos;
    private Vector3 currentSpeed;

    private float m_CloakCooling;

    private bool UDmoving;
    private bool LRmoving;

    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () 
    {
        m_UDAxisName = "Vertical" + m_Playernumber;
        m_LRAxisName = "Horizontal" + m_Playernumber;
        m_CloakButton = "CloakEngage" + m_Playernumber;
        m_Direction = Vector3.zero;
        currentSpeed = Vector3.zero;
        UDmoving = false;
        LRmoving = false;
        lastPos = transform.position;

        m_CloakCooling = 0f;
    }
	
	// Update is called once per frame
	void Update () 
    {
        m_UDInputValue = Input.GetAxis(m_UDAxisName);
        m_LRInputValue = Input.GetAxis(m_LRAxisName);
	}

    private void FixedUpdate()
    {
        //control the movement
        calculateSpeed();
        tankDirection();
        UDMovement();
        LRMovement();
        CloakEngage();

    }

    void calculateSpeed()
    {
        Vector3 nowPos = transform.position;
        Vector3 difPos = nowPos - lastPos;
        currentSpeed = difPos / Time.deltaTime;
        //Debug.Log(currentSpeed);

        if (Mathf.Abs(currentSpeed.z) > 0.1 && Mathf.Abs(m_UDInputValue) > 0.1)
        {
            UDmoving = true;
        }
        else
        {
            UDmoving = false;
        }

        if (Mathf.Abs(currentSpeed.x) > 0.1 && Mathf.Abs(m_LRInputValue) > 0.1)
        {
            LRmoving = true;
        }
        else
        {
            LRmoving = false;
        }

        lastPos = nowPos;
    }

    void UDMovement()
    {
        if(LRmoving==false)
        {
            Vector3 UDmovement = Vector3.forward * m_UDInputValue * m_Speed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + UDmovement);
        }
    }

    void LRMovement()
    {
        if (UDmoving == false)
        {
            Vector3 LRmovemnt = Vector3.right * m_LRInputValue * m_Speed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + LRmovemnt);
        }
    }

    void tankDirection()
    {
        //Adjust tank direction
        if (Mathf.Abs(m_UDInputValue)>=Mathf.Epsilon && UDmoving == true)
        {
            m_Direction = (m_UDInputValue / Mathf.Abs(m_UDInputValue)) * Vector3.forward;
            transform.forward = m_Direction;
        }

        if (Mathf.Abs(m_LRInputValue)>=Mathf.Epsilon && LRmoving == true) 
        {
            m_Direction = (m_LRInputValue / Mathf.Abs(m_LRInputValue)) * Vector3.right;
            transform.forward = m_Direction;
        }
    }

    public void SetSpeed(float speed)
    {
        m_Speed = speed;
    }

    void CloakEngage()
    {
        if (Input.GetButton(m_CloakButton) && m_CloakCooling <= 0f)
        {
            Debug.Log("CloakEngage!!!");
            tankBuffControl tankBuff = gameObject.GetComponent<tankBuffControl>();
            StartCoroutine(tankBuff.CloakEngage());
            m_CloakCooling = m_CloakEngageCoolDown;
        }

        if (m_CloakCooling > 0) 
        {
            m_CloakCooling -= m_DecreaseFactor * Time.deltaTime;
        }
    }

}
