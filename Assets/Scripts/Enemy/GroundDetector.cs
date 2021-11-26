using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool isGrounded = true;
    public LayerMask WhatIsGround;
    private Examinee examinee;

    private void Start()
    {
    }
    private void FixedUpdate()
    {
        //isGrounded = Physics.CheckBox(transform.position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, 12);
        isGrounded = Physics.CheckSphere(transform.position, 0.5f, WhatIsGround);
    }

    private void OnDrawGizmosSelected()
    {
     
        //Gizmos.DrawLine(transform.position, transform.position - Vector3.up * 1f);
        //Gizmos.DrawCube(transform.position, new Vector3(1f, 1f, 1f));
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    public void SetSelfRef(Examinee e)
    {
        examinee = e;
    }

    private void TouchLava()
    {

    }
}
