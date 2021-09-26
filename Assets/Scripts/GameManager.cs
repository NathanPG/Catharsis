using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]public GameObject playerObject = null;

    private static GameManager _instance;

    public UIController uiController;

    public Transform playerTransform;
    public Vector3 spawnPoint;

    private void Start()
    {
        spawnPoint = playerTransform.position;
        //TODO: CHECK UI CONTROLLER
        if(uiController == null)
        {
            
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {

            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) Debug.LogError("Failed to find player reference");

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
