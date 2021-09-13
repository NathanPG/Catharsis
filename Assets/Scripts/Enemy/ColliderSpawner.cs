using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSpawner : MonoBehaviour
{
    public List<GameObject> enemyToSpawn;
    private bool spawned = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!spawned)
            {
                spawned = true;
                StartCoroutine("Spawn");
            }
            
        }
    }

    private IEnumerator Spawn()
    {
        foreach(GameObject g in enemyToSpawn)
        {
            g.SetActive(true);
            g.GetComponent<EnemyBase>().Spawn();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
