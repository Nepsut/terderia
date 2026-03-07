using System.Collections.Generic;
using UnityEngine;

public class EventCardTargetManager : MonoSingleton<EventCardTargetManager>
{
    public static Dictionary<string, GameObject> EventCardTargets { get; private set; }

    private void Awake()
    {
        EventCardTargets = new();

        foreach (Transform child in transform)
        {
            EventCardTargets.Add(child.name, child.gameObject);
        }
    }
}