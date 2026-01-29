using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CardHolder : MonoBehaviour
{
    [SerializeField] private RectTransform holderRect;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    [SerializeField] private Button holderButton;
    [SerializeField] private Button toggleButton;
    [SerializeField] private Image holderImage;
    [SerializeField] private float animDuration = 0.5f;
    private const float holderOffset = -22f;
    private RectTransform selfRect;
    private DraggableObject[] draggableChildren;
    private bool isActive = false;
    private bool cardIsDragged = false;
    private bool allCardsHome = true;
    private float activePosY;
    private float inactivePosY;
    private int moveTweenId = -1;
    private Coroutine moveManagerCoroutine;

    public event Action OnHolderShowDone;
    public event Action OnHolderHideStarted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selfRect = GetComponent<RectTransform>();
        draggableChildren = holderRect.GetComponentsInChildren<DraggableObject>();
        activePosY = selfRect.rect.height + transform.position.y + holderOffset;
        inactivePosY = transform.position.y;

        foreach (DraggableObject childDrag in draggableChildren)
        {
            childDrag.enabled = false;
            childDrag.siblingIndex = childDrag.transform.GetSiblingIndex();
            childDrag.OnDragStart += () =>
            {
                layoutGroup.enabled = false;
                cardIsDragged = true;
                allCardsHome = false;
                childDrag.transform.SetAsLastSibling();
            };
            childDrag.OnDragEnd += () =>
            {
                cardIsDragged = false;
                childDrag.transform.SetSiblingIndex(childDrag.siblingIndex);
            };
            childDrag.OnCardReturn += () =>
            {
                if (!cardIsDragged)
                {
                    layoutGroup.enabled = true;
                    StartCoroutine(DoAfterFrame(() => allCardsHome = true));
                }
            };
            OnHolderShowDone += () => childDrag.returnPosition = childDrag.transform.position;
        }

        ActivateHolder();
        // holderButton.onClick.AddListener(ActivateHolder);
        // toggleButton.onClick.AddListener(ActivateHolder);
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
        OnHolderHideStarted?.Invoke();
    }

    private IEnumerator DoAfterFrame(Action action)
    {
        yield return null;
        action?.Invoke();
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
        }
        moveTweenId = -1;
        moveManagerCoroutine = null;

        if (moveUp) OnHolderShowDone?.Invoke();
    }
}
