using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool isGrounded = true;

    private Examinee examinee;
    private void FixedUpdate()
    {
        isGrounded = Physics.CheckBox(transform.position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, 12);
        if (!isGrounded) Debug.Log("Not Grounded!");
        RaycastHit r;
        if(Physics.Raycast(transform.position, Vector3.down, out r, 0.5f))
        {
            if(r.transform.tag == "Lava" && !examinee.dead)
            {
                examinee.dead = true;
                examinee.Death();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawLine(transform.position, transform.position - Vector3.up * 0.5f);
        //Gizmos.DrawCube(transform.position, new Vector3(1f, 1f, 1f));
    }

    public void SetSelfRef(Examinee e)
    {
        examinee = e;
    }

    private void TouchLava()
    {

    }
}
