using Fusion;
using UnityEngine;

public class ColorModeSync : NetworkBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] handSkinMesh;

    private void Awake() => ExperienceModeManager.OnExperienceModeSelected += SetModeColor;

    private void OnDestroy() => ExperienceModeManager.OnExperienceModeSelected -= SetModeColor;

    private void SetModeColor(ExperienceMode mode)
    {
        if (mode == ExperienceMode.Patient) ModeColor = Color.blue;
        else if (mode == ExperienceMode.Psychologist) ModeColor = Color.red;
    }

    [Networked(OnChanged = nameof(OnColorChanged))] private Color ModeColor { get; set; }

    private static void OnColorChanged(Changed<ColorModeSync> changed)
    {
        foreach (var meshRenderer in changed.Behaviour.handSkinMesh)
            meshRenderer.material.color = changed.Behaviour.ModeColor;
    }
}
