using UnityEngine;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    [SerializeField] private GameObject tutorialTutorial;
    [SerializeField] private GameObject eventTutorial1;
    [SerializeField] private GameObject cardTutorial1;

    public static bool TutorialActive { get; private set; } = false;
    
    
    private void Start()
    {
        SubscribeEventListeners();
    }
    
    private void SubscribeEventListeners()
    {
        EventManager.OnEventStart += ShowTutorialTutorial;
        CardHolder.OnHolderActivateDone += ShowCardTutorial1;
    }
    
    private void UnsubscribeEventListeners()
    {
        EventManager.OnEventStart -= ShowTutorialTutorial;
        CardHolder.OnHolderActivateDone -= ShowCardTutorial1;
    }

    private void HandleTutorialOver()
    {
        TutorialActive = false;
    }

    private void ShowTutorialTutorial()
    {
        TutorialActive = true;
        tutorialTutorial.SetActive(true);
        tutorialTutorial.GetComponent<TutorialPopup>().OnTutorialClosed += HandleTutorialOver;
        tutorialTutorial.GetComponent<TutorialPopup>().OnTutorialClosed += () =>
        {
            TutorialActive = true;
            eventTutorial1.SetActive(true);
            eventTutorial1.GetComponent<TutorialPopup>().OnTutorialClosed += HandleTutorialOver;
        };
        EventManager.OnEventStart -= ShowTutorialTutorial;
    }

    private void ShowCardTutorial1()
    {
        TutorialActive = true;
        cardTutorial1.SetActive(true);
        cardTutorial1.GetComponent<TutorialPopup>().OnTutorialClosed += HandleTutorialOver;
        CardHolder.OnHolderActivateDone -= ShowCardTutorial1;
    }
}