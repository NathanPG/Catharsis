using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeControl : MonoBehaviour
{
    public string animName;
    bool played = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!played)
        {
            played = true;
            GetComponent<Animator>().Play(animName);
        }
    }


}
