using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : EnemyBase
{
    private State state;
    public GameObject bulletPrefab;
    public Transform fireLocation;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        state = State.Idle;
        player = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        playerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        canAttackPalyer = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (playerInSight && canAttackPalyer) state = State.Attacking; //Shoot
        else state = State.Idle; //Alert

        switch (state)
        {
            case State.Attacking:
                agent.isStopped = true;
                /*
                if ((transform.position - startingPosition).magnitude > maxChaseRange)
                {
                    state = State.Returning;
                    break;
                }
                */
                Attack();
                break;
            case State.Idle:
                agent.isStopped = true;
                break;
        }
    }

    protected void Attack()
    {
        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            //ATTACK
            
            Debug.Log("Shot Fired!");
            Fire();
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void Fire()
    {
        Vector3 fireLoc = fireLocation.position;
        Vector3 targetLoc = player.position;

        GameObject proj = Instantiate(bulletPrefab, fireLoc, Quaternion.LookRotation(targetLoc - fireLoc));
        proj.GetComponent<Bullet>().FireProjectile(fireLoc, targetLoc, damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && !isDead)
        {



            isDead = true;





            BeAttacked();
        }
    }
}
