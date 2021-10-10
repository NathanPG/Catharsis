using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : EnemyBase
{
    private State state;
    public GameObject projectilePrefab, rulerObj;
    public Transform fireLocation;
    private Animator rangedAnimator;
    // Start is called before the first frame update
    void Start()
    {
        rangedAnimator = GetComponent<Animator>();
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
                Attack();
                break;
            case State.Idle:
                agent.isStopped = true;
                break;
        }
    }

    protected void Attack()
    {
        Vector3 rotation = Quaternion.LookRotation(player.transform.position).eulerAngles;
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = Quaternion.Euler(rotation);


        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            rangedAnimator.Play("RangedAttack");
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void Fire()
    {
        Vector3 fireLoc = fireLocation.position;
        Vector3 targetLoc = player.position;
        rulerObj.SetActive(false);
        GameObject proj = Instantiate(projectilePrefab, fireLoc, Quaternion.LookRotation(targetLoc - fireLoc));
        proj.GetComponent<Projectile>().FireProjectile(fireLoc, targetLoc, damage);
    }

    private void ResetRuler()
    {
        rulerObj.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && !isDead)
        {
            BeAttacked();
        }
    }

    private void BeAttacked()
    {
        isDead = true;
        GameManager.Instance.TimeStopEffect();
        //TODO: RANDOM DEATH ANIM
        rangedAnimator.Play("Death0");
    }
}
