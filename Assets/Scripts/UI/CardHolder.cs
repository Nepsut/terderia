using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardSystem;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CardHolder : MonoBehaviour
{
    [SerializeField] private RectTransform holderRect;
    [SerializeField] private RectTransform draggedCardParent;
    [SerializeField] private RectTransform deckRect;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    [SerializeField] private Image holderImage;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private float animDuration = 0.5f;
    [SerializeField] private float cardRehomeTime = 0.32f;
    private const float holderOffset = -40f;
    private float cardRowY = 220;
    private const float distanceBetweenCards = 305f;
    private const float centerCardX = 747.5f;
    private RectTransform selfRect;
    private List<DraggableObject> draggableChildren;
    private bool isInitialized = false;
    public bool IsActive { get; private set; } = false;
    private bool moveQueued = false;
    private bool moveUp = false;
    private bool allCardsHome = true;
    private float activePosY;
    private float inactivePosY;
    private int moveTweenId = -1;
    private Coroutine moveManagerCoroutine;
    private List<int> cardRehomeTweens;
    private Coroutine cardRehomeCoroutine;

    private void Awake()
    {
        selfRect = GetComponent<RectTransform>();
        activePosY = selfRect.rect.height + selfRect.anchoredPosition.y + holderOffset;
        inactivePosY = selfRect.anchoredPosition.y;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.OnCardUsed += usedCard =>
        {
            draggableChildren.Remove(usedCard.GetComponent<DraggableObject>());
            RehomeCards();
        };
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
    }

    public void ActivateHolder()
    {
        if (IsActive || !allCardsHome || moveTweenId != -1)
        {
            moveQueued = true;
            moveUp = true;
            return;
        }

        IsActive = true;
        holderImage.enabled = false;
        if (moveManagerCoroutine != null) StopCoroutine(moveManagerCoroutine);
        moveManagerCoroutine = null;
        moveManagerCoroutine = StartCoroutine(MoveManager(moveUp: true));
    }

    public void DeactivateHolder()
    {
        if (!IsActive || !allCardsHome || moveTweenId != -1)
        {
            moveQueued = true;
            moveUp = false;
            return;
        }

        IsActive = false;
        holderImage.enabled = true;
        if (moveManagerCoroutine != null) StopCoroutine(moveManagerCoroutine);
        moveManagerCoroutine = null;
        moveManagerCoroutine = StartCoroutine(MoveManager(moveUp: false));
    }

    private IEnumerator MoveManager(bool moveUp)
    {
        if (moveTweenId != -1) LeanTween.cancel(moveTweenId);
        if (moveUp) moveTweenId = LeanTween.moveY(selfRect, activePosY, animDuration).setEaseInOutCubic().id;
        else moveTweenId = LeanTween.moveY(selfRect, inactivePosY, animDuration).setEaseInOutCubic().id;
        layoutGroup.enabled = true;
        yield return new WaitForSeconds(animDuration);
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
            foreach (Transform child in holderRect) Destroy(child.gameObject);
            foreach (CardData cardData in CardManager.PlayerHand)
            {
                Card card = Instantiate(cardPrefab, deckRect.transform.position, Quaternion.identity, holderRect).GetComponent<Card>();
                card.InitializeCard(cardData);
            }
            draggableChildren = holderRect.GetComponentsInChildren<DraggableObject>().ToList();
            draggableChildren.ForEach(child => SubscribeCardMoveEvents(child));
            RehomeCards();
            isInitialized = true;
        }
        else layoutGroup.enabled = true;
    }

    public void RehomeCards()
    {
        if (draggableChildren.Count == 0) return;
        if (cardRehomeCoroutine != null) StopCoroutine(cardRehomeCoroutine);
        cardRehomeCoroutine = StartCoroutine(CardRehomeHandler());
    }

    private IEnumerator CardRehomeHandler()
    {
        cardRehomeTweens?.ForEach(tweenId => LeanTween.cancel(tweenId));
        cardRehomeTweens = new();
        int cardCount = holderRect.childCount;
        float firstX = centerCardX - distanceBetweenCards * (cardCount / 2);
        if (cardCount % 2 == 0) firstX += distanceBetweenCards / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            draggableChildren[i].DisallowMovement();
            Vector2 targetPosition = new(firstX + i * distanceBetweenCards, cardRowY);
            cardRehomeTweens.Add(LeanTween.move(draggableChildren[i].GetComponent<RectTransform>(), targetPosition, cardRehomeTime)
                                .setEaseInOutQuart().id);
        }

        yield return new WaitForSeconds(cardRehomeTime);
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
            cardRowY = holderRect.GetChild(0).GetComponent<RectTransform>().rect.y; 
        allCardsHome = true;
        yield return null;
        if (moveQueued && moveUp) ActivateHolder();
        else if (moveQueued && !moveUp) DeactivateHolder();
        // layoutGroup.enabled = false;
        cardRehomeTweens = null;
        cardRehomeCoroutine = null;
    }
}
