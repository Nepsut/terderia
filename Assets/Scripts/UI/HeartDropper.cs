using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HeartDropper : MonoBehaviour
{
    private RectTransform selfRect;
    private Vector2 startPos;
    private const float dropAmount = -400f;
    private const float dropTime = 0.8f;
    private int tweenId = -1;

    private void Start()
    {
        selfRect = GetComponent<RectTransform>();
        startPos = selfRect.anchoredPosition;
    }

    public void DropHeart()
    {
        if (tweenId != -1)
        {
            selfRect.anchoredPosition = startPos;
            StartCoroutine(HeartDropHandler());
        }
        else
        {
            StartCoroutine(HeartDropHandler());
        }
    }

    private IEnumerator HeartDropHandler()
    {
        tweenId = LeanTween.moveY(selfRect, selfRect.anchoredPosition.y + dropAmount, dropTime).setEaseInOutQuad().id;
        yield return new WaitForSeconds(dropTime);
        selfRect.anchoredPosition = startPos;
        tweenId = -1;
    }
}