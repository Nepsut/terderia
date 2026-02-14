using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
{
    [SerializeField] private RectTransform loadingScreen;
    [SerializeField] private GameObject continuePrompt;
    [SerializeField] private InputReader inputReader;

    private bool fadeInProgress = false;
    private bool loadInProgress = false;
    private bool loadScreenOpen = false;
    private const float loadFadeTime = 0.32f;
    private event Action<Scene> OnSceneTransitionStart;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        OnSceneTransitionStart += SceneTransitionAnimator;
        OnSceneTransitionStart += SceneLoader;
    }

    private void OnApplicationQuit()
    {
        inputReader.OnClickEvent -= CloseLoadingScreen;
        OnSceneTransitionStart -= SceneTransitionAnimator;
        OnSceneTransitionStart -= SceneLoader;
    }

    private async void CloseLoadingScreen()
    {
        continuePrompt.SetActive(false);
        inputReader.OnClickEvent -= CloseLoadingScreen;
        await FadeOutLoadScreen();
        loadScreenOpen = false;
    }

    private void HandleLoadCompleted()
    {
        continuePrompt.SetActive(true);
        //Animate continue prompt here if necessary
        inputReader.OnClickEvent += CloseLoadingScreen;
    }

    public async void SceneLoader(Scene newScene)
    {
        loadInProgress = true;
        await SceneManager.LoadSceneAsync(newScene.ToString());
        loadInProgress = false;
        if (!fadeInProgress) HandleLoadCompleted();
    }

    public async void SceneTransitionAnimator(Scene _)
    {
        fadeInProgress = true;
        LeanTween.alpha(loadingScreen, 1f, loadFadeTime).setEaseOutQuart();
        await Task.Delay(TimeSpan.FromSeconds(loadFadeTime));
        fadeInProgress = false;
        if (!loadInProgress) HandleLoadCompleted();
    }

    public async Awaitable FadeOutLoadScreen()
    {
        LeanTween.alpha(loadingScreen, 0f, loadFadeTime).setEaseOutQuart();
        await Task.Delay(TimeSpan.FromSeconds(loadFadeTime));
        loadingScreen.gameObject.SetActive(false);
    }

    public void StartTransition(Scene newScene)
    {
        if (loadScreenOpen)
        {
            Debug.LogWarning("Tried to start scene transition while one was already in progress!");
            return;
        }
        loadScreenOpen = true;
        continuePrompt.SetActive(false);
        loadingScreen.gameObject.SetActive(true);
        OnSceneTransitionStart?.Invoke(newScene);
    }

    public enum Scene
    {
        MainMenu = 0,

        //maps below this point
        Map1,

        //map 1 events below this point
        CabinEvent1
    }
}
