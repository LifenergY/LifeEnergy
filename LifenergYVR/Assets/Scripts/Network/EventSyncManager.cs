using Fusion;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// This script is responsible for syncing events across the network
public class EventSyncManager : NetworkBehaviour
{
    // Reference to the event sync channel, presumably a communication channel to sync events
    [SerializeField] private EventSyncChannel eventSyncChannel;

    // This dictionary maps event IDs to ISyncable objects
    private Dictionary<int, ISyncable> eventDictionary = new();

    // This variable keeps track of the current event ID
    private int eventID;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Add RPC_EventTriggered function as a listener to the 'OnEventTriggered' event
        eventSyncChannel.OnEventTriggered += RPC_EventTriggered;

        // Register all the events in the scene
        RegisterEvent();
    }

    // OnDestroy is called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        // Remove RPC_EventTriggered function as a listener to the 'OnEventTriggered' event
        eventSyncChannel.OnEventTriggered -= RPC_EventTriggered;
    }

    // Register all the events in the scene
    private void RegisterEvent()
    {
        // For each root object in the scene
        foreach (var rootObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            // Get all MonoBehaviour components in the root object and its children
            var allComponents = rootObject.GetComponentsInChildren<MonoBehaviour>(true);

            // For each component in allComponents
            foreach (var component in allComponents)
            {
                // If component is not ISyncable, skip this iteration
                if (component is not ISyncable) continue;

                // Try to cast the component to IRegistrable and call its RegisterEvent method
                IRegistrable registrable = component as IRegistrable;
                registrable?.RegisterEvent(eventID);

                // Add the component to the event dictionary
                eventDictionary.Add(eventID, component as ISyncable);

                // Increment the event ID
                eventID++;
            }
        }
    }

    // This function is called remotely by the network, and is used to trigger an event based on its ID
    [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies, InvokeLocal = false)]
    private void RPC_EventTriggered(int eventID)
    {
        // If the event ID exists in the dictionary, trigger the event
        if (eventDictionary.TryGetValue(eventID, out var iSyncable))
            iSyncable.SyncEvent();
        else
            // Otherwise, print a warning message
            Debug.LogWarning($"No event associated with EventID {eventID}");
    }
}
