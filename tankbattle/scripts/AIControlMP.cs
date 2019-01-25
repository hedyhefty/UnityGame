using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControlMP : MonoBehaviour {

    public Transform m_EnergyCube;
    public int m_NumberOfPlayers = 1;

    public float ai_VisibleRange = 8f;
    public float ai_CloakBias = 4f;
    public float ai_Speed = 8f;
    public float ai_DecisionInterval = 2f;
    public float ai_TimeDecreasingFactor = 1f;
    public float ai_DecisionSpeedUpRate = 8f;

    private GameObject[] m_Players;
    private bool[] m_PlayerCloaking;
    //private int m_ClosestPlayerIndex;

    private Rigidbody ai_Ridgidbody;
    private Vector3[] ai_DistanceFromPlayer;
    private Vector3 ai_DistanceFromECube;

    private float ai_AgainstPlayerRange = 8f;
    private float ai_OriDecInterval;
    private bool ai_TargetDetected;
    private Vector3 ai_targetDirectionByRay;
    private float ai_DistanceFixing;
    private float ai_ChangeCounting;

    //private float ai_OriPlayerRange;

    // Use this for initialization
    private void Awake()
    {
        ai_Ridgidbody = GetComponent<Rigidbody>();

    }

    void Start()
    {
        ai_DistanceFixing = DetectFront();
        ai_ChangeCounting = 0;

        m_Players = GameObject.FindGameObjectsWithTag("Player");
        ai_DistanceFromPlayer = new Vector3[m_NumberOfPlayers];

        m_PlayerCloaking = new bool[m_NumberOfPlayers];


        ai_OriDecInterval = ai_DecisionInterval;

        CalculasDistanceFromPlayers();
        ai_DistanceFromECube = m_EnergyCube.position - transform.position;
        ai_TargetDetected = false;
        ai_targetDirectionByRay = Vector3.zero;
        ai_AgainstPlayerRange = ai_VisibleRange;
        //ai_OriPlayerRange = ai_AgainstPlayerRange;
    }

    // Update is called once per frame
    void Update()
    {
        DetectTarget();
        if (!ai_TargetDetected)
        {
            CalculateDistance();
            int ClosestPlayerIndex = FindClosestPlayer();
            GameObject ClosestPlayer = m_Players[ClosestPlayerIndex];
            float distanceFromPlayer = ai_DistanceFromPlayer[ClosestPlayerIndex].magnitude;
            if(m_PlayerCloaking[ClosestPlayerIndex])
            {
                distanceFromPlayer += ai_CloakBias;
                //Debug.Log("add bias!");
            }
            
            if (distanceFromPlayer < ai_AgainstPlayerRange)
            {
                AttackPlayer(ClosestPlayer);
                //Debug.Log("Fuck player!");
            }
            else
            {
                AttackEnergyCube();
                //Debug.Log("Fuck Cube!");
            }
        }
        else
        {
            transform.forward = ai_targetDirectionByRay;
        }
    }

    private void FixedUpdate()
    {
        UpDateIntervalTime();
    }

    void ChangeDirection(Vector3 TargetPosition)
    {
        if (ai_DecisionInterval <= 0)
        {
            Vector3 targetDirection = TargetPosition - transform.position;
            float x_Direction = targetDirection.x / Mathf.Abs(targetDirection.x);
            float z_Direction = targetDirection.z / Mathf.Abs(targetDirection.z);
            float wheel = Random.Range(-1, 1);

            if (wheel < 0)
            {
                transform.forward = new Vector3(x_Direction, 0, 0);
                //transform.Translate(transform.TransformDirection(Vector3.right * x_Direction));
            }
            else
            {
                transform.forward = new Vector3(0, 0, z_Direction);
                //transform.Translate(transform.TransformDirection(Vector3.forward * z_Direction));
            }

            if (targetDirection.magnitude <= ai_AgainstPlayerRange)
            {
                ai_DecisionInterval = (targetDirection.magnitude / ai_DecisionSpeedUpRate) * (ai_OriDecInterval + Random.Range(0, 0.2f));
            }
            else
            {
                ai_DecisionInterval = (ai_OriDecInterval + Random.Range(0f, 0.7f));
            }

        }
    }

    void AttackPlayer(GameObject c_player)
    {
        ChangeDirection(c_player.transform.position);
        float FrontDistance = DetectFront();
        float Differencial = Mathf.Abs(ai_DistanceFixing - FrontDistance);
        if (Differencial > 1f)
        {
            ai_ChangeCounting++;
            if (ai_ChangeCounting < 40)
            {
                FrontDistance = ai_DistanceFixing;
            }
            else
            {
                ai_ChangeCounting = 0;
                ai_DistanceFixing = FrontDistance;
            }

        }
        //Debug.Log(FrontDistance);

        if (FrontDistance > 2f)
        {
            Vector3 movement = transform.forward * ai_Speed * Time.deltaTime;
            ai_Ridgidbody.MovePosition(ai_Ridgidbody.position + movement);
        }
    }

    void AttackEnergyCube()
    {
        ChangeDirection(m_EnergyCube.position);
        float FrontDistance = DetectFront();

        float Differencial = Mathf.Abs(ai_DistanceFixing - FrontDistance);
        if (Differencial > 1f)
        {
            ai_ChangeCounting++;
            if (ai_ChangeCounting < 20)
            {
                FrontDistance = ai_DistanceFixing;
            }
            else
            {
                ai_ChangeCounting = 0;
                ai_DistanceFixing = FrontDistance;
            }

        }
        //Debug.Log(FrontDistance);
        if(FrontDistance>2f)
        {
            Vector3 movement = transform.forward * ai_Speed * Time.deltaTime;
            ai_Ridgidbody.MovePosition(ai_Ridgidbody.position + movement);
        }
    }

    void CalculateDistance()
    {
        //ai_DistanceFromPlayer = m_player1.position - transform.position;
        CalculasDistanceFromPlayers();
        ai_DistanceFromECube = m_EnergyCube.position - transform.position;
    }

    void UpDateIntervalTime()
    {
        if (ai_DecisionInterval > 0)
        {
            ai_DecisionInterval -= ai_TimeDecreasingFactor * Time.deltaTime;
        }
    }

    void DetectTarget()
    {
        Vector3 y_bias = new Vector3(0, 0, 0);
        int ai_countFalse = 0;
        Ray ai_DetectionRayZP = new Ray(transform.position, Vector3.forward * 100);
        //Debug.DrawLine(transform.position, transform.position + Vector3.forward * 100, Color.red);
        RaycastHit[] target_HitZP;
        target_HitZP = Physics.RaycastAll(ai_DetectionRayZP);

        if (target_HitZP.Length > 0)
        {
            int closestZP = GetClosest(target_HitZP);
            if (target_HitZP[closestZP].collider.gameObject.layer == 9 || target_HitZP[closestZP].collider.gameObject.layer == 11)
            {
                ai_TargetDetected = true;
                ai_targetDirectionByRay = Vector3.forward;
            }
            else
            {
                ai_countFalse++;
            }
        }
        else
        {
            ai_countFalse++;
        }

        Ray ai_DetectionRayZN = new Ray(transform.position + y_bias, Vector3.forward * -100);
        //Debug.DrawLine(transform.position, transform.position + Vector3.forward * -100, Color.red);
        RaycastHit[] target_HitZN;
        target_HitZN = Physics.RaycastAll(ai_DetectionRayZN);
        if (target_HitZN.Length > 0)
        {
            int closestZN = GetClosest(target_HitZN);
            if (target_HitZN[closestZN].collider.gameObject.layer == 9 || target_HitZN[closestZN].collider.gameObject.layer == 11)
            {
                ai_TargetDetected = true;
                ai_targetDirectionByRay = Vector3.forward * (-1);
            }
            else
            {
                ai_countFalse++;
            }
        }
        else
        {
            ai_countFalse++;
        }

        Ray ai_DetectionRayXP = new Ray(transform.position + y_bias, Vector3.right * 100);
        //Debug.DrawLine(transform.position, transform.position + Vector3.right * 100, Color.red);
        RaycastHit[] target_HitXP;
        target_HitXP = Physics.RaycastAll(ai_DetectionRayXP);
        if (target_HitXP.Length > 0)
        {
            int closestXP = GetClosest(target_HitXP);
            if (target_HitXP[closestXP].collider.gameObject.layer == 9 || target_HitXP[closestXP].collider.gameObject.layer == 11)
            {
                ai_TargetDetected = true;
                ai_targetDirectionByRay = Vector3.right;
            }
            else
            {
                ai_countFalse++;
            }
        }
        else
        {
            ai_countFalse++;
        }


        Ray ai_DetectionRayXN = new Ray(transform.position + y_bias, Vector3.right * -100);
        //Debug.DrawLine(transform.position, transform.position + Vector3.right * -100, Color.red);
        RaycastHit[] target_HitXN;
        target_HitXN = Physics.RaycastAll(ai_DetectionRayXN);
        if (target_HitXN.Length > 0)
        {
            int closestXN = GetClosest(target_HitXN);
            if (target_HitXN[closestXN].collider.gameObject.layer == 9 || target_HitXN[closestXN].collider.gameObject.layer == 11)
            {
                ai_TargetDetected = true;
                ai_targetDirectionByRay = Vector3.right * (-1);
            }
            else
            {
                ai_countFalse++;
            }
        }
        else
        {
            ai_countFalse++;
        }

        //if(target_HitZN.Length>0)
        //{
        //    int testClosest = GetClosest(target_HitZN);
        //    Debug.Log("closest is:" + target_HitZN[testClosest].collider.gameObject);
        //}

        //for (int i = 0; i < target_HitZN.Length;i++)
        //{
        //    Debug.Log(target_HitZN[i].collider.gameObject);
        //    Debug.Log(target_HitZN[i].distance);
        //    Debug.Log(i);
        //}

        //print("count:");
        //Debug.Log(ai_countFalse);
        if (ai_countFalse == 4)
        {
            ai_TargetDetected = false;
        }
        //Debug.Log(ai_TargetDetected);
    }

    int GetClosest(RaycastHit[] raycastHits)
    {
        int closestIndex = 0;
        float closestDistence = Mathf.Infinity;

        for (int i = 0; i < raycastHits.Length; i++)
        {
            if (raycastHits[i].distance <= closestDistence)
            {
                closestDistence = raycastHits[i].distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    public void Cloaking(int i)
    {
        m_PlayerCloaking[i - 1] = true;
        //Debug.Log("i know cloaking");
    }

    public void OutFromCloak(int i)
    {
        m_PlayerCloaking[i - 1] = false;
    }

    void CalculasDistanceFromPlayers()
    {
        for (int i = 0; i < m_NumberOfPlayers; i++)
        {
            ai_DistanceFromPlayer[i] = m_Players[i].gameObject.transform.position - transform.position;
        }
    }

    private int FindClosestPlayer()
    {
        float minDistance = Mathf.Infinity;
        int minIndex = 0;

        for (int i = 0; i < m_NumberOfPlayers; i++)
        {
            if(m_PlayerCloaking[i])
            {
                if (ai_DistanceFromPlayer[i].magnitude + ai_CloakBias < minDistance)
                {
                    minDistance = ai_DistanceFromPlayer[i].magnitude;
                    minIndex = i;
                }
            }
            else
            {
                if (ai_DistanceFromPlayer[i].magnitude < minDistance)
                {
                    minDistance = ai_DistanceFromPlayer[i].magnitude;
                    minIndex = i;
                }
            }
        }
        return minIndex;
    }

    void InitializedCloaking()
    {
        for (int i = 0; i < m_NumberOfPlayers;i++)
        {
            m_PlayerCloaking[i] = false;
        }
    }

    private float DetectFront()
    {
        Vector3 Detect_bias = new Vector3(0, 0, 0);
        Ray ai_DetectionRayFront = new Ray(transform.position + Detect_bias, transform.forward);
        Debug.DrawLine(transform.position + Detect_bias, transform.position + transform.forward * 100, Color.red);
        RaycastHit[] Front_Hit;
        Front_Hit = Physics.RaycastAll(ai_DetectionRayFront);
        if (Front_Hit.Length > 0)
        {
            int closestFront = GetClosest(Front_Hit);
            //Debug.Log("Closest ting: " + Front_Hit[closestFront].collider.gameObject);
            Vector3 TrueDistance = Front_Hit[closestFront].collider.gameObject.transform.position - transform.position;
            return Front_Hit[closestFront].distance;
        }

        return Mathf.Infinity;
    }
}
