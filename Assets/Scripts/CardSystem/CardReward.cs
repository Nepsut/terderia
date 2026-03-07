using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
public class CardReward : CardVisualsOnly
{
    [field: SerializeField] public Button SelfButton { get; private set; }
    public RectTransform SelfRect { get; private set; }
    private CanvasGroup selfGroup;
    public bool IsSelected { get; private set; } = false;
    private bool isFadingOut = false;
    public event Action OnRewardSelected;
    public event Action OnRewardDeselected;

    private void Start()
    {
        selfGroup = GetComponent<CanvasGroup>();
        SelfRect = GetComponent<RectTransform>();
        SelfButton.onClick.AddListener(SelectCard);
    }

    public void SelectCard()
    {
        IsSelected = !IsSelected;
        if (IsSelected) OnRewardSelected?.Invoke();
        else OnRewardDeselected?.Invoke();
    }

    public void DeselectCard()
    {
        IsSelected = false;
    }

    public void StartFadeOut(float fadeTime)
    {
        if (isFadingOut) return;
        isFadingOut = true;

        LeanTween.value(gameObject, value => selfGroup.alpha = value, 1f, 0f, fadeTime)
            .setOnComplete(() => gameObject.SetActive(false))
            .setEaseOutQuart();
    }
}
