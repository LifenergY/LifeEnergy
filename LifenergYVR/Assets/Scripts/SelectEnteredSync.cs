using Fusion;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectEnteredSync : MonoBehaviour, ISyncable, IRegistrable
{
    [SerializeField] private EventSyncChannel eventSyncChannel;
    [SerializeField] private ExperienceModeChannel experienceModeChannel;

    private XRGrabInteractable xrGrabInteractable;
    private int buttonId;

    private void Awake()
    {
        xrGrabInteractable = GetComponent<XRGrabInteractable>();

        if (experienceModeChannel.GetSelectedExperienceMode() == ExperienceMode.Patient)
          xrGrabInteractable.selectEntered.AddListener(SyncOnClickEvent);
    }

    private void OnDestroy() => xrGrabInteractable.selectEntered.RemoveListener(SyncOnClickEvent);

    private void SyncOnClickEvent(SelectEnterEventArgs arg) => eventSyncChannel.TriggerEvent(buttonId);

    public void SyncEvent() => xrGrabInteractable.selectEntered?.Invoke(null);

    public void RegisterEvent(int id) => buttonId = id;
}