using System;
using UnityEngine;

public class ButtonSyncChannel : ScriptableObject
{
    public Action<int> OnEventTriggered;

    public void TriggerEvent(int eventID) => OnEventTriggered?.Invoke(eventID);
}
