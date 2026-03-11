using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] private Vector2 bobVector;
    [SerializeField] private float lapTime;
    [SerializeField] private LeanTweenType tweenType;
    [SerializeField] private LeanTweenType loopType;
    private float tweenTime;
    private bool useRectTransform = false;
    private RectTransform selfRect;
    private int tweenId = -1;
    private Vector2 startPos;

    private void Start()
    {
        tweenTime = lapTime / 2f;
        useRectTransform = TryGetComponent(out selfRect);

        if (useRectTransform)
        {
            startPos = selfRect.localPosition;
            tweenId = LeanTween.move(selfRect, selfRect.anchoredPosition + bobVector, tweenTime)
            .setEase(tweenType)
            .setLoopType(loopType).id;
        }
        else
        {
            startPos = transform.position;
            tweenId = LeanTween.move(gameObject, (Vector2)transform.position + bobVector, tweenTime)
            .setEase(tweenType)
            .setLoopType(loopType).id;
        }
    }

    private void OnEnable()
    {
        if (tweenId == -1) return;

        LeanTween.cancel(tweenId);
        if (useRectTransform)
        {
            selfRect.localPosition = startPos;
            tweenId = LeanTween.move(selfRect, selfRect.anchoredPosition + bobVector, tweenTime)
            .setEase(tweenType)
            .setLoopType(loopType).id;
        }
        else
        {
            transform.position = startPos;
            tweenId = LeanTween.move(gameObject, (Vector2)transform.position + bobVector, tweenTime)
            .setEase(tweenType)
            .setLoopType(loopType).id;
        }
    }
}
