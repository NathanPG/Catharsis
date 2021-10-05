using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    public int numofCollectibles;
    public GameObject trophyLight, photoLight, emptyPhoto, emptyTrophy;

    private void Start()
    {
        GameManager.Instance.cabinet = this;
    }

    private void UpdateProgress()
    {
        numofCollectibles--;
        if(numofCollectibles == 0)
        {
            FinishedCollection();
        }
    }

    private void FinishedCollection()
    {
        Debug.Log("COLLECTION FINISHED");
        //Open drawer
    }

    public void TrophyCollected()
    {
        emptyTrophy.SetActive(false);
        trophyLight.SetActive(true);
        UpdateProgress();
    }

    public void PhotoCollected()
    {
        emptyPhoto.SetActive(false);
        photoLight.SetActive(true);
        UpdateProgress();
    }


    
}
