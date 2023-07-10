using Fusion;
using UnityEngine;
using System.Collections.Generic;

public class PatientAuthorityHandler : MonoBehaviour
{
   // [SerializeField] private List<NetworkObject> networkObjects;

    private void Awake() => ExperienceModeManager.OnExperienceModeSelected += ModeSelected;

    private void OnDestroy() => ExperienceModeManager.OnExperienceModeSelected -= ModeSelected;

    private void ModeSelected(ExperienceMode mode)
    {
        if (mode == ExperienceMode.Patient) RequestAuthority();
    }

    private void RequestAuthority()
    {
        // Find all objects with a NetworkObject component
        NetworkObject[] allNetworkObjects = GameObject.FindObjectsOfType<NetworkObject>();
        foreach (var networkObject in allNetworkObjects)
        {
            // Request authority
            networkObject.RequestStateAuthority();
        }
    }
}