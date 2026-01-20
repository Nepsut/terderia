using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float returnTime = 0.74f;
    [HideInInspector] public int siblingIndex;
    private Vector2 mousePos => UIController.MousePosition;
    private bool _pointerOnObject = false;
    private bool _draggingOn = false;
    private Vector2 returnPosition;
    private int tweenId = -1;
    private Coroutine resetCoroutine;

    public event Action OnDragStart;
    public event Action OnDragEnd;
    public event Action OnCardReturn;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerOnObject = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerOnObject = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_pointerOnObject)
        {
            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
                resetCoroutine = null;
            }
            else returnPosition = transform.position;

            if (tweenId != -1) LeanTween.cancel(tweenId);
            _draggingOn = true;
            OnDragStart?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_draggingOn)
        {
            tweenId = LeanTween.move(gameObject, returnPosition, returnTime).setEaseOutQuart().id;
            resetCoroutine = StartCoroutine(ResetTweenId());
            OnDragEnd?.Invoke();
        }
        _draggingOn = false;
    }

    private IEnumerator ResetTweenId()
    {
        yield return new WaitForSeconds(returnTime);
        OnCardReturn?.Invoke();
        tweenId = -1;
        resetCoroutine = null;
    }

    private void OnDisable()
    {
        _draggingOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_draggingOn)
        {
            transform.position = Vector2.Lerp(transform.position, mousePos, moveSpeed * Time.unscaledDeltaTime);
        }
    }
}
