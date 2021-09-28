using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    public int numofCollectibles;

    private void Start()
    {
        GameManager.Instance.cabinet = this;
    }

    public void UpdateProgress()
    {
        numofCollectibles--;
        if(numofCollectibles == 0)
        {
            FinishedCollection();
        }
    }

    public void FinishedCollection()
    {
        Debug.Log("COLLECTION FINISHED");
        //Turn on lights

        //Open drawer
    }
}
