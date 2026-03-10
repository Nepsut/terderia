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
    [SerializeField] private CanvasGroup reshufflePromptGroup;
    [SerializeField] private Image holderImage;
    [SerializeField] private GameObject cardPrefab;

    private const float cardRehomeTime = 0.32f;
    private const float cardRehomePauseTime = 0.16f;
    private const float holderMoveQueueCheck = 0.1f;
    private readonly WaitForSeconds cardRehomeWait = new(cardRehomeTime);
    private readonly WaitForSeconds cardRehomePauseWait = new(cardRehomePauseTime);
    private readonly WaitForSeconds holderQueueCheckWait = new(holderMoveQueueCheck);
    public const float HolderMoveDuration = 0.5f;
    private const float holderOffset = -40f;
    private const float initialCardRowY = 220f;
    private float cardRowY = initialCardRowY;
    private const float distanceBetweenCards = 305f;
    private const float centerCardX = 747.5f;
    private RectTransform selfRect;
    private List<DraggableObject> draggableChildren;
    private bool HolderMoveAllowed => !IsMoving && !rehomeInProgress && allCardsHome && allCardsDown && moveTweenId == -1;
    private bool isInitialized = false;
    public bool IsActive { get; private set; } = false;
    public bool IsMoving { get; private set; } = false;
    private bool moveQueued = false;
    private bool moveUp = false;
    private bool allCardsHome = true;
    private int cardsDownCount = 0;
    private bool allCardsDown = true;
    private bool reshuffleInProgress = false;
    private bool rehomeInProgress = false;
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
        reshufflePromptGroup.alpha = 0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.OnCardUsed += usedCard =>
        {
            DraggableObject draggableObject = usedCard.GetComponent<DraggableObject>();
            UnsubscribeCardMoveEvents(draggableObject);
            draggableChildren.Remove(draggableObject);

            RehomeCards(CardRehomeStyle.flipNone);
            StartCoroutine(DelayedCardsHomeCheck());
            Destroy(usedCard.gameObject);
        };
        CardManager.OnCardAddedToHand += HandleCardAddition;
        CardManager.OnCardAddedToDeck += _ => SetDeckIndicatorValue();
        reshuffleButton.onClick.AddListener(ReshuffleHand);
        shownDeckValue = CardManager.DeckCardCount;
        deckCountIndicator.text = shownDeckValue.ToString();
    }

    private void SubscribeCardMoveEvents(DraggableObject draggableObject)
    {
        draggableObject.siblingIndex = draggableObject.transform.GetSiblingIndex();
        draggableObject.OnDragStart += HandleCardDragStart;
        draggableObject.OnReturn += HandleCardReturn;
        draggableObject.OnHoverStart += HandleCardStartHover;
        draggableObject.OnHoverEnd += HandleCardStopHover;
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

    private void HandleCardStartHover(DraggableObject draggableObject)
    {
        cardsDownCount--;
        allCardsDown = false;
    }

    private void HandleCardStopHover(DraggableObject draggableObject)
    {
        cardsDownCount++;
        if (cardsDownCount == draggableChildren.Count && cardsDownCount == holderRect.childCount)
        {
            allCardsDown = true;
        }
    }

    public void ActivateHolder()
    {
        if (IsActive && !HolderMoveAllowed)
        {
            if (GameManager.Instance.DebugModeOn) Debug.Log("Queuing card holder activation.");
            moveQueued = true;
            moveUp = true;
            return;
        }

        holderImage.enabled = false;
        if (moveManagerCoroutine != null) StopCoroutine(moveManagerCoroutine);
        if (GameManager.Instance.DebugModeOn) Debug.Log("Activating card holder.");
        moveManagerCoroutine = StartCoroutine(MoveManager(moveUp: true));
    }

    public void DeactivateHolder()
    {
        if (!IsActive && HolderMoveAllowed)
        {
            if (GameManager.Instance.DebugModeOn) Debug.Log("Queuing card holder deactivation.");
            holderImage.enabled = true;
            moveQueued = true;
            moveUp = false;
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
        if (moveUp)
        {
            moveTweenId = LeanTween.moveY(selfRect, activePosY, HolderMoveDuration).setEaseInOutCubic().id;
            reshufflePromptGroup.alpha = 0f;
            LeanTween.value(gameObject, value => reshufflePromptGroup.alpha = value, 0f, 1f, HolderMoveDuration)
                .setEaseInQuart();
        }
        else
        {
            moveTweenId = LeanTween.moveY(selfRect, inactivePosY, HolderMoveDuration).setEaseInOutCubic().id;
            reshufflePromptGroup.alpha = 1f;
            LeanTween.value(gameObject, value => reshufflePromptGroup.alpha = value, 1f, 0f, HolderMoveDuration)
                .setEaseInQuart();
        }
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

        if (moveUp && !isInitialized)
        {
            ClearHand();
            yield return null;
            FillHand();
            yield return null;
            RehomeCards(CardRehomeStyle.flipAll);
        }
        else layoutGroup.enabled = true;
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
        draggableChildren.ForEach(child => SubscribeCardMoveEvents(child));
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

    private void RehomeCards(CardRehomeStyle style)
    {
        if (draggableChildren.Count == 0) return;
        if (cardRehomeCoroutine != null) StopCoroutine(cardRehomeCoroutine);
        allCardsHome = false;
        rehomeInProgress = true;
        cardRehomeCoroutine = StartCoroutine(CardRehomeHandler(style));
    }

    private IEnumerator CardRehomeHandler(CardRehomeStyle style)
    {
        cardRehomeTweens?.ForEach(tweenId => LeanTween.cancel(tweenId));
        cardRehomeTweens = new();
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
            child.AllowMovement();
        });
        if (draggableChildren != null && holderRect.childCount != 0)
            cardRowY = draggableChildren[0].SelfRect.rect.y; 
        allCardsHome = true;
        cardsDownCount = draggableChildren.Count;
        allCardsDown = true;
        yield return null;

        while (moveQueued)
        {
            if (HolderMoveAllowed)
            {
                if (GameManager.Instance.DebugModeOn)
                {
                    Debug.Log("Trying to call queued holder movement.");
                }
                if (moveUp && !IsActive) ActivateHolder();
                else if (!moveUp && IsActive) DeactivateHolder();
            }
            yield return holderQueueCheckWait;
        }

        if (reshuffleInProgress) reshuffleInProgress = false;
        cardRehomeTweens = null;
        cardRehomeCoroutine = null;
        rehomeInProgress = false;
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
