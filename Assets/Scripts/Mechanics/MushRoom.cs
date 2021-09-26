using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushRoom : MonoBehaviour
{
    public float mushRoomForce;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            var rb = other.gameObject.GetComponent<Rigidbody>();
            //Clear Force
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            //MAKE JUMP
            rb.AddForce(Vector3.up * mushRoomForce);
        }
    }
}
