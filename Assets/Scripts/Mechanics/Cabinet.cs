using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    public int numofCollectibles;
    public GameObject trophyLight, photoLight, emptyPhoto, emptyTrophy, weaponObj;

    private Animation animation;
    private bool finished = false, opened = false;

    private void Start()
    {
        GameManager.Instance.cabinet = this;
        animation = GetComponent<Animation>();
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
        finished = true;
        weaponObj.GetComponent<WeaponPickUp>().SetAvailable();
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && finished && !opened)
        {
            //Open drawer
            opened = true;
            animation.Play();
            GetComponent<AudioSource>().Play();
        }
    }


}
