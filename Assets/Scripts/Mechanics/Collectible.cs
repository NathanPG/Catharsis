using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string collectibleName = "";
    public bool shouldShowNarrative = false;
     
    public List<string> narrativeMsg;

    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hand" && shouldShowNarrative && !isCollected)
        {
            isCollected = true;
            GameManager.Instance.uiController.ShowNarrative(narrativeMsg);

            GetComponent<Animation>().Play();

            if (collectibleName == "Trophy")
            {
                GameManager.Instance.cabinet.TrophyCollected();
            }
            else if(collectibleName == "Photo")
            {
                GameManager.Instance.cabinet.PhotoCollected();
            }
        }
    }
}
