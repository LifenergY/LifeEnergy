using Fusion;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EventSyncManager : NetworkBehaviour
{
    [SerializeField] private EventSyncChannel eventSyncChannel;

    private Dictionary<int, ISyncable> eventDictionary = new();

    private int eventID;

    private void Awake()
    {
        eventSyncChannel.OnEventTriggered += RPC_EventTriggered;
        RegisterEvent();
    }

    private void OnDestroy() => eventSyncChannel.OnEventTriggered -= RPC_EventTriggered;

    private void RegisterEvent()
    {
        foreach (var rootObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            var allComponents = rootObject.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var component in allComponents)
            {
                if (component is not ISyncable) continue;

                IRegistrable registrable = component as IRegistrable;
                registrable?.RegisterEvent(eventID);

                eventDictionary.Add(eventID, component as ISyncable);

                eventID++;
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies, InvokeLocal = false)]
    private void RPC_EventTriggered(int eventID)
    {
        if (eventDictionary.TryGetValue(eventID, out var iSyncable))
            iSyncable.SyncEvent();
        else
            Debug.LogWarning($"No event associated with EventID {eventID}");
    }
}
