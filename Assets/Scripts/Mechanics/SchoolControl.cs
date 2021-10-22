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
            //Debug.Log(floor.gameObject.name);
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
        if(other.tag == "Player" && !isFalling)
        {
            GameManager.Instance.spawnPoint = new Vector3(-1.11000001f, -18.2700005f, 69.3650055f);
            isFalling = true;
            StartToFall();
        }
    }

    private void StartToFall()
    {
        if (!isFalling) return;
        floorList[floorIndex].GetComponent<Floor>().Fall();
        if (floorIndex == 7) return;
        Invoke("StartToFall", floorList[floorIndex+1].GetComponent<Floor>().fallCD);
        floorIndex++;
    }

    public void SpawnReset()
    {
        isFalling = false;
        CancelInvoke("StartToFall");
        floorIndex = 0;
        foreach (GameObject g in floorList)
        {
            //Debug.Log("RESET: "+ g.name);
            g.SetActive(true);
            g.GetComponent<Floor>().ResetFloor();
        }
    }
}
