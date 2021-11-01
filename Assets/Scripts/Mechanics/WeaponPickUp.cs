using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    private bool pickedUp, available = false;

    public void SetAvailable()
    {
        available = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && available && !pickedUp)
        {
            //Open drawer
            pickedUp = true;
            GetComponent<AudioSource>().Play();
            GameManager.Instance.playerObject.GetComponent<PlayerController>().enableWeapon();
            this.gameObject.SetActive(false);
        }
    }
}
