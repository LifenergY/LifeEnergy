using Fusion;
using TMPro;
using UnityEngine;

public class NameSync : NetworkBehaviour, IPlayerJoined
{
    [Header("Channels")]
    [SerializeField] private ExperienceModeChannel experienceModeChannel;

    [Header("Texts")]
    [SerializeField] private TMP_Text modeText;
    [SerializeField] private TMP_Text nameText;

    [Networked(OnChanged = nameof(OnNameChanged))]
    public string NetworkedName { get; set; }

    [Networked(OnChanged = nameof(OnModeChanged))]
    public string NetworkedMode { get; set; }

    private void Awake() => ExperienceModeManager.OnExperienceModeSelected += PlayerReady;

    private void OnDestroy() => ExperienceModeManager.OnExperienceModeSelected -= PlayerReady;

    private void PlayerReady(ExperienceMode mode) => UpdateNameAndMode();

    public void PlayerJoined(PlayerRef player) => UpdateNameAndMode();

    private void UpdateNameAndMode()
    {
        if (!Object.HasStateAuthority) return;

            NetworkedMode = experienceModeChannel.GetSelectedExperienceMode().ToString();
            NetworkedName = PlayerPrefsManager.GetPlayerName();
    }

    private static void OnNameChanged(Changed<NameSync> changed)
    {
        changed.Behaviour.nameText.text = changed.Behaviour.NetworkedName;

        if (changed.Behaviour.NetworkedMode == "Psychologist")
            FindAnyObjectByType<DatabaseManager>().SetPsychologistName(changed.Behaviour.NetworkedName);
    }

    private static void OnModeChanged(Changed<NameSync> changed) => changed.Behaviour.modeText.text = changed.Behaviour.NetworkedMode;
}
