using UnityEngine;

public interface ISyncable
{
    void SyncEvent();
}

public class SyncableEvent : MonoBehaviour, ISyncable
{
    public void SyncEvent() { }
}
