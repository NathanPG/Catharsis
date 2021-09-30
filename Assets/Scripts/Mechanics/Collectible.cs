using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public bool shouldShowNarrative = false;
    public List<string> narrativeMsg;

    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hand" && shouldShowNarrative && !isCollected)
        {
            isCollected = true;
            GameManager.Instance.uiController.ShowNarrative(narrativeMsg);
            GameManager.Instance.cabinet.UpdateProgress();
        }
    }
}
