using System.Collections;
using UnityEngine;
public class ButtonObject : MonoBehaviour
{
    private bool canLoad = false;

    private void Start()
    {
        StartCoroutine(setActive());
    }
    private IEnumerator setActive()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        canLoad = true;
    }

    public void LoadGame()
    {
        if(canLoad)
            SceneLoader.Instance.LoadNewScene("Level1");
    }
}
