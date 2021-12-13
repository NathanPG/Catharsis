using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryController : MonoBehaviour
{
    bool started = false;
    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Player" && !started)
        {
            started = true;
            GetComponent<Animator>().Play("bookCaseAnim");
        } 
    }
}
