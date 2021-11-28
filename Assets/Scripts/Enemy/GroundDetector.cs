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
        isGrounded = Physics.CheckSphere(transform.position, 0.5f, WhatIsGround);
    }

    private void OnDrawGizmosSelected()
    {
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
