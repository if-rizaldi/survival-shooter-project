using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> { }
public class GameEventListener : MonoBehaviour
{
    public GameEvent[] gameEvents;
    public CustomGameEvent[] responses;

    private void OnEnable()
    {
        foreach (GameEvent gameEvent in gameEvents)
        {
            gameEvent.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        foreach (GameEvent gameEvent in gameEvents)
        {
            gameEvent.UnregisterListener(this);
        }
    }

    public void OnEventRaised(Component sender, object data)
    {
        foreach (CustomGameEvent response in responses)
        {
            response.Invoke(sender, data);
        }
    }
}
