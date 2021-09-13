using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]public GameObject playerObject = null;

    private static GameManager _instance;

    private void Start()
    {
        
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
