using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerObject = null;
    [HideInInspector] public PlayerController playerController = null;
    [HideInInspector] public Rigidbody playerRigidbody = null;
    [HideInInspector] public MovementControl movementControl = null;
    private static GameManager _instance;

    //Mechanics

    public UIController uiController;

    public Transform playerTransform;
    public Vector3 spawnPoint;

    
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
        //if(playerObject == null) playerObject = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(TimeStopCR());
    }
    IEnumerator TimeStopCR()
    {
        //Debug.Log("TIME STOP");
        playerController.canUseBulletTime = false;
        //SLOW DOWN FOR 0.2S
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(1.5f);

        playerController.canUseBulletTime = true;
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
        
        //playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) Debug.LogError("Failed to find player reference");
        playerController = playerObject.GetComponent<PlayerController>();
        playerRigidbody = playerObject.GetComponent<Rigidbody>();
        movementControl = playerObject.GetComponent<MovementControl>();
    }

    private void Start()
    {
        spawnPoint = playerTransform.position;
    }

    public void Level1Start(Vector3 startPos)
    {
        spawnPoint = startPos;
        playerObject.transform.position = spawnPoint;
        playerController.canJump = true;
        playerController.canMove = true;
        playerRigidbody.useGravity = true;
        playerController.canUseBulletTime = false;
    }

    public void Level2Start(Vector3 startPos)
    {
        spawnPoint = startPos;
        playerObject.transform.position = spawnPoint;
        //TransitionEnd();
    }

    public void Level3Start(Vector3 startPos)
    {
        spawnPoint = startPos;
        playerObject.transform.position = spawnPoint;
        //TransitionEnd();
    }

    public void TransitionStart()
    {
        playerController.canJump = false;
        playerController.canMove = false;
        playerRigidbody.useGravity = false;
        playerController.canUseBulletTime = false;
    }
    public void TransitionEnd()
    {
        playerController.canJump = true;
        playerController.canMove = true;
        playerRigidbody.useGravity = true;
        playerController.canUseBulletTime = true;
    }
}
