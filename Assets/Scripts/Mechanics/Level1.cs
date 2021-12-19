using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour
{
    public Material Level1SkyBox;
    public Transform startPosition;
    private void Start()
    {
        GameManager.Instance.Level1Start(startPosition.position);
        RenderSettings.skybox = Level1SkyBox;
        RenderSettings.fog = false;
    }
}
