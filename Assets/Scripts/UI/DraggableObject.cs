using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DraggableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float returnTime = 0.74f;
    [SerializeField] private float hoverOffsetHorizontal;
    [SerializeField] private float hoverOffsetVertical;
    private Vector2 hoverOffsetVector => new(hoverOffsetHorizontal, hoverOffsetVertical);
    [SerializeField] private float hoverAnimTime = 0.32f;
    [HideInInspector] public int siblingIndex;
    private RectTransform selfRect;
    private InputReader inputReader;
    private bool draggingAllowed = true;
    private bool hoverAllowed = true;
    private Vector2 mousePos => UIController.MousePosition;
    private bool _pointerOnObject = false;
    private bool _draggingOn = false;
    public Vector2 returnPosition;
    private int hoverTweenId = -1;
    private int dragTweenId = -1;
    private Coroutine resetHoverCoroutine;
    private Coroutine resetDragCoroutine;

    public event Action<DraggableObject> OnDragStart;
    public event Action<DraggableObject> OnDragEnd;
    public event Action<DraggableObject> OnReturn;
    public event Action<DraggableObject> OnHoverStart;
    public event Action<DraggableObject> OnHoverEnd;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        _pointerOnObject = true;

        if (hoverAllowed && (hoverOffsetHorizontal != 0f || hoverOffsetVertical != 0))
        {
            StartHover();
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        _pointerOnObject = false;

        if (hoverAllowed && (hoverOffsetHorizontal != 0f || hoverOffsetVertical != 0))
        {
            EndHover();
        }
    }

    public void HandleClick()
    {
        if (!draggingAllowed) return;

        if (_pointerOnObject)
        {
            if (resetDragCoroutine != null)
            {
                StopCoroutine(resetDragCoroutine);
                resetDragCoroutine = null;
            }
            if (resetHoverCoroutine != null)
            {
                StopCoroutine(resetHoverCoroutine);
                resetHoverCoroutine = null;
            }

            hoverAllowed = false;

            if (dragTweenId != -1) LeanTween.cancel(dragTweenId);
            if (hoverTweenId != -1) LeanTween.cancel(hoverTweenId);
            _draggingOn = true;
            hoverAllowed = false;
            OnDragStart?.Invoke(this);
        }
    }

    public void HandleClickEnd()
    {
        EndDrag();
    }

    private void StartHover()
    {
        if (resetHoverCoroutine != null)
        {
            StopCoroutine(resetHoverCoroutine);
            resetHoverCoroutine = null;
        }

        if (hoverTweenId != -1) LeanTween.cancel(hoverTweenId);
        hoverTweenId = LeanTween.move(selfRect, returnPosition + hoverOffsetVector, hoverAnimTime).setEaseInOutQuart().id;
        resetHoverCoroutine = StartCoroutine(ResetHoverTween());
        OnHoverStart?.Invoke(this);
    }

    private void EndHover()
    {
        if (resetHoverCoroutine != null)
        {
            StopCoroutine(resetHoverCoroutine);
            resetHoverCoroutine = null;
        }

        if (hoverTweenId != -1) LeanTween.cancel(hoverTweenId);
        hoverTweenId = LeanTween.move(selfRect, returnPosition, hoverAnimTime).setEaseInOutQuart().id;
        resetHoverCoroutine = StartCoroutine(ResetHoverTween());
    }

    private void EndDrag()
    {
        if (_draggingOn)
        {
            if (hoverTweenId != -1)
            {
                LeanTween.cancel(hoverTweenId);
                hoverTweenId = -1;
            }
            dragTweenId = LeanTween.move(selfRect, returnPosition + hoverOffsetVector, returnTime).setEaseOutQuart().id;
            resetDragCoroutine = StartCoroutine(ResetDragTween());
            OnDragEnd?.Invoke(this);
        }
        _draggingOn = false;
    }

    private IEnumerator ResetHoverTween()
    {
        yield return new WaitForSeconds(hoverAnimTime);
        hoverTweenId = -1;
        resetHoverCoroutine = null;
        OnHoverEnd?.Invoke(this);
    }

    private IEnumerator ResetDragTween()
    {
        yield return new WaitForSeconds(returnTime);
        hoverAllowed = true;
        dragTweenId = -1;
        resetDragCoroutine = null;
        OnReturn?.Invoke(this);
        if (!_pointerOnObject) EndHover();
    }

    private void OnDisable()
    {
        _pointerOnObject = false;
        transform.position = returnPosition;
        StopAllCoroutines();
        if (dragTweenId != -1) LeanTween.cancel(dragTweenId);
        if (hoverTweenId != -1) LeanTween.cancel(hoverTweenId);
    }

    public void DisallowMovement()
    {
        draggingAllowed = false;
        hoverAllowed = false;
        StopAllCoroutines();
        if (dragTweenId != -1) LeanTween.cancel(dragTweenId);
        if (hoverTweenId != -1) LeanTween.cancel(hoverTweenId);
    }

    public void AllowMovement()
    {
        draggingAllowed = true;
        hoverAllowed = true;
    }

    public void SetReturnPosition()
    {
        returnPosition = selfRect.anchoredPosition;
    }

    private void Start()
    {
        selfRect = GetComponent<RectTransform>();
        inputReader = UIController.Instance.MainInputReader;
        inputReader.OnClickEvent += HandleClick;
        inputReader.OnClickReleaseEvent += HandleClickEnd;
    }

    private void OnDestroy()
    {
        inputReader.OnClickEvent -= HandleClick;
        inputReader.OnClickReleaseEvent -= HandleClickEnd;
    }

    // Update is called once per frame
    void Update()
    {
        if (!draggingAllowed && _draggingOn) EndDrag();

        if (_draggingOn)
        {
            transform.position = Vector2.Lerp(transform.position, mousePos, moveSpeed * Time.unscaledDeltaTime);
        }
    }
}
