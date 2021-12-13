using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public bool isShowingNarrative = false, isShowingTips = false;
    public Text narrativeText, tipText, goalText;
    public float narrativeFadeDuration, tipFadeDuration, narrativeDuration, tipDuration;

    private int narrativeIndex = 0;
    private List<string> currentList;
    private string currentTip;

    private void Awake()
    {
        narrativeText.enabled = false;
        tipText.enabled = false;
        goalText.enabled = false;
    }

    IEnumerator TipInorOut(bool shouldFadeIn)
    {
        // FADE OUT
        if (!shouldFadeIn)
        {
            for (float i = tipFadeDuration; i >= 0; i -= Time.deltaTime)
            {
                tipText.color = new Color(1f, 1f, 1f, i);
                yield return null;
            }
            tipText.enabled = false;
            isShowingTips = false;
        }

        // FADE IN
        else
        {
            isShowingTips = true;
            // FADE IN
            for (float i = 0; i <= tipFadeDuration; i += Time.deltaTime)
            {
                tipText.color = new Color(1f, 1f, 1f, i);
                yield return null;
            }

            yield return new WaitForSeconds(tipDuration);

            // FADE OUT
            StartCoroutine(TipInorOut(false));
        }
    }

    IEnumerator NarrativeInorOut(bool shouldFadeIn)
    {
        // FADE OUT
        if (!shouldFadeIn)
        {
            for (float i = narrativeFadeDuration; i >= 0; i -= Time.deltaTime)
            {
                narrativeText.color = new Color(1f, 1f, 1f, i);
                yield return null;
            }

            if(narrativeIndex < currentList.Count-1)
            {
                narrativeIndex ++;
                StartCoroutine(NarrativeInorOut(true));
            }
            else
            {
                narrativeText.enabled = false;
                isShowingNarrative = false;
            }
        }

        // FADE IN
        else
        {
            isShowingNarrative = true;
            narrativeText.text = currentList[narrativeIndex];
            // FADE IN
            for (float i = 0; i <= narrativeFadeDuration; i += Time.deltaTime)
            {
                narrativeText.color = new Color(1f, 1f, 1f, i);
                yield return null;
            }

            yield return new WaitForSeconds(narrativeDuration);
            // FADE OUT
            
            StartCoroutine(NarrativeInorOut(false));
            
        }
    }

    public void ShowNarrative(List<string> narrativeList)
    {
        if (isShowingNarrative)
        {
            //Hide Narrative, then Show
            StopCoroutine(NarrativeInorOut(true));
            StopCoroutine(NarrativeInorOut(false));
        }
        

        currentList = narrativeList;
        //RESET INDEX
        narrativeIndex = 0;
        narrativeText.text = currentList[narrativeIndex];
        narrativeText.enabled = true;
        StartCoroutine(NarrativeInorOut(true));
    }

    public void ShowTips(string tipMsg)
    {
        
        if (isShowingTips)
        {
            //Hide Tips, then Show
            StopCoroutine(TipInorOut(true));
            StopCoroutine(TipInorOut(false));
        }
        currentTip = tipMsg;
        tipText.text = currentTip;
        tipText.enabled = true;
        StartCoroutine(TipInorOut(true));
    }

    public void ToggleGoal(bool b, string s)
    {
        if (s.Length > 0)
        {
            goalText.text = s;
        }

        if (b)
        {  
            goalText.enabled = true;
        }
        else
        {
            goalText.enabled = false;
        }

        
    }
}
