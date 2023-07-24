using UnityEngine;
using UnityEngine.UI;

// This script is responsible for synchronizing button events over the network
public class ButtonEventSync : MonoBehaviour, ISyncable, IRegistrable
{
    // This channel allows us to communicate with the EventSync system
    [SerializeField] private EventSyncChannel eventSyncChannel;
    // This channel allows us to get the current ExperienceMode
    [SerializeField] private ExperienceModeChannel experienceModeChannel;

    // A reference to the Button component
    private Button button;
    // A unique identifier for this button
    private int buttonId;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Get the Button component attached to the same GameObject
        button = GetComponent<Button>();

        // If the selected experience mode is "Patient", attach SyncOnClickEvent function to the button's onClick event
        if (experienceModeChannel.GetSelectedExperienceMode() == ExperienceMode.Patient)
            button.onClick.AddListener(SyncOnClickEvent);
    }

    // OnDestroy is called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        // Unsubscribe the SyncOnClickEvent from the onClick event when the button is destroyed
        // This helps in avoiding memory leaks or other unexpected behaviour
        button.onClick.RemoveListener(SyncOnClickEvent);
    }

    // Function to synchronize button click events over the network
    private void SyncOnClickEvent()
    {
        // Trigger the synchronization event with the given buttonId
        eventSyncChannel.TriggerEvent(buttonId);
    }

    // Function to perform the button click event
    public void SyncEvent()
    {
        // Invoke the button's onClick event
        button.onClick?.Invoke();
    }

    // Function to register the button click event with the given id
    public void RegisterEvent(int id)
    {
        // Set the buttonId to the provided id
        buttonId = id;
    }
}
