using System;
using UnityEngine;

public class EventSyncChannel : ScriptableObject
{
    public Action<int> OnEventTriggered;

    public void TriggerEvent(int eventID) => OnEventTriggered?.Invoke(eventID);
}
