using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : MonoBehaviour
{
    public Transform startPosTransform;

    public Material Level2SkyBox;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Level2Start(startPosTransform.position);
        RenderSettings.skybox = Level2SkyBox;
        RenderSettings.fog = true;
        RenderSettings.fogDensity = 0.01f;
    }

}
