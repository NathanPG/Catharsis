using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : MonoBehaviour
{
    public Transform startPosTransform;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Level2Start(startPosTransform.position);
    }

}
