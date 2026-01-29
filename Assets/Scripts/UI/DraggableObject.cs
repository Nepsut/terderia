using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float returnTime = 0.74f;
    [SerializeField] private float hoverOffsetHorizontal;
    [SerializeField] private float hoverOffsetVertical;
    [SerializeField] private float hoverAnimTime = 0.5f;
    [HideInInspector] public int siblingIndex;
    public bool DraggingAllowed = true;
    private bool hoverAllowed = true;
    private Vector2 mousePos => UIController.MousePosition;
    private bool _pointerOnObject = false;
    private bool _draggingOn = false;
    public Vector2 returnPosition;
    private int hoverTweenId = -1;
    private int dragTweenId = -1;
    private Coroutine resetHoverCoroutine;
    private Coroutine resetDragCoroutine;

    public event Action OnDragStart;
    public event Action OnDragEnd;
    public event Action OnCardReturn;

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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!DraggingAllowed) return;

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
            _draggingOn = true;
            hoverAllowed = false;
            OnDragStart?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
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
        hoverTweenId = LeanTween.move(gameObject,
                                      new Vector2(returnPosition.x + hoverOffsetHorizontal,
                                                  returnPosition.y + hoverOffsetVertical),
                                      hoverAnimTime).setEaseInOutQuart().id;
        StartCoroutine(ResetHoverTween());
    }

    private void EndHover()
    {
        if (resetHoverCoroutine != null)
        {
            StopCoroutine(resetHoverCoroutine);
            resetHoverCoroutine = null;
        }

        if (hoverTweenId != -1) LeanTween.cancel(hoverTweenId);
        hoverTweenId = LeanTween.move(gameObject, returnPosition, hoverAnimTime).setEaseInOutQuart().id;
        StartCoroutine(ResetHoverTween());
    }

    private void EndDrag()
    {
        if (_draggingOn)
        {
            dragTweenId = LeanTween.move(gameObject, returnPosition, returnTime).setEaseOutQuart().id;
            resetDragCoroutine = StartCoroutine(ResetDragTween());
            OnDragEnd?.Invoke();
        }
        _draggingOn = false;
    }

    private IEnumerator ResetHoverTween()
    {
        yield return new WaitForSeconds(hoverAnimTime);
        hoverTweenId = -1;
        resetHoverCoroutine = null;
    }

    private IEnumerator ResetDragTween()
    {
        yield return new WaitForSeconds(returnTime);
        hoverAllowed = true;
        dragTweenId = -1;
        resetDragCoroutine = null;
        if (_pointerOnObject == true) StartHover();
        OnCardReturn?.Invoke();
    }

    private void OnDisable()
    {
        transform.position = returnPosition;
        EndDrag();
    }

    // Update is called once per frame
    void Update()
    {
        if (!DraggingAllowed && _draggingOn) EndDrag();

        if (_draggingOn)
        {
            transform.position = Vector2.Lerp(transform.position, mousePos, moveSpeed * Time.unscaledDeltaTime);
        }
    }
}
