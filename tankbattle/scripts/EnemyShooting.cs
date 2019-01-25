using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour 
{
    public Rigidbody e_Shell;
    public Transform e_FireTransform;
    public float e_LunchForce = 20f;
    public float e_TimeInterval = 1f;
    private float e_DecreaseFactor = 1f;
	
	// Update is called once per frame
	void Update ()
    {
        AutoShoot();
	}

    private void FixedUpdate()
    {
        if(e_TimeInterval>0)
        {
            e_TimeInterval -= e_DecreaseFactor * Time.deltaTime;
        }
    }

    void AutoShoot()
    {
        if(e_TimeInterval<=0)
        {
            Rigidbody shellInstance = Instantiate(e_Shell, e_FireTransform.position, e_FireTransform.rotation * Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody;
            shellInstance.velocity = e_LunchForce * e_FireTransform.forward;
            e_TimeInterval = 1f;
        }
    }
}
