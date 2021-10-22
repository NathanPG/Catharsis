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
        //isFalling = true;
        //StartToFall();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isFalling = true;
            Invoke("StartToFall", floorList[0].GetComponent<Floor>().fallCD);
        }
    }

    private void StartToFall()
    {
        if (!isFalling || floorIndex >= 8) return;
        floorList[floorIndex].GetComponent<Floor>().Fall();
        Invoke("StartToFall", floorList[floorIndex + 1].GetComponent<Floor>().fallCD);
        Debug.Log("Fall:" + floorIndex);
        floorIndex++;
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
