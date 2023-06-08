using UnityEngine;

public enum ExperienceMode { None, Patient, Psychologist }

public class ExperienceModeChannel : ScriptableObject
{
    public ExperienceMode slectedExperienceMode;

    public void SetExperienceMode(ExperienceMode mode) => slectedExperienceMode = mode;

    public ExperienceMode GetSelectedExperienceMode() => slectedExperienceMode;
}
