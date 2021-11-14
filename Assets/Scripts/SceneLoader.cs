using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public UnityEvent OnLoadBegin = new UnityEvent();
    public UnityEvent OnLoadEnd = new UnityEvent();
    //public ScreenFader screenFader = null;
    
    public Camera mainCamera = null;
    public Camera persistentCamera = null;
    private ScreenFade screenFade = null;
    private bool isLoading = false;

    private void Awake()
    {
        screenFade = GetComponent<ScreenFade>();
        SceneManager.sceneLoaded += SetActiveScene;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SetActiveScene;
    }

    public void LoadNewScene(string sceneName)
    {
        if (!isLoading)
        {
            StartCoroutine(LoadScene(sceneName));
        }
    }

    private IEnumerator LoadScene(string sceneName)
    {
        isLoading = true;

        OnLoadBegin?.Invoke();
        persistentCamera.enabled = true;
        //yield return screenFader.StartFadeIn();
        yield return new WaitForSecondsRealtime(screenFade.FadeIn());
        yield return StartCoroutine(UnloadCurrent());

        yield return new WaitForSecondsRealtime(3.0f);

        
        yield return StartCoroutine(LoadNew(sceneName));
        //yield return screenFader.StartFadeOut();
        //yield return new WaitForSecondsRealtime(screenFade.FadeOut());
        screenFade.FadeOut();
        persistentCamera.enabled = false;
        OnLoadEnd?.Invoke();

        isLoading = false;
    }

    private IEnumerator UnloadCurrent()
    {
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        while(!unloadOperation.isDone)
		    yield return null;
    }

    private IEnumerator LoadNew(string sceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loadOperation.isDone)
            yield return null;
    }

    private void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
    }
}

