using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3 : MonoBehaviour
{
    public Transform startPosTransform;
    void Start()
    {
        GameManager.Instance.Level3Start(startPosTransform.position);
        RenderSettings.fog = false;
    }
}
