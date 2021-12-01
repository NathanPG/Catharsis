using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pendulum : MonoBehaviour
{
    List<GameObject> TouchedObjs = new List<GameObject>();
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            Debug.Log("HitC");
    }
    */
    private void OnTriggerEnter(Collider other)
    {
        TouchedObjs.Add(other.gameObject);

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //.........
    }

    private void FixedUpdate()
    {
        foreach (GameObject g in TouchedObjs)
        {
            Vector3 direction = g.transform.position - transform.position;
            g.GetComponent<Rigidbody>().AddForce(direction.normalized * 100f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TouchedObjs.Remove(other.gameObject);

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            other.gameObject.GetComponent<Examinee>().ResetDestOrDie();
        }
    }

    
}
