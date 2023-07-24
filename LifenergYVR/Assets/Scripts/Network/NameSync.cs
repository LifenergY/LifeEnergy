using Fusion;
using TMPro;
using UnityEngine;

// This script is responsible for synchronizing the player's name and experience mode across the network
public class NameSync : NetworkBehaviour, IPlayerJoined
{
    [Header("Channels")]
    // This channel allows us to get the current ExperienceMode
    [SerializeField] private ExperienceModeChannel experienceModeChannel;

    [Header("Texts")]
    // Text fields for displaying the mode and name
    [SerializeField] private TMP_Text modeText;
    [SerializeField] private TMP_Text nameText;

    // Networked properties that syncs the name and mode over the network
    // When their value changes, OnNameChanged and OnModeChanged methods will be called respectively
    [Networked(OnChanged = nameof(OnNameChanged))]
    public string NetworkedName { get; set; }

    [Networked(OnChanged = nameof(OnModeChanged))]
    public string NetworkedMode { get; set; }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Listen for ExperienceModeSelected event
        ExperienceModeManager.OnExperienceModeSelected += PlayerReady;
    }

    // OnDestroy is called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        // Unsubscribe from the ExperienceModeSelected event when the object is destroyed
        // This helps in avoiding memory leaks or other unexpected behaviour
        ExperienceModeManager.OnExperienceModeSelected -= PlayerReady;
    }

    // Function to update the player's name and mode when the player is ready
    private void PlayerReady(ExperienceMode mode)
    {
        // Update name and mode whenever a player is ready
        UpdateNameAndMode();
    }

    // This function is called whenever a new player joins
    public void PlayerJoined(PlayerRef player)
    {
        // Update name and mode whenever a new player joins
        UpdateNameAndMode();
    }

    // Function to update the NetworkedMode and NetworkedName values
    private void UpdateNameAndMode()
    {
        // Check if the object is under the authority of the local client
        if (!Object.HasStateAuthority) return;

        // Update networked properties with selected mode and player name
        NetworkedMode = experienceModeChannel.GetSelectedExperienceMode().ToString();
        NetworkedName = PlayerPrefsManager.GetPlayerName();
    }

    // Function that gets called when the NetworkedName changes
    private static void OnNameChanged(Changed<NameSync> changed)
    {
        // Update the name displayed on the UI
        changed.Behaviour.nameText.text = changed.Behaviour.NetworkedName;

        // If the networked mode is "Psychologist", update the Psychologist's name in the database
        if (changed.Behaviour.NetworkedMode == "Psychologist")
            FindAnyObjectByType<DatabaseManager>().SetPsychologistName(changed.Behaviour.NetworkedName);
    }

    // Function that gets called when the NetworkedMode changes
    private static void OnModeChanged(Changed<NameSync> changed)
    {
        // Update the mode displayed on the UI
        changed.Behaviour.modeText.text = changed.Behaviour.NetworkedMode;
    }
}
