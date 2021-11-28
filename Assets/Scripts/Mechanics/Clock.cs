using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public Transform shortTransform, longTransform;
    public float dayLengthRealLifeInSecs = 12f;
    private float day;

    private void FixedUpdate()
    {
        //12 seconds for 1 day
        day += Time.deltaTime / dayLengthRealLifeInSecs;

        float dayNorm = day % 1f;
        shortTransform.localEulerAngles = new Vector3(0, 0, dayNorm * 360f);
        longTransform.localEulerAngles = new Vector3(0, 0, dayNorm * 360f * 24f);
    }
}
