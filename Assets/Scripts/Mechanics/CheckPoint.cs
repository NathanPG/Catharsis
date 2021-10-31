using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool shouldShowTip = false;
    public bool shouldShowNarrative = false;
    public bool shouldRepeatNarrative = false;
    public bool isSpawnPoint = false;
    public bool isStartPoint = false;
    public bool shouldShowGoal = true;

    public string tipString;
    public List<string> narrativeStrings;
    public string goalString;

    private bool isPlayerIn = false;
    private bool inBulletTutorial = false;

    private void Start()
    {
        if (isStartPoint)
        {
            GameManager.Instance.uiController.ShowTips(tipString);
            GameManager.Instance.uiController.ShowNarrative(narrativeStrings);
            GameManager.Instance.uiController.ToggleGoal(true, goalString);
        }
    }

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

            if (shouldShowGoal)
            {
                GameManager.Instance.uiController.ToggleGoal(true, goalString);
            }
            else
            {
                GameManager.Instance.uiController.ToggleGoal(false, goalString);
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
