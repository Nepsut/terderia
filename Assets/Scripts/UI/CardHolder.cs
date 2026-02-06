using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CardHolder : MonoBehaviour
{
    [SerializeField] private RectTransform holderRect;
    [SerializeField] private RectTransform draggedCardParent;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    [SerializeField] private Image holderImage;
    [SerializeField] private float animDuration = 0.5f;
    [SerializeField] private float cardRehomeTime = 0.32f;
    private float cardRowY = 0;
    private const float distanceBetweenCards = 315f;
    private const float centerCardX = 747.5f;
    private RectTransform selfRect;
    private List<DraggableObject> draggableChildren;
    private bool isActive = false;
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
        draggableChildren = holderRect.GetComponentsInChildren<DraggableObject>().ToList();
        activePosY = selfRect.rect.height + selfRect.anchoredPosition.y;
        inactivePosY = selfRect.anchoredPosition.y;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (DraggableObject childDrag in draggableChildren)
        {
            childDrag.enabled = false;
            SubscribeCardMoveEvents(childDrag);
        }

        // ActivateHolder();
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
        if (isActive || !allCardsHome || moveTweenId != -1)
        {
            moveQueued = true;
            moveUp = true;
            return;
        }

        isActive = true;
        holderImage.enabled = false;
        if (moveManagerCoroutine != null) StopCoroutine(moveManagerCoroutine);
        moveManagerCoroutine = null;
        moveManagerCoroutine = StartCoroutine(MoveManager(moveUp: true));
    }

    public void DeactivateHolder()
    {
        if (!isActive || !allCardsHome || moveTweenId != -1)
        {
            moveQueued = true;
            moveUp = false;
            return;
        }

        isActive = false;
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
        foreach (DraggableObject childDrag in draggableChildren)
        {
            childDrag.enabled = moveUp;
            childDrag.SetReturnPosition();
        }
        moveTweenId = -1;
        moveManagerCoroutine = null;
        layoutGroup.enabled = false;
        layoutGroup.enabled = true;

        if (moveUp)
        {
            if (cardRowY == 0) cardRowY = holderRect.GetChild(0).GetComponent<RectTransform>().rect.y;
        }
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
        draggableChildren.ForEach(child =>
        {
            child.SetReturnPosition();
            child.enabled = true;
            child.AllowMovement();
        });
        allCardsHome = true;
        yield return null;
        layoutGroup.enabled = true;
        if (moveQueued && moveUp) ActivateHolder();
        else if (moveQueued && !moveUp) DeactivateHolder();
        // layoutGroup.enabled = false;
        cardRehomeTweens = null;
        cardRehomeCoroutine = null;
    }
}
