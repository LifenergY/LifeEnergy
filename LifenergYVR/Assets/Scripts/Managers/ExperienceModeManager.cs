using Fusion;
using System;
using UnityEngine;

// This script manages the ExperienceMode for the player based on their joining order
public class ExperienceModeManager : NetworkBehaviour, IPlayerJoined
{
    [Header("Channels")]
    // This channel allows us to get and set the current ExperienceMode
    [SerializeField] private ExperienceModeChannel experienceModeChannel;

    // Networked property to sync whether the player is in patient mode across the network
    [Networked] NetworkBool IsPlayerPatient { get; set; }

    // Event to notify listeners when the experience mode has been selected
    public static Action<ExperienceMode> OnExperienceModeSelected;

    // This function is called whenever a new player joins
    public void PlayerJoined(PlayerRef player)
    {
        // If the joining player is not the local player, we return immediately as the below logic is only for the local player
        if (player != Runner.LocalPlayer) return;

        // Get the local experience mode
        var localMode = experienceModeChannel.GetSelectedExperienceMode();

        // If this is the first player (PlayerId == 0), set IsPlayerPatient based on whether the localMode is Patient or Psychologist
        if (player.PlayerId == 0)
        {
            if (localMode == ExperienceMode.Patient) IsPlayerPatient = true;
            else if (localMode == ExperienceMode.Psychologist) IsPlayerPatient = false;
        }
        // If this is the second player (PlayerId == 1), ensure they get the opposite role of the first player
        else if (player.PlayerId == 1)
        {
            if (localMode == ExperienceMode.Patient && IsPlayerPatient)
                experienceModeChannel.SetExperienceMode(ExperienceMode.Psychologist);
            else if (localMode == ExperienceMode.Psychologist && !IsPlayerPatient)
                experienceModeChannel.SetExperienceMode(ExperienceMode.Patient);

            // Print the selected mode for debugging purposes
            print(experienceModeChannel.GetSelectedExperienceMode().ToString());
        }

        // Invoke the OnExperienceModeSelected event
        OnExperienceModeSelected?.Invoke(experienceModeChannel.GetSelectedExperienceMode());
    }
}
