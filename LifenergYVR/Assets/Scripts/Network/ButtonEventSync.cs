using UnityEngine;
using UnityEngine.UI;

public class ButtonEventSync : MonoBehaviour, ISyncable, IRegistrable
{
    [SerializeField] private EventSyncChannel eventSyncChannel;
    [SerializeField] private ExperienceModeChannel experienceModeChannel;

    private Button button;
    private int buttonId;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (experienceModeChannel.GetSelectedExperienceMode() == ExperienceMode.Patient)
            button.onClick.AddListener(SyncOnClickEvent);
    }

    private void OnDestroy() => button.onClick.RemoveListener(SyncOnClickEvent);

    private void SyncOnClickEvent() => eventSyncChannel.TriggerEvent(buttonId);

    public void SyncEvent() => button.onClick?.Invoke();

    public void RegisterEvent(int id) => buttonId = id;
}
