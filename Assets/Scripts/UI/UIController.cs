using System.Collections;
using System.Collections.Generic;
using CardSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoSingleton<UIController>
{
    [SerializeField] private InputReader inputReader;

    [Space, Header("Card Reward Elements")]
    [SerializeField] private RectTransform cardRewardBackground;
    [SerializeField] private CanvasGroup cardRewardGroup;
    [SerializeField] private RectTransform cardRewardPanel;
    [SerializeField] private Transform cardRewardHolder;
    [SerializeField] private ContentSizeFitter cardRewardPanelFitter;
    [SerializeField] private TMP_Text cardRewardHeader;
    [SerializeField] private TMP_Text cardRewardMessage;
    [SerializeField] private CardReward[] rewardCards;
    [SerializeField] private RectTransform cardAnimParent;
    [SerializeField] private RectTransform cardDeckRect;
    [SerializeField] private Button continueFromRewardsButton;
    private RectTransform continueFromRewardsRect;
    private const string defaultHeaderMessage = "New cards unlocked!";
    private const string allUnlockedHeaderMessage = "All rewards already unlocked!";
    private const string chooseOneMessage = "Choose one card.";
    private const string blankChooseMessage = "Choose up to _ cards.";
    public static bool CardRewardsOpen { get; private set; } = false;
    private int currentRewardsCount = 0;
    private int choosableRewardCount = 0;
    private int selectedRewardCount = 0;
    private const float cardFloatTime = 0.40f;
    private WaitForSeconds cardFloatWait;
    private const float cardFloatAmount = 36f;
    private const float cardTravelToDeckTime = 0.80f;
    private WaitForSeconds cardToDeckWait;
    private const float rewardPanelFadeTime = 0.32f;

    public InputReader MainInputReader => inputReader;
    private static Vector2 _mousePos;
    public static Vector2 MousePosition => _mousePos;
    public static bool IsMenuOpen { get; private set; } = false;

    public static bool PauseEventProgression => CardRewardsOpen;

    private void Start()
    {
        inputReader.OnPointEvent += pos => _mousePos = pos;
        cardFloatWait = new(cardFloatTime);
        cardToDeckWait = new(cardTravelToDeckTime);
        continueFromRewardsButton.onClick.AddListener(HandleRewardsContinue);
        continueFromRewardsRect = continueFromRewardsButton.GetComponent<RectTransform>();
        cardRewardBackground.gameObject.SetActive(false);
    }

    public void HandleCardRewards(int choiceAmount, string[] cardIds)
    {
        if (choiceAmount < 1 || cardIds == null || cardIds.Length < 1)
        {
            if (GameManager.Instance.DebugModeOn)
            {
                Debug.LogWarning(
                    $"HandleCardRewards was called with invalid parameter! Amount {choiceAmount}, ids {cardIds}");
            }
            return;
        }

        List<string> realUnlockableCards = new();

        for (int i = 0; i < cardIds.Length; i++)
        {
            if (!CardManager.IsCardUnlocked(cardIds[i]))
            {
                realUnlockableCards.Add(cardIds[i]);
            }
        }

        if (realUnlockableCards.Count == 0) cardRewardHeader.text = allUnlockedHeaderMessage;
        else cardRewardHeader.text = defaultHeaderMessage;

        if (realUnlockableCards.Count > choiceAmount)
        {
            if (choiceAmount == 1)
                cardRewardMessage.text = chooseOneMessage;
            else
                cardRewardMessage.text = blankChooseMessage.Replace("_", choiceAmount.ToString());
        }
        else cardRewardMessage.text = "";

        currentRewardsCount = realUnlockableCards.Count;
        choosableRewardCount = choiceAmount;
        selectedRewardCount = 0;

        for (int i = 0; i < rewardCards.Length; i++)
        {
            rewardCards[i].DeselectCard();
            if (i >= realUnlockableCards.Count)
            {
                rewardCards[i].gameObject.SetActive(false);
                continue;
            }

            rewardCards[i].InitializeCard(realUnlockableCards[i]);
            rewardCards[i].gameObject.SetActive(true);

            if (currentRewardsCount >= choosableRewardCount)
            {
                rewardCards[i].SelectCard();
                rewardCards[i].SelfButton.interactable = false;
                selectedRewardCount++;
            }
            else
            {
                rewardCards[i].OnRewardSelected += HandleRewardSelected;
                rewardCards[i].OnRewardDeselected += HandleRewardDeselected;
                rewardCards[i].SelfButton.interactable = true;
            }

        }

        continueFromRewardsButton.interactable = false;
        CardRewardsOpen = true;
        cardRewardBackground.gameObject.SetActive(true);
        cardRewardGroup.alpha = 0f;
        LeanTween.value(gameObject, value => cardRewardGroup.alpha = value, 0f, 1f, rewardPanelFadeTime)
            .setEaseInQuart()
            .setOnComplete(() =>
            {
                continueFromRewardsButton.interactable = currentRewardsCount >= choosableRewardCount;
                foreach (CardReward cardReward in rewardCards)
                {
                    cardReward.SelfButton.interactable = currentRewardsCount < choosableRewardCount;
                }
            });
    }

    private void HandleRewardSelected()
    {
        selectedRewardCount++;

        if (selectedRewardCount < choosableRewardCount)
        {
            foreach (CardReward cardReward in rewardCards)
            {
                cardReward.SelfButton.interactable = true;
            }
            continueFromRewardsButton.interactable = false;
            return;
        }

        foreach (CardReward cardReward in rewardCards)
        {
            cardReward.SelfButton.interactable = cardReward.IsSelected;
            continueFromRewardsButton.interactable = true;
        }
    }

    private void HandleRewardDeselected()
    {
        selectedRewardCount--;

        foreach (CardReward cardReward in rewardCards)
        {
            cardReward.SelfButton.interactable = true;
        }
        continueFromRewardsButton.interactable = false;
    }

    private void HandleRewardsContinue()
    {
        if (selectedRewardCount < choosableRewardCount) return;

        continueFromRewardsButton.interactable = false;
        cardRewardPanelFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        bool[] wasCardAddedToDeck = new bool[currentRewardsCount];

        for (int i = 0; i < currentRewardsCount; i++)
        {
            if (rewardCards[i].IsSelected)
            {
                wasCardAddedToDeck[i] = !CardManager.IsDeckFull;
                CardManager.UnlockCard(rewardCards[i].CardData.id, addToDeck: true);
            }
            rewardCards[i].OnRewardSelected -= HandleRewardSelected;
            rewardCards[i].OnRewardDeselected -= HandleRewardDeselected;
        }

        StartCoroutine(HandleRewardCardAnimations(wasCardAddedToDeck));
    }

    private IEnumerator HandleRewardCardAnimations(bool[] wasCardAddedToDeck)
    {
        for (int i = 0; i < currentRewardsCount; i++)
        {
            rewardCards[i].transform.SetParent(cardAnimParent);

            if (rewardCards[i].IsSelected && wasCardAddedToDeck[i])
            {
                LeanTween.moveY(rewardCards[i].SelfRect, 
                                rewardCards[i].SelfRect.anchoredPosition.y + cardFloatAmount, cardFloatTime)
                                .setEaseInOutQuart();
                LeanTween.move(rewardCards[i].gameObject, cardDeckRect.transform.position, cardTravelToDeckTime)
                    .setEaseInQuart()
                    .setDelay(cardFloatTime);
            }
            else if (rewardCards[i].IsSelected)
            {
                LeanTween.moveY(rewardCards[i].SelfRect, 
                                rewardCards[i].SelfRect.anchoredPosition.y + cardFloatAmount, cardFloatTime)
                                .setEaseInOutQuart()
                                .setOnComplete(() => rewardCards[i].StartFadeOut(cardFloatTime));
                
            }
            else if (!rewardCards[i].IsSelected)
            {
                LeanTween.moveY(rewardCards[i].SelfRect, 
                                rewardCards[i].SelfRect.anchoredPosition.y - cardFloatAmount, cardFloatTime)
                                .setEaseInQuart();
                rewardCards[i].StartFadeOut(cardFloatTime);
            }
        }

        yield return cardFloatWait;

        LeanTween.value(gameObject, value => cardRewardGroup.alpha = value, 1f, 0f, rewardPanelFadeTime).setEaseOutQuart();

        yield return cardToDeckWait;

        foreach (CardReward cardReward in rewardCards)
        {
            cardReward.transform.SetParent(cardRewardHolder);
        }

        cardRewardPanelFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        cardRewardBackground.gameObject.SetActive(false);
        CardRewardsOpen = false;
    }
}