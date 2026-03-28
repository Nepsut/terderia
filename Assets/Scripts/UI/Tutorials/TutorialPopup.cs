using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [field: SerializeField] public Button DoneButton { get; private set; }
    [SerializeField] private TMP_Text doneButtonText;
    [field: SerializeField] public Button BackButton { get; private set; }
    [field: SerializeField] public Button CloseButton { get; private set; }
    [SerializeField] private RectTransform tutorialMaskRect;
    [SerializeField] private RectTransform tutorialTextHolderRect;
    [SerializeField] private TMP_Text tutorialNumberText;

    private const float tutorialTweenTime = 0.24f;
    private int activeTutorialPage = 0;     //Start from 0
    private int tutorialCount;
    private float textHolderBaseX;
    private float maskRectWidth;
    private int tweenId = -1;

    public event Action OnTutorialClosed;

    private void Start()
    {
        tutorialCount = tutorialTextHolderRect.childCount - 1;
        maskRectWidth = tutorialMaskRect.rect.width;
        if (tutorialCount == 0)
        {
            tutorialNumberText.text = "";
            doneButtonText.text = "Done";
        }
        else tutorialNumberText.text = (activeTutorialPage + 1).ToString() + "/" + (tutorialCount + 1).ToString();

        textHolderBaseX = tutorialTextHolderRect.localPosition.x;
        DoneButton.onClick.AddListener(NextPage);
        BackButton.onClick.AddListener(PrevPage);
        inputReader.OnScrollDown += ScrollNextPage;
        inputReader.OnScrollUp += PrevPage;
        CloseButton.onClick.AddListener(CloseSelf);
    }

    private void ScrollNextPage()
    {
        if (!DoneButton.interactable || !BackButton.interactable) return;
        if (activeTutorialPage >= tutorialCount) return;

        activeTutorialPage++;
        DoneButton.interactable = false;
        BackButton.interactable = false;

        if (tweenId != -1)
        {
            LeanTween.cancel(tweenId);
            tweenId = -1;
        }

        tweenId = LeanTween.moveX(tutorialTextHolderRect, -activeTutorialPage * maskRectWidth, tutorialTweenTime)
            .setEaseInOutQuart()
            .setOnComplete(() =>
            {
                tweenId = -1;
                BackButton.gameObject.SetActive(true);
                DoneButton.interactable = true;
                BackButton.interactable = true;
                tutorialNumberText.text = (activeTutorialPage + 1).ToString() + "/" + (tutorialCount + 1).ToString();
                if (activeTutorialPage == tutorialCount)
                {
                    doneButtonText.text = "Done";
                }
            }).id;
    }

    private void NextPage()
    {
        if (!DoneButton.interactable || !BackButton.interactable) return;
        if (activeTutorialPage >= tutorialCount)
        {
            CloseSelf();
            return;
        }

        activeTutorialPage++;
        DoneButton.interactable = false;
        BackButton.interactable = false;

        if (tweenId != -1)
        {
            LeanTween.cancel(tweenId);
            tweenId = -1;
        }

        tweenId = LeanTween.moveX(tutorialTextHolderRect, -activeTutorialPage * maskRectWidth, tutorialTweenTime)
            .setEaseInOutQuart()
            .setOnComplete(() =>
            {
                tweenId = -1;
                BackButton.gameObject.SetActive(true);
                DoneButton.interactable = true;
                BackButton.interactable = true;
                tutorialNumberText.text = (activeTutorialPage + 1).ToString() + "/" + (tutorialCount + 1).ToString();
                if (activeTutorialPage == tutorialCount)
                {
                    doneButtonText.text = "Done";
                }
            }).id;
    }

    private void PrevPage()
    {
        if (activeTutorialPage <= 0 || !DoneButton.interactable || !BackButton.interactable) return;

        activeTutorialPage--;
        DoneButton.interactable = false;
        BackButton.interactable = false;

        if (tweenId != -1)
        {
            LeanTween.cancel(tweenId);
            tweenId = -1;
        }

        tweenId = LeanTween.moveX(tutorialTextHolderRect, -activeTutorialPage * maskRectWidth, tutorialTweenTime)
            .setEaseInOutQuart()
            .setOnComplete(() =>
            {
                tweenId = -1;
                doneButtonText.text = "Next";
                DoneButton.interactable = true;
                BackButton.interactable = true;
                tutorialNumberText.text = (activeTutorialPage + 1).ToString() + "/" + (tutorialCount + 1).ToString();
                if (activeTutorialPage == 0)
                {
                    BackButton.gameObject.SetActive(false);
                }
            }).id;
    }

    private void CloseSelf()
    {
        gameObject.SetActive(false);
        DoneButton.onClick.RemoveAllListeners();
        BackButton.onClick.RemoveAllListeners();
        CloseButton.onClick.RemoveAllListeners();
        inputReader.OnScrollDown -= ScrollNextPage;
        inputReader.OnScrollUp -= PrevPage;
        OnTutorialClosed?.Invoke();
        OnTutorialClosed = null;
    }
}