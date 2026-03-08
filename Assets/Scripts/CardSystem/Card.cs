using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DraggableObject))]
[RequireComponent(typeof(Image))]
public class Card : CardVisualsOnly
{
    public Image SelfImage { get; private set; }
    private DraggableObject selfDraggable;

    public static event Action<Card> OnCardDragStart;
    public static event Action<Card> OnCardDragEnd;

    private void Awake()
    {
        selfDraggable = GetComponent<DraggableObject>();
        SelfImage = GetComponent<Image>();
    }

    private void Start()
    {
        selfDraggable.OnDragStart += _ => OnCardDragStart?.Invoke(this);
        selfDraggable.OnDragEnd += _ => OnCardDragEnd?.Invoke(this);
    }
}