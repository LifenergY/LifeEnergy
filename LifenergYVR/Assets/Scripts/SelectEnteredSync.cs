using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// This script is responsible for synchronizing the 'selectEntered' event of an XRGrabInteractable across the network
public class SelectEnteredSync : MonoBehaviour, ISyncable, IRegistrable
{
    // Reference to the event sync channel, which will be used to trigger the event across the network
    [SerializeField] private EventSyncChannel eventSyncChannel;

    // Reference to the ExperienceModeChannel, which is used to get the current experience mode
    [SerializeField] private ExperienceModeChannel experienceModeChannel;

    // Reference to the XRGrabInteractable component of the object
    private XRGrabInteractable xrGrabInteractable;

    // Id of the button, used when triggering the event
    private int buttonId;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Get the XRGrabInteractable component
        xrGrabInteractable = GetComponent<XRGrabInteractable>();

        // If the current experience mode is 'Patient', add the SyncOnClickEvent function as a listener to the 'selectEntered' event
        if (experienceModeChannel.GetSelectedExperienceMode() == ExperienceMode.Patient)
            xrGrabInteractable.selectEntered.AddListener(SyncOnClickEvent);
    }

    // OnDestroy is called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        // Remove the SyncOnClickEvent function as a listener to the 'selectEntered' event when the object is destroyed
        xrGrabInteractable.selectEntered.RemoveListener(SyncOnClickEvent);
    }

    // This function is called whenever the 'selectEntered' event is triggered
    private void SyncOnClickEvent(SelectEnterEventArgs arg)
    {
        // Trigger the event across the network using the button id
        eventSyncChannel.TriggerEvent(buttonId);
    }

    // This function is called to sync the 'selectEntered' event
    public void SyncEvent()
    {
        // Invoke the 'selectEntered' event
        xrGrabInteractable.selectEntered?.Invoke(null);
    }

    // This function is called to register the button id
    public void RegisterEvent(int id)
    {
        // Set the button id
        buttonId = id;
    }
}
