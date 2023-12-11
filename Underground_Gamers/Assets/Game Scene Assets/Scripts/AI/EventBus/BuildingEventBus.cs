using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingEventBus : MonoBehaviour
{
    private static readonly
    IDictionary<Transform, UnityEvent>
        events = new Dictionary<Transform, UnityEvent>();

    public static void Subscribe(Transform transform, UnityAction action)
    {
        UnityEvent thisEvent;

        if (events.TryGetValue(transform, out thisEvent))
        {
            thisEvent.AddListener(action);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(action);
            events.Add(transform, thisEvent);
        }
    }

    public static void Unsubscribe(Transform transform, UnityAction action)
    {
        UnityEvent thisEvent;

        if (events.TryGetValue(transform, out thisEvent))
        {
            thisEvent.RemoveListener(action);
        }
    }

    public static void Publish(Transform type)
    {
        UnityEvent thisEvent;

        if (events.TryGetValue(type, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
