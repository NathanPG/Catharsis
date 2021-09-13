using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BaseProjectile
{
    Vector3 m_direction;
    bool m_fired;
    // Update is called once per frame
    void Update()
    {
        if (m_fired)
        {
            transform.position += m_direction * (speed * Time.deltaTime);
        }
    }

    public override void FireProjectile(Vector3 launcher, Vector3 target, float timeBetweenShots)
    {
        m_direction = (target - launcher).normalized;
        m_fired = true;

        Destroy(gameObject, 5.0f); 
    }

    
    //HIT
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
           // other.transform.parent.GetComponent<Enemy>().Damage(m_damage);

            Destroy(gameObject);
        }
    }
    
}
