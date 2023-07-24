using System;
using UnityEngine;

// This script is a ScriptableObject that acts as a channel for button synchronization events
public class ButtonSyncChannel : ScriptableObject
{
    // This event is invoked when a button synchronization event is triggered
    public Action<int> OnEventTriggered;

    // This method is called to trigger a button synchronization event, passing in an event ID
    // It invokes the OnEventTriggered event, calling all methods that have subscribed to it
    public void TriggerEvent(int eventID) => OnEventTriggered?.Invoke(eventID);
}
