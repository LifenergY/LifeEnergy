using Fusion;
using System;
using UnityEngine;

public class ExperienceModeManager : NetworkBehaviour, IPlayerJoined
{
    [Header("Channels")]
    [SerializeField] private ExperienceModeChannel experienceModeChannel;

    [Networked] NetworkBool IsPlayerPatient { get; set; }

    public static Action<ExperienceMode> OnExperienceModeSelected;

    public void PlayerJoined(PlayerRef player)
    {
        if (player != Runner.LocalPlayer) return;

        var localMode = experienceModeChannel.GetSelectedExperienceMode();

        if (player.PlayerId == 0)
        {
            if (localMode == ExperienceMode.Patient) IsPlayerPatient = true;
            else if (localMode == ExperienceMode.Psychologist) IsPlayerPatient = false;
        }
        else if (player.PlayerId == 1)
        {
            if (localMode == ExperienceMode.Patient && IsPlayerPatient) experienceModeChannel.SetExperienceMode(ExperienceMode.Psychologist);
            else if (localMode == ExperienceMode.Psychologist && !IsPlayerPatient) experienceModeChannel.SetExperienceMode(ExperienceMode.Patient);
            print(experienceModeChannel.GetSelectedExperienceMode().ToString());
        }

        OnExperienceModeSelected?.Invoke(experienceModeChannel.GetSelectedExperienceMode());
    }
}
