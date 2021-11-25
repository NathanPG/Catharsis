using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Examinee : EnemyBase
{
    public bool shouldMove = true, shouldRespawn = true, dead = false;
    public GroundDetector groundDetector = null;
    public Transform destBox;
    public State state;

    private Animator examineeAnimator;
    

    private void Start()
    {
        examineeAnimator = GetComponent<Animator>();
        startingPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        state = State.Idle;
        player = Camera.main.transform;
        //examineeAnimator.SetBool("Idle", true);
        canMove = true;

        SetDest(destBox.position);

        groundDetector.SetSelfRef(this);
    }
    // Update is called once per frame
    void Update()
    {
        if (dead) return;
        /*
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
                agent.isStopped = false;
                //meleeAnimator.SetBool("Idle", false);
                //meleeAnimator.Play("Forward");
                //Approach();
                break;
            case State.Attacking:
                agent.isStopped = true;
                //Attack();
                break;
            case State.Idle:
                //meleeAnimator.SetBool("Idle", true);
                agent.isStopped = true;
                break;
        }
        */
    }

    private void FixedUpdate()
    {
        if (dead) return;

        if (!groundDetector.isGrounded)
        {

            agent.enabled = false;
            examineeAnimator.enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;

        }
    }

    public void Respawn()
    {
        agent.enabled = true;
        transform.position = startingPosition;
        examineeAnimator.enabled = true;
        GetComponent<Rigidbody>().isKinematic = true;

        SetDest(destBox.position);
    }

    public void Death()
    {
        //Touch Lava
        dead = true;
        //Respawn
        Invoke("Respawn", 1f);
    }
}
