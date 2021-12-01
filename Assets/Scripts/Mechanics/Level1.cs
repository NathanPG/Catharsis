using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour
{
    public Material Level1SkyBox;
    private void Start()
    {
        GameManager.Instance.Level1Start();
        RenderSettings.skybox = Level1SkyBox;
    }
}
