using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string collectibleName = "";
    public bool shouldShowNarrative = false;
     
    public List<string> narrativeMsg;

    //Better to make subclass
    public Cabinet cabinet;

    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hand" && !isCollected)
        {
            isCollected = true;
            if (shouldShowNarrative)
            {
                GameManager.Instance.uiController.ShowNarrative(narrativeMsg);
            }
            
            GetComponent<Animation>().Play();
            GetComponent<AudioSource>().Play();
            if (collectibleName == "Trophy")
            {
                cabinet.TrophyCollected();
            }
            else if(collectibleName == "Photo")
            {
                cabinet.PhotoCollected();
            }
        }
    }
}
