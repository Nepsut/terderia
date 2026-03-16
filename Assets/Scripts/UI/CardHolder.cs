using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CardHolder : MonoBehaviour
{
    [SerializeField] private RectTransform holderRect;
    [SerializeField] private RectTransform draggedCardParent;
    [SerializeField] private RectTransform deckRect;
    [SerializeField] private RectTransform cardClearingParent;
    [SerializeField] private TMP_Text shufflePromptText;
    [SerializeField] private TMP_Text deckCountIndicator;
    [SerializeField] private Image deckImage;
    [SerializeField] private Sprite deckFullSprite;
    [SerializeField] private Sprite deckTwoLeftSprite;
    [SerializeField] private Sprite deckOneLeftSprite;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    [SerializeField] private Button reshuffleButton;
    [SerializeField] private RectTransform reshuffleHolder;
    [SerializeField] private Image holderImage;
    [SerializeField] private GameObject cardPrefab;

    private const float cardRehomeTime = 0.32f;
    private const float cardRehomePauseTime = 0.16f;
    private const float holderMoveQueueCheckTime = 0.1f;
    private const float rehomeQueueCheckTime = 0.1f;
    private readonly WaitForSeconds cardRehomeWait = new(cardRehomeTime);
    private readonly WaitForSeconds cardRehomePauseWait = new(cardRehomePauseTime);
    private readonly WaitForSeconds holderQueueCheckWait = new(holderMoveQueueCheckTime);
    private readonly WaitForSeconds rehomeQueueCheckWait = new(rehomeQueueCheckTime);
    public const float HolderMoveDuration = 0.5f;
    private const float holderOffset = -40f;
    private const float initialCardRowY = 220f;
    private float cardRowY = initialCardRowY;
    private const float distanceBetweenCards = 305f;
    private const float centerCardX = 747.5f;
    private RectTransform selfRect;
    private List<DraggableObject> draggableChildren;
    private bool HolderMoveAllowed => !IsMoving && !rehomeInProgress && allCardsHome && AreAllCardsDown() && moveTweenId == -1;
    private bool isInitialized = false;
    public bool IsActive { get; private set; } = false;
    public bool IsMoving { get; private set; } = false;
    private bool moveQueued = false;
    private bool moveUp = false;
    private bool allCardsHome = true;
    private bool reshuffleInProgress = false;
    private bool rehomeInProgress = false;
    private const float shuffleBgMoveDuration = 0.32f;
    private const float shuffleBgVisibleY = 273f;
    private const float shuffleBgHiddenY = 146f;
    private int shownDeckValue = 0;
    private float activePosY;
    private float inactivePosY;
    private int moveTweenId = -1;
    private Coroutine moveManagerCoroutine;
    private List<int> cardRehomeTweens;
    private Coroutine cardRehomeCoroutine;

    private enum CardRehomeStyle
    {
        flipNone = 0,
        flipLast = 1,
        flipAll = 2
    }

    private void Awake()
    {
        selfRect = GetComponent<RectTransform>();
        activePosY = selfRect.rect.height + selfRect.anchoredPosition.y + holderOffset;
        inactivePosY = selfRect.anchoredPosition.y;
        reshuffleHolder.anchoredPosition = new(reshuffleHolder.anchoredPosition.x, shuffleBgHiddenY);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.OnCardUsed += usedCard =>
        {
            DraggableObject draggableObject = usedCard.GetComponent<DraggableObject>();
            UnsubscribeCardMoveEvents(draggableObject);
            draggableChildren.Remove(draggableObject);
            reshuffleButton.interactable = false;
            RehomeCards(CardRehomeStyle.flipNone, false);
            draggableChildren?.ForEach(child => child.DisallowMovement());
            StartCoroutine(DelayedCardsHomeCheck());
            Destroy(usedCard.gameObject);
        };
        CardManager.OnCardAddedToHand += HandleCardAddition;
        CardManager.OnCardAddedToDeck += _ => SetDeckIndicatorValue();
        reshuffleButton.onClick.AddListener(ReshuffleHand);
        shownDeckValue = CardManager.DeckCardCount;
        deckCountIndicator.text = shownDeckValue.ToString();
    }

    public bool AreAllCardsDown()
    {
        if (draggableChildren == null) return true;
        return draggableChildren.TrueForAll(draggable => draggable.IsAtRestPosition);
    }

    private void SubscribeCardMoveEvents(DraggableObject draggableObject)
    {
        draggableObject.siblingIndex = draggableObject.transform.GetSiblingIndex();
        draggableObject.OnDragStart += HandleCardDragStart;
        draggableObject.OnReturn += HandleCardReturn;
    }

    private void UnsubscribeCardMoveEvents(DraggableObject draggableObject)
    {
        draggableObject.siblingIndex = draggableObject.transform.GetSiblingIndex();
        draggableObject.OnDragStart -= HandleCardDragStart;
        draggableObject.OnReturn -= HandleCardReturn;
    }

    private void HandleCardDragStart(DraggableObject draggableObject)
    {
        layoutGroup.enabled = false;
        allCardsHome = false;
        draggableObject.transform.SetParent(draggedCardParent);
    }

    private void HandleCardReturn(DraggableObject draggableObject)
    {
        draggableObject.transform.SetParent(holderRect);
        draggableObject.transform.SetSiblingIndex(draggableObject.siblingIndex);
        if (draggableChildren.Count == holderRect.childCount) allCardsHome = true;
    }

    public void AllowCardDragging()
    {
        draggableChildren?.ForEach(child => child.AllowMovement());
    }

    public void ActivateHolder()
    {
        if (GameManager.Instance.DebugModeOn)
        {
            string message = string.Concat($"Trying to activate CardHolder. States of relevant ",
            $"variables: IsMoving = {IsMoving}, rehomeInProgress = {rehomeInProgress}, ",
            $"allCardsHome = {allCardsHome}, AreAllCardsDown = {AreAllCardsDown()}, ",
            $"moveTweenId = {moveTweenId}. Holder allowed to move: {HolderMoveAllowed}.");
            Debug.Log(message);
        }
        reshuffleButton.interactable = false;

        if (IsActive && !HolderMoveAllowed)
        {
            if (GameManager.Instance.DebugModeOn) Debug.Log("Queuing card holder activation.");
            moveQueued = true;
            moveUp = true;
        }

        holderImage.enabled = false;
        if (moveManagerCoroutine != null) StopCoroutine(moveManagerCoroutine);
        if (GameManager.Instance.DebugModeOn) Debug.Log("Activating card holder.");
        moveManagerCoroutine = StartCoroutine(MoveManager(moveUp: true));
    }

    public void DeactivateHolder()
    {
        if (GameManager.Instance.DebugModeOn)
        {
            string message = string.Concat($"Trying to deactivate CardHolder. States of relevant ",
            $"variables: IsMoving = {IsMoving}, rehomeInProgress = {rehomeInProgress}, ",
            $"allCardsHome = {allCardsHome}, AreAllCardsDown = {AreAllCardsDown()}, ",
            $"moveTweenId = {moveTweenId}. Holder allowed to move: {HolderMoveAllowed}.");
            Debug.Log(message);
        }
        reshuffleButton.interactable = false;

        if (!IsActive || !HolderMoveAllowed)
        {
            if (GameManager.Instance.DebugModeOn) Debug.Log("Queuing card holder deactivation.");
            holderImage.enabled = true;
            moveQueued = true;
            moveUp = false;
            StartCoroutine(HandleQueuedHolderMove());
            return;
        }

        holderImage.enabled = true;
        if (moveManagerCoroutine != null) StopCoroutine(moveManagerCoroutine);
        if (GameManager.Instance.DebugModeOn) Debug.Log("Deactivating card holder.");
        moveManagerCoroutine = StartCoroutine(MoveManager(moveUp: false));
    }

    private IEnumerator MoveManager(bool moveUp)
    {
        IsMoving = true;
        moveQueued = false;
        if (moveTweenId != -1) LeanTween.cancel(moveTweenId);

        if (moveUp) moveTweenId = LeanTween.moveY(selfRect, activePosY, HolderMoveDuration).setEaseInOutCubic().id;
        else moveTweenId = LeanTween.moveY(selfRect, inactivePosY, HolderMoveDuration).setEaseInOutCubic().id;
        layoutGroup.enabled = true;
        yield return new WaitForSeconds(HolderMoveDuration);
        if (draggableChildren != null)
        {
            foreach (DraggableObject childDrag in draggableChildren)
            {
                childDrag.enabled = moveUp;
                childDrag.SetReturnPosition();
            }
        }
        moveTweenId = -1;
        moveManagerCoroutine = null;
        layoutGroup.enabled = false;

        if (moveUp)
        {
            LeanTween.moveY(reshuffleHolder, shuffleBgVisibleY, shuffleBgMoveDuration)
            .setEaseInQuart()
            .setOnComplete(() => reshuffleButton.interactable = true);
        }
        else
        {
            reshuffleButton.interactable = false;
            LeanTween.moveY(reshuffleHolder, shuffleBgHiddenY, shuffleBgMoveDuration)
            .setEaseInQuart();
        }

        if (moveUp && !isInitialized)
        {
            ClearHand();
            yield return null;
            FillHand();
            yield return null;
            RehomeCards(CardRehomeStyle.flipAll);
        }
        else
        {
            layoutGroup.enabled = true;
            AllowCardDragging();
        }
        IsActive = moveUp;
        IsMoving = false;
    }

    private void HandleCardAddition(CardData addedCard)
    {
        Card card = Instantiate(cardPrefab, deckRect.transform.position, Quaternion.identity, holderRect).GetComponent<Card>();
        card.InitializeCard(addedCard);
        SubscribeCardMoveEvents(card.GetComponent<DraggableObject>());
        draggableChildren = holderRect.GetComponentsInChildren<DraggableObject>().ToList();
        if (IsActive) RehomeCards(CardRehomeStyle.flipLast);
    }

    private void FillHand()
    {
        foreach (CardData cardData in CardManager.PlayerHand)
        {
            Card card = Instantiate(cardPrefab, deckRect.transform.position, Quaternion.identity, holderRect).GetComponent<Card>();
            card.InitializeCard(cardData);
            card.SelfImage.sprite = CardManager.Instance.Cardback;
            card.transform.GetChild(0).gameObject.SetActive(false);
        }
        draggableChildren = holderRect.GetComponentsInChildren<DraggableObject>().ToList();
        draggableChildren.ForEach(child => 
        {
            SubscribeCardMoveEvents(child);
            child.DisallowMovement();
        });
        shownDeckValue = CardManager.TotalCardCount;
        deckCountIndicator.text = shownDeckValue.ToString();
        isInitialized = true;
    }

    public void ClearHand()
    {
        isInitialized = false;
        draggableChildren?.ForEach(child => UnsubscribeCardMoveEvents(child));
        draggableChildren?.Clear();
        foreach (Transform child in holderRect)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in cardClearingParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void ReshuffleHand()
    {
        if (!allCardsHome || reshuffleInProgress || !IsActive) return;
        reshuffleInProgress = true;
        reshuffleButton.interactable = false;
        StartCoroutine(CardReshuffleHandler());
    }

    private IEnumerator CardReshuffleHandler()
    {
        CardManager.ReshufflePlayerHand();
        layoutGroup.enabled = false;

        foreach (DraggableObject draggable in draggableChildren)
        {
            draggable.DisallowMovement();
            draggable.transform.SetParent(cardClearingParent);
            LeanTween.move(draggable.gameObject, deckRect.transform.position, cardRehomeTime)
                .setEaseInQuart();
            LeanTween.scaleX(draggable.gameObject, 0f, cardRehomeTime / 2f)
                .setEaseInOutQuad()
                .setOnComplete(() => 
                {
                    draggable.SelfImage.sprite = CardManager.Instance.Cardback;
                    draggable.transform.GetChild(0).gameObject.SetActive(false);
                });
            LeanTween.scaleX(draggable.gameObject, 1f, cardRehomeTime / 2f)
                .setEaseInOutQuad()
                .setDelay(cardRehomeTime / 2);
            shownDeckValue += 2;
            CorrectDeckIndicatorByOne();
            yield return cardRehomePauseWait;
        }

        yield return cardRehomeWait;

        ClearHand();
        yield return null;
        FillHand();
        yield return null;
        RehomeCards(CardRehomeStyle.flipAll);
    }

    private void RehomeCards(CardRehomeStyle style, bool allowMovement = true)
    {
        if (draggableChildren.Count == 0) return;
        if (cardRehomeCoroutine != null) StopCoroutine(cardRehomeCoroutine);
        rehomeInProgress = true;
        cardRehomeCoroutine = StartCoroutine(CardRehomeHandler(style, allowMovement));
    }

    private IEnumerator CardRehomeHandler(CardRehomeStyle style, bool allowMovement = true)
    {
        cardRehomeTweens?.ForEach(tweenId => LeanTween.cancel(tweenId));
        cardRehomeTweens = new();

        while (!allCardsHome)
        {
            yield return rehomeQueueCheckWait;
        }
        
        allCardsHome = false;

        int cardCount = draggableChildren.Count;
        if (GameManager.Instance.DebugModeOn) Debug.Log($"Rehoming {cardCount} cards.");
        float firstX = centerCardX - distanceBetweenCards * (cardCount / 2);
        if (cardCount % 2 == 0) firstX += distanceBetweenCards / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            draggableChildren[i].DisallowMovement();
            float realY = draggableChildren[i].IsReturnPositionSet ? cardRowY : initialCardRowY;
            Vector2 targetPosition = new(firstX + i * distanceBetweenCards, realY);
            if (GameManager.Instance.DebugModeOn) Debug.Log($"Rehoming a card to {targetPosition}");
            cardRehomeTweens.Add(LeanTween.move(draggableChildren[i].SelfRect, targetPosition, cardRehomeTime)
                                .setEaseInOutQuart().id);
            CorrectDeckIndicatorByOne();

            if (style == CardRehomeStyle.flipAll || (style == CardRehomeStyle.flipLast && i == cardCount - 1))
            {
                cardRehomeTweens.Add(LeanTween.scaleX(draggableChildren[i].gameObject, 0f, cardRehomeTime / 2f)
                                    .setEaseInOutQuart()
                                    .setOnComplete(() => 
                                    {
                                        draggableChildren[i].transform.GetChild(0).gameObject.SetActive(true);
                                        draggableChildren[i].GetComponent<Card>().ResetBackgroundImage();
                                    }).id);
                cardRehomeTweens.Add(LeanTween.scaleX(draggableChildren[i].gameObject, 1f, cardRehomeTime / 2f)
                                    .setEaseInOutQuart()
                                    .setDelay(cardRehomeTime / 2f).id);
                yield return cardRehomePauseWait;
            }
        }

        yield return cardRehomeWait;
        layoutGroup.enabled = false;
        layoutGroup.enabled = true;
        yield return null;
        draggableChildren?.ForEach(child =>
        {
            child.SetReturnPosition();
            child.enabled = true;
            if (allowMovement) child.AllowMovement();
        });
        if (draggableChildren != null && holderRect.childCount != 0)
            cardRowY = draggableChildren[0].SelfRect.rect.y; 
        allCardsHome = true;
        yield return null;

        if (reshuffleInProgress) reshuffleInProgress = false;
        cardRehomeTweens = null;
        cardRehomeCoroutine = null;
        rehomeInProgress = false;
        if (!moveQueued && EventManager.Instance.DialogueHasChoices) reshuffleButton.interactable = true;
    }

    private IEnumerator HandleQueuedHolderMove()
    {
        while (moveQueued)
        {
            yield return holderQueueCheckWait;

            if (GameManager.Instance.DebugModeOn)
                Debug.Log("Running queued CardHolder move check.");
            
            if (HolderMoveAllowed)
            {
                if (GameManager.Instance.DebugModeOn)
                {
                    Debug.Log("Trying to call queued holder movement.");
                }
                if (moveUp && !IsActive) ActivateHolder();
                else if (!moveUp && IsActive) DeactivateHolder();
            }
        }
    }

    private void CorrectDeckIndicatorByOne()
    {
        if (shownDeckValue < CardManager.DeckCardCount)
        {
            shownDeckValue++;
            deckCountIndicator.text = shownDeckValue.ToString();
            deckImage.enabled = true;
        }
        else if (shownDeckValue > CardManager.DeckCardCount)
        {
            shownDeckValue--;
            deckCountIndicator.text = shownDeckValue == 0 ? "" : shownDeckValue.ToString();
            if (shownDeckValue <= 0) deckImage.enabled = false;
            else deckImage.enabled = true;
        }

        deckImage.sprite = shownDeckValue switch
        {
            1 => deckOneLeftSprite,
            2 | 3 => deckTwoLeftSprite,
            _ => deckFullSprite
        };
    }

    public void SetDeckIndicatorValue()
    {
        shownDeckValue = CardManager.DeckCardCount;
        deckCountIndicator.text = shownDeckValue.ToString();
    }

    private IEnumerator DelayedCardsHomeCheck()
    {
        yield return null;
        if (GameManager.Instance.DebugModeOn)
        {
            string message = string.Concat($"Draggable children count {draggableChildren.Count}, ",
            $"holder children count {holderRect.childCount}. \nCards were already home: {allCardsHome}");
            Debug.Log(message);
        }
        if (draggableChildren.Count == holderRect.childCount) allCardsHome = true;
    }
}
