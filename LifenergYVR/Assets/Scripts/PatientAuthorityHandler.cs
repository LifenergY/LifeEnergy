using Fusion;
using UnityEngine;
using System.Collections.Generic;

public class PatientAuthorityHandler : MonoBehaviour
{
    //[SerializeField] private List<NetworkObject> networkObjects;

    //private void Awake() => ExperienceModeManager.OnExperienceModeSelected += ModeSelected;

    //private void OnDestroy() => ExperienceModeManager.OnExperienceModeSelected -= ModeSelected;

    //private void ModeSelected(ExperienceMode mode)
    //{
    //    if (mode == ExperienceMode.Patient) RequestAuthority();
    //}

    //private void RequestAuthority()
    //{
    //    foreach (var networkObject in networkObjects)
    //    {
    //        networkObject.RequestStateAuthority();
    //        RequestAuthorityInChildren(networkObject.gameObject);
    //    }
    //}

    //private void RequestAuthorityInChildren(GameObject gameObject)
    //{
    //    foreach (Transform child in gameObject.transform)
    //    {
    //        var networkObject = child.GetComponent<NetworkObject>();
    //        if (networkObject != null)
    //        {
    //            networkObject.RequestStateAuthority();
    //        }
    //        RequestAuthorityInChildren(child.gameObject);
    //    }
    //}
}
