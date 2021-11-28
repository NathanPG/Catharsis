using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Examinee : EnemyBase
{
    public bool shouldMove = true, shouldRespawn = true, dead = false;
    public GroundDetector groundDetector = null;
    public Transform destBox, hipTransform;
    
    public State state;

    private Animator examineeAnimator;
    private Vector3 destPosition;
    private bool lostTrack = false;

    private void Start()
    {
        examineeAnimator = GetComponent<Animator>();
        startingPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        state = State.Idle;
        player = Camera.main.transform;
        //examineeAnimator.SetBool("Idle", true);
        canMove = true;
        destPosition = destBox.position;
        StartCoroutine(SetDest(destPosition, 3f));

        groundDetector.SetSelfRef(this);
    }
    // Update is called once per frame
    void Update()
    {
        if (dead) return;
        
        canAttackPalyer = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!shouldMove)
        {
            //STAY
            state = State.Idle;
        }
        else if (canAttackPalyer)
        {
            //meleeAnimator.SetBool("Attacking", true);
            state = State.Attacking;
        }
        else
        {
            //meleeAnimator.SetBool("Attacking", false);
            state = State.Approaching;
            //CONTINUE TO MOVE
        }

        switch (state)
        {
            case State.Approaching:
                //agent.isStopped = false;
                //meleeAnimator.SetBool("Idle", false);
                //meleeAnimator.Play("Forward");
                //Approach();
                break;
            case State.Attacking:
                //agent.isStopped = true;
                //Attack();
                break;
            case State.Idle:
                //meleeAnimator.SetBool("Idle", true);
                //agent.isStopped = true;
                break;
        }
        
    }

    private void FixedUpdate()
    {
        if (dead) return;

        if (!groundDetector.isGrounded) Debug.Log("NOT GROUNDED");

        if (!groundDetector.isGrounded && !lostTrack)
        {
            Debug.Log("Lost Track!");
            lostTrack = true;
            agent.enabled = false;
            examineeAnimator.enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }

        RaycastHit r;
        if (Physics.Raycast(transform.position, Vector3.down, out r, 2f))
        {
            
            if (r.transform.tag == "Lava" && !dead)
            {
                Debug.Log("Touched Lava");
                dead = true;
                Death();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawLine(transform.position, transform.position - Vector3.up * 2f);
    }

    private void Respawn()
    {
        agent.enabled = true;
        transform.position = startingPosition;
        examineeAnimator.enabled = true;
        lostTrack = false;
        GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(SetDest(destPosition, 3f));
        dead = false;
    }

    public void Death()
    {
        //Touch Lava
        dead = true;
        //Respawn
        Invoke("Respawn", 1f);
    }

    
}
