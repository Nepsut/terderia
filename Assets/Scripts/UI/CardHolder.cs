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
    [SerializeField] private Button holderButton;
    [SerializeField] private Button toggleButton;
    [SerializeField] private Image holderImage;
    [SerializeField] private float animDuration = 0.5f;
    [SerializeField] private float cardRehomeTime = 0.32f;
    private float cardRowY = 0;
    private const float distanceBetweenCards = 264.33f;
    private const float holderOffset = -22f;
    private RectTransform selfRect;
    private List<DraggableObject> draggableChildren;
    private bool isActive = false;
    private bool allCardsHome = true;
    private float activePosY;
    private float inactivePosY;
    private int moveTweenId = -1;
    private Coroutine moveManagerCoroutine;
    private List<int> cardRehomeTweens;
    private Coroutine cardRehomeCoroutine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selfRect = GetComponent<RectTransform>();
        draggableChildren = holderRect.GetComponentsInChildren<DraggableObject>().ToList();
        activePosY = selfRect.rect.height + transform.position.y + holderOffset;
        inactivePosY = transform.position.y;

        foreach (DraggableObject childDrag in draggableChildren)
        {
            childDrag.enabled = false;
            SubscribeCardMoveEvents(childDrag);
        }

        ActivateHolder();
        EventManager.OnCardUsed += usedCard =>
        {
            draggableChildren.Remove(usedCard.GetComponent<DraggableObject>());
            RehomeCards();
        };
        // holderButton.onClick.AddListener(ActivateHolder);
        // toggleButton.onClick.AddListener(ActivateHolder);
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

    private void ActivateHolder()
    {
        if (isActive || !allCardsHome || moveTweenId != -1) return;

        isActive = true;
        holderImage.enabled = false;
        holderButton.enabled = false;
        toggleButton.onClick.RemoveListener(ActivateHolder);
        toggleButton.onClick.AddListener(DeactivateHolder);
        if (moveManagerCoroutine != null) StopCoroutine(moveManagerCoroutine);
        moveManagerCoroutine = null;
        moveManagerCoroutine = StartCoroutine(MoveManager(moveUp: true));
    }

    public void DeactivateHolder()
    {
        if (!isActive || !allCardsHome || moveTweenId != -1) return;

        isActive = false;
        holderImage.enabled = true;
        holderButton.enabled = true;
        toggleButton.onClick.AddListener(ActivateHolder);
        toggleButton.onClick.RemoveListener(DeactivateHolder);
        if (moveManagerCoroutine != null) StopCoroutine(moveManagerCoroutine);
        moveManagerCoroutine = null;
        moveManagerCoroutine = StartCoroutine(MoveManager(moveUp: false));
    }

    private IEnumerator MoveManager(bool moveUp)
    {
        if (moveTweenId == -1) LeanTween.cancel(moveTweenId);
        if (moveUp) moveTweenId = LeanTween.moveY(selfRect, activePosY, animDuration).setEaseInOutCubic().id;
        else moveTweenId = LeanTween.moveY(selfRect, inactivePosY, animDuration).setEaseInOutCubic().id;
        yield return new WaitForSeconds(animDuration);
        foreach (DraggableObject childDrag in draggableChildren)
        {
            childDrag.enabled = moveUp;
            childDrag.returnPosition = childDrag.transform.position;
        }
        moveTweenId = -1;
        moveManagerCoroutine = null;

        if (moveUp)
        {
            if (cardRowY == 0) cardRowY = holderRect.GetChild(0).transform.position.y;
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
        float firstX = holderRect.position.x - distanceBetweenCards * (cardCount / 2);
        if (cardCount % 2 == 0) firstX += distanceBetweenCards / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            draggableChildren[i].DisallowMovement();
            Vector2 targetPosition = new(firstX + i * distanceBetweenCards, cardRowY);
            cardRehomeTweens.Add(LeanTween.move(draggableChildren[i].gameObject, targetPosition, cardRehomeTime)
                                .setEaseInOutQuart().id);
        }

        yield return new WaitForSeconds(cardRehomeTime);
        layoutGroup.enabled = true;
        draggableChildren.ForEach(child =>
        {
            child.returnPosition = child.transform.position;
            child.enabled = true;
            child.AllowMovement();
        });
        yield return null;
        layoutGroup.enabled = false;
        cardRehomeTweens = null;
        cardRehomeCoroutine = null;
    }
}
