using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public List<GameObject> fragmentList = new List<GameObject>();

    private void Start()
    {
        foreach(Transform child in transform)
        {
            fragmentList.Add(child.gameObject);
        }
        //DestroyObjs();
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Weapon")
        {
            GetComponent<BoxCollider>().enabled = false;
            DestroyObjs();
        }
    }


    private void DestroyObjs()
    {
        foreach (GameObject g in fragmentList)
        {
            g.GetComponent<Rigidbody>().isKinematic = false;
            g.GetComponent<Rigidbody>().useGravity = true;
            Destroy(g, 5f);
        }
    }
}
