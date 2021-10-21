using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private Vector3 spawnPoint;
    //Developer Set Fall Interval From Last Fllor
    public float fallCD = 1f;

    void Start()
    {
        spawnPoint = transform.position;
    }

    public void Fall()
    {

    }

    public void ResetFloor()
    {
        
    }
}
