using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabInteractionEventSync : MonoBehaviour, ISyncable, IRegistrable
{
    [SerializeField] private EventSyncChannel eventSyncChannel;

    private XRGrabInteractable xRGrabInteractable;
    private int eventID;
    private void Awake()
    {
        xRGrabInteractable = GetComponent<XRGrabInteractable>();

        //if (experienceModeChannel.GetSelectedExperienceMode() == ExperienceMode.Patient)
        //    button.onClick.AddListener(SyncOnClickEvent);
    }

    public void RegisterEvent(int id)=> eventID = id;
    

    public void SyncEvent()
    {
        throw new System.NotImplementedException();
    }
}
