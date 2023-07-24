using Fusion;
using UnityEngine;
using System.Collections.Generic;

// This script handles requesting authority for all NetworkObjects when the player is in "Patient" mode
public class PatientAuthorityHandler : MonoBehaviour
{
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Listen for ExperienceModeSelected event
        ExperienceModeManager.OnExperienceModeSelected += ModeSelected;
    }

    // OnDestroy is called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        // Unsubscribe from the ExperienceModeSelected event when the object is destroyed
        // This helps in avoiding memory leaks or other unexpected behaviour
        ExperienceModeManager.OnExperienceModeSelected -= ModeSelected;
    }

    // This function is called whenever the experience mode changes
    private void ModeSelected(ExperienceMode mode)
    {
        // If the selected mode is "Patient", request authority for all NetworkObjects
        if (mode == ExperienceMode.Patient) RequestAuthority();
    }

    // Function to request state authority for all NetworkObjects in the scene
    private void RequestAuthority()
    {
        // Find all objects with a NetworkObject component
        NetworkObject[] allNetworkObjects = GameObject.FindObjectsOfType<NetworkObject>();
        foreach (var networkObject in allNetworkObjects)
        {
            // Request state authority over each network object
            networkObject.RequestStateAuthority();
        }
    }
}
