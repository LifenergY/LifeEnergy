using UnityEngine;

public class ExperienceModeSelect : MonoBehaviour
{
    [Header("Channels")]
    [SerializeField] private ExperienceModeChannel experienceModeChannel;

    public void PatientMode() => experienceModeChannel.SetExperienceMode(ExperienceMode.Patient);

    public void PsychologistMode() => experienceModeChannel.SetExperienceMode(ExperienceMode.Psychologist);
}
