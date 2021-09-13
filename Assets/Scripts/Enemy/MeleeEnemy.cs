using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : EnemyBase
{
    public State state;
    public Animator meleeAnimator;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        state = State.Idle;
        player = Camera.main.transform;
        meleeAnimator.SetBool("Idle", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        playerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        canAttackPalyer = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (playerInSight && canAttackPalyer) state = State.Attacking;

        //Try to approach
        else if (playerInSight && !canAttackPalyer && state != State.Returning)
        {
            if ((transform.position - startingPosition).magnitude > maxChaseRange)
            {
                state = State.Returning;
            }
            else
            {
                state = State.Approaching;
            }
        }  
        else
        {
            if ((transform.position - startingPosition).magnitude < 3f)
            {
                state = State.Idle;
            }
            else
            {
                state = State.Returning;
            }
        }

        switch (state)
        {
            case State.Approaching:
                agent.isStopped = false;
                meleeAnimator.SetBool("Idle", false);
                Approach();
                break;
            case State.Attacking:
                agent.isStopped = true;
                Attack();
                break;
            case State.Idle:
                meleeAnimator.SetBool("Idle", true);
                agent.isStopped = true;
                break;
            case State.Returning:
                meleeAnimator.SetBool("Idle", false);
                agent.isStopped = false;
                Returning();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && !isDead)
        {
            Debug.Log("Hit Enemy");
            BeAttacked();
        }
    }

    void Attack()
    {
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            //ATTACK
            transform.LookAt(player);
            meleeAnimator.Play("Attack");
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    
}
