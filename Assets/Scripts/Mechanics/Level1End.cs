using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1End : MonoBehaviour
{
    private bool canLoad = true;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && canLoad)
        {
            SceneLoader.Instance.LoadNewScene("Level2");
        }
    }
}
