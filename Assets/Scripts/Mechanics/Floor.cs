using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private Vector3 spawnPoint;
    private Quaternion spawnRotation;
    private Rigidbody rb;
    //Developer Set Fall Interval From Last Fllor
    public float fallCD = 1f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        spawnPoint = transform.position;
        spawnRotation = transform.rotation;
    }

    public void Fall()
    {
        Debug.Log(this.gameObject.name);
        rb.isKinematic = false;
        rb.useGravity = true;
        Invoke("Hide", 5f);
    }

    private void Hide()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        this.gameObject.SetActive(false);
    }

    public void ResetFloor()
    {
        CancelInvoke("Hide");
        rb.isKinematic = true;
        rb.useGravity = false;
        transform.rotation = spawnRotation;
        transform.position = spawnPoint;
    }
}
