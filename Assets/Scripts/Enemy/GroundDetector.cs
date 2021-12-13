using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool isGrounded = true;
    public LayerMask WhatIsGround;
    

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.3f, WhatIsGround);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, 0.3f);
    }

   
}
