using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmClock : MonoBehaviour
{
    private bool isOn = true;
    public AudioClip turnOff;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && isOn)
        {
            isOn = false;
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().PlayOneShot(turnOff);
        }
    }
}
