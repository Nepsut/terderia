using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
{
    [SerializeField] private RectTransform loadingScreen;
    [SerializeField] private GameObject continuePrompt;
    [SerializeField] private InputReader inputReader;
    public static bool LoadScreenOpen { get; private set; } = false;
    private const float loadFadeTime = 0.32f;
    private const float loadDelayAmount = 0.5f;
    private WaitForSeconds loadDelayWait;
    private bool loadDelayActive = false;

    //Events
    public static event Action<Scene> OnSceneTransitionStart;
    public static event Action OnSceneLoadStarted;
    public static event Action<Scene> OnSceneLoadFinished;
    public static event Action OnSceneTransitionEnd;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        loadDelayWait = new(loadDelayAmount);
        OnSceneTransitionStart += SceneLoader;
    }

    private void OnApplicationQuit()
    {
        inputReader.OnClickEvent -= CloseLoadingScreen;
        OnSceneTransitionStart -= SceneLoader;
    }

    private async void CloseLoadingScreen()
    {
        continuePrompt.SetActive(false);
        inputReader.OnClickEvent -= CloseLoadingScreen;
        await FadeOutLoadScreen();
        LoadScreenOpen = false;
    }

    private void HandleLoadCompleted()
    {
        continuePrompt.SetActive(true);
        //Animate continue prompt here if necessary
        inputReader.OnClickEvent += CloseLoadingScreen;
    }

    private async void SceneLoader(Scene newScene)
    {
        LeanTween.alpha(loadingScreen, 1f, loadFadeTime).setEaseOutQuart();
        await Task.Delay(TimeSpan.FromSeconds(loadFadeTime));
        OnSceneLoadStarted?.Invoke();
        await SceneManager.LoadSceneAsync(newScene.ToString());
        OnSceneLoadFinished?.Invoke(newScene);
        HandleLoadCompleted();
    }

    private async Awaitable FadeOutLoadScreen()
    {
        LeanTween.alpha(loadingScreen, 0f, loadFadeTime).setEaseOutQuart();
        await Task.Delay(TimeSpan.FromSeconds(loadFadeTime));
        loadingScreen.gameObject.SetActive(false);
        OnSceneTransitionEnd?.Invoke();
    }

    public void StartTransition(Scene newScene)
    {
        if (LoadScreenOpen)
        {
            Debug.LogWarning("Tried to start scene transition while one was already in progress!");
            return;
        }
        LoadScreenOpen = true;
        continuePrompt.SetActive(false);
        loadingScreen.gameObject.SetActive(true);
        OnSceneTransitionStart?.Invoke(newScene);
    }

    public void StartTransitionAfterDelay(Scene newScene)
    {
        if (!loadDelayActive && !LoadScreenOpen)
            StartCoroutine(TransitionStartWaiter(newScene));
    }

    private IEnumerator TransitionStartWaiter(Scene newScene)
    {
        loadDelayActive = true;
        yield return loadDelayWait;
        StartTransition(newScene);
        loadDelayActive = false;
    }

    public enum Scene
    {
        MainMenu = 0,

        //maps below this point
        Map1,

        //map 1 events below this point
        CabinEvent1,
        KoboldsEvent1,
        FanaticFrogEvent1,
        LizardEvent1,
        KappaEvent1
    }
}
