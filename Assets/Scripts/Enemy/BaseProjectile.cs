using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    public float speed = 5.0f;
    public float height = 1.5f;
    public abstract void FireProjectile(Vector3 launcher, Vector3 target, float timeBetweenShots);
}
