using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public float grappleDistance;
    private SpringJoint joint;
    public GameObject player;
    public CapsuleCollider capsuleCollider;
    private bool grappling = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        //Debug.DrawRay(this.transform.position, this.transform.forward);
    }

    private void LateUpdate()
    {
        if (grappling)
        {
            DrawRope();
        }
    }

    public void StartGrapple()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, grappleDistance, whatIsGrappleable))
        {
            if (grappling) return;



            lineRenderer.enabled = true;



            grappling = true;
            lineRenderer.positionCount = 2;
            grapplePoint = hit.point;
            joint = player.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distance = Vector3.Distance(player.transform.position, grapplePoint);

            joint.maxDistance = distance * 0.2f;
            joint.minDistance = distance * 0.1f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 0.5f;
        }
    }

    void DrawRope()
    {
        lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }


    public void StopGrapple()
    {
        grappling = false;
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }
}
