using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]public GameObject playerObject = null;

    private static GameManager _instance;

    //Mechanics
    public Cabinet cabinet;

    public UIController uiController;

    public Transform playerTransform;
    public Vector3 spawnPoint;

    private void Start()
    {
        spawnPoint = playerTransform.position;
    }

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void TimeStopEffect()
    {
        if (Time.timeScale != 1) return;
        if(playerObject == null) playerObject = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(TimeStopCR());
    }
    IEnumerator TimeStopCR()
    {

        playerObject.GetComponent<PlayerController>().canUseBulletTime = false;
        //SLOW DOWN FOR 0.2S
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(0.2f);

        playerObject.GetComponent<PlayerController>().canUseBulletTime = true;
        Time.timeScale = 1f;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) Debug.LogError("Failed to find player reference");
    }

}
