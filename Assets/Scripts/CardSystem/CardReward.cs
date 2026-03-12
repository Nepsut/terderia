using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
public class CardReward : CardVisualsOnly
{
    [field: SerializeField] public Button SelfButton { get; private set; }
    [SerializeField] private TMP_Text SelectedText;
    public RectTransform SelfRect { get; private set; }
    private CanvasGroup selfGroup;
    private float selectAnimDuration = 0.32f;
    private readonly Color32 SelectedColor = new(0x87, 0x84, 0x81, 0xff); 
    private Color32 NotSelectedColor => Color.white;
    private readonly Color32 SelectedTextColor = new(0x1f, 0x1f, 0x1f, 0xff);
    private readonly Color32 NotSelectedTextColor = new(0x1f, 0x1f, 0x1f, 0x00);
    public bool IsSelected { get; private set; } = false;
    private bool isFadingOut = false;
    private bool isAnimatingSelection = false;
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
        if (isAnimatingSelection) return;
        isAnimatingSelection = true;
        IsSelected = !IsSelected;
        if (IsSelected)
        {
            OnRewardSelected?.Invoke();
            LeanTween.value(gameObject, SetButtonColor, NotSelectedColor, SelectedColor, selectAnimDuration)
                .setEaseInQuart()
                .setOnComplete(() =>
                {
                    isAnimatingSelection = false;
                });
            LeanTween.value(gameObject, SetTextColor, NotSelectedTextColor, SelectedTextColor, selectAnimDuration)
                .setEaseInQuart();
        }
        else
        {
            OnRewardDeselected?.Invoke();
            LeanTween.value(gameObject, SetButtonColor, SelectedColor, NotSelectedColor, selectAnimDuration)
                .setEaseInQuart()
                .setOnComplete(() =>
                {
                    isAnimatingSelection = false;
                });
            LeanTween.value(gameObject, SetTextColor, SelectedTextColor, NotSelectedTextColor, selectAnimDuration)
                .setEaseInQuart();
        }
    }

    public void SetAsSelected()
    {
        IsSelected = true;
    }

    public void ResetAlpha()
    {
        if (selfGroup == null) selfGroup = GetComponent<CanvasGroup>();
        selfGroup.alpha = 1f;
    }

    public void SetAsDeselected()
    {
        IsSelected = false;
        SetButtonColor(NotSelectedColor);
        SetTextColor(NotSelectedTextColor);
    }

    private void SetButtonColor(Color color)
    {
        var c = SelfButton.colors;
        c.normalColor = color;
        c.highlightedColor = color;
        c.pressedColor = color;
        c.selectedColor = color;
        c.disabledColor = color;
        SelfButton.colors = c;
    }

    private void SetTextColor(Color color)
    {
        SelectedText.color = color;
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
