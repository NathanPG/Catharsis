using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class LineVisual : MonoBehaviour
{
    public bool isLineHitting = false;
    public LayerMask whatIsGrappleable;
    public float grappleDistance;

    private void FixedUpdate()
    {
        if(Physics.Raycast(this.transform.position, this.transform.forward, grappleDistance, whatIsGrappleable))
        {
            GetComponent<XRInteractorLineVisual>().lineWidth = 0.01f;
        }
        else
        {
            GetComponent<XRInteractorLineVisual>().lineWidth = 0.0005f;
        }
    }
}
