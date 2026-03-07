using System;
using UnityEngine;

[RequireComponent(typeof(DraggableObject))]
public class Card : CardVisualsOnly
{
    private DraggableObject selfDraggable;

    public static event Action<Card> OnCardDragStart;
    public static event Action<Card> OnCardDragEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selfDraggable = GetComponent<DraggableObject>();
        selfDraggable.OnDragStart += _ => OnCardDragStart?.Invoke(this);
        selfDraggable.OnDragEnd += _ => OnCardDragEnd?.Invoke(this);
    }
}