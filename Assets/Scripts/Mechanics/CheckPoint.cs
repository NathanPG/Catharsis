using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool shouldShowTip = false;
    public bool shouldShowNarrative = false;
    public bool shouldRepeatNarrative = false;
    public bool isSpawnPoint = false;

    public string tipString;
    public List<string> narrativeStrings;

    private bool isPlayerIn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isPlayerIn)
        {
            isPlayerIn = true;
            if (shouldShowTip)
            {
                GameManager.Instance.uiController.ShowTips(tipString);
            }
            if (shouldShowNarrative)
            {
                GameManager.Instance.uiController.ShowNarrative(narrativeStrings);
            }
            if (isSpawnPoint)
            {
                GameManager.Instance.spawnPoint = transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerIn = false;
            if (!shouldRepeatNarrative) shouldShowNarrative = false;
        }
    }
}
