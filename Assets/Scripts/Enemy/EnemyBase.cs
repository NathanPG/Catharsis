using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    
    public LayerMask whatIsPlayer;
    public float timeBetweenAttacks, sightRange, attackRange, maxChaseRange, damage;
    public List<AudioClip> hitList = new List<AudioClip>();
    [HideInInspector] public Transform player;

    public Vector3 startingPosition;
    protected NavMeshAgent agent;
    protected bool playerInSight, canAttackPalyer, canMove;
    protected bool alreadyAttacked = false;
    protected bool isDead = false;

    public enum State
    {
        Approaching,
        Attacking,
        Returning,
        Idle
    }

    protected void Approach()
    {
        agent.SetDestination(player.position);
    }

    protected IEnumerator SetDest(Vector3 globalPosition, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(globalPosition);
        }
    }

    protected IEnumerator Death(float duration)
    {
        /*
        MeshRenderer mr = GetComponent<MeshRenderer>();
        float elapsedTime = 0f;
        float cutOff = transform.position.y + 1.7f;
        while (elapsedTime <= duration)
        {
            cutOff = Mathf.Lerp(transform.position.y + 1.7f, transform.position.y - 1.7f, elapsedTime / duration);
            mr.material.SetFloat("_CutoffHeight", cutOff);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        mr.material.SetFloat("_CutoffHeight", transform.position.y - 1.7f);
        */
        yield return new WaitForSecondsRealtime(3f);
        Destroy(this.gameObject);
    }

    protected void HitSFX()
    {
        GetComponent<AudioSource>().Play();
    }

    
    public void Spawn()
    {
        //StartCoroutine(SpawnEffect(1f));
    }

    //Old dissolve effect
    public IEnumerator SpawnEffect(float duration)
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        float elapsedTime = 0f;
        float cutOff = startingPosition.y - 1.7f;
        while (elapsedTime <= duration)
        {
            cutOff = Mathf.Lerp(startingPosition.y - 1.7f, startingPosition.y + 1.7f, elapsedTime / duration);
            mr.material.SetFloat("_CutoffHeight", cutOff);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        mr.material.SetFloat("_CutoffHeight", startingPosition.y + 1.7f);
        //Spawn complete
    }

    protected void Returning()
    {
        agent.SetDestination(startingPosition);
    }
    

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxChaseRange);
    }

    protected void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void SetMove(int i)
    {
        canMove = (i == 1) ? true : false;
    }

    protected void DeathAnimEnd()
    {
        Destroy(this.gameObject, 5f);
    }
}
