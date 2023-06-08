using UnityEngine;

public class PsychologistHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] interactionObjects;

    private void Awake() => ExperienceModeManager.OnExperienceModeSelected += ModeSelected;

    private void OnDestroy() => ExperienceModeManager.OnExperienceModeSelected -= ModeSelected;

    private void ModeSelected(ExperienceMode mode)
    {
        if (mode == ExperienceMode.Psychologist) foreach (var obj in interactionObjects) Destroy(obj);
    }
}
