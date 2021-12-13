using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pendulum : MonoBehaviour
{
    List<GameObject> TouchedObjs = new List<GameObject>();
    public GameObject hammerObj;

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("C");
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            collision.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;


            Vector3 direction = collision.gameObject.transform.position - hammerObj.transform.position;
            direction = new Vector3(direction.x, 0f, direction.z);
            collision.gameObject.GetComponent<Examinee>().EnableRagdoll();
            collision.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * 1000f, ForceMode.Impulse);

        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        TouchedObjs.Add(other.gameObject);

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;


            Vector3 direction = other.gameObject.transform.position - hammerObj.transform.position;
            direction = new Vector3(direction.x, 0f, direction.z);
            other.gameObject.GetComponent<Examinee>().EnableRagdoll();

            //other.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * 1000f, ForceMode.Impulse);

            var rigsChildren = other.gameObject.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody r in rigsChildren)
            {
                r.isKinematic = false;
                r.AddForce(direction.normalized * 50f, ForceMode.Impulse);
            }

            other.gameObject.GetComponent<Examinee>().Death();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //.........
    }

    /*
    private void Update()
    {
        foreach (GameObject g in TouchedObjs)
        {
            Vector3 direction = g.transform.position - hammerObj.transform.position;
            direction = new Vector3(direction.x, 0f, direction.z);
            g.GetComponent<Rigidbody>().AddForce(direction.normalized * 100f);
            
        }
    }
    */

    /*
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
    */
    
}
