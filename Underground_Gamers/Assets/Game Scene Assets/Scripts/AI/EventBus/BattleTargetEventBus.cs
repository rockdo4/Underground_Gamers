using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class BattleTargetEventBus : MonoBehaviour
{
    private static readonly
    IDictionary<CharacterStatus, UnityEvent>
        events = new Dictionary<CharacterStatus, UnityEvent>();

    public static void Subscribe(CharacterStatus aiController, UnityAction action)
    {
        UnityEvent thisEvent;

        if(events.TryGetValue(aiController, out thisEvent))
        {
            thisEvent.AddListener(action);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(action);
            events.Add(aiController, thisEvent);
        }
    }    
    
    public static void Unsubscribe(CharacterStatus aiController, UnityAction action)
    {
        UnityEvent thisEvent;

        if (events.TryGetValue(aiController, out thisEvent))
        {
            thisEvent.RemoveListener(action);
        }
    }

    public static void Publish(CharacterStatus type)
    {

        UnityEvent thisEvent;

        if (events.TryGetValue(type, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
