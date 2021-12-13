using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2End : MonoBehaviour
{
    private bool canLoad = true;
    public Material loadMaterial;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hand" && canLoad)
        {
            SceneLoader.Instance.LoadNewScene("Level3");
            RenderSettings.skybox = loadMaterial;
        }
    }
}
