using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolControl : MonoBehaviour
{
    private List<GameObject> floorList = new List<GameObject>();
    private int floorIndex = 0;
    private bool isFalling = false;
    private void Awake()
    {
        foreach(Transform floor in transform)
        {
            floorList.Add(floor.gameObject);
        }
        Debug.Log("Floor Num:" + floorList.Count);
    }

    private void Start()
    {
        StartToFall();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isFalling = true;
            StartToFall();
        }
    }

    private void StartToFall()
    {
        if (!isFalling) return;
        //Debug.Log("Test");
        floorList[floorIndex].GetComponent<Floor>().Fall();
        Invoke("StartToFall", floorList[floorIndex + 1].GetComponent<Floor>().fallCD);
    }

    public void SpawnReset()
    {
        isFalling = false;
        CancelInvoke("StartToFall");
        floorIndex = 0;
        foreach (GameObject g in floorList)
        {
            g.GetComponent<Floor>().ResetFloor();
        }
    }
}
