using Fusion;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class PsychologistActions : NetworkBehaviour
{
    [Header("Renderers")]
    [SerializeField] private SkinnedMeshRenderer[] renderers;

    [Header("Materials")]
    [SerializeField] private Material handMatOpaque;
    [SerializeField] private Material handMatTransparent;

    [Header("Actions")]
    [SerializeField] private InputActionReference hidePsychologistActionRef;

    [Networked] private NetworkBool isHidden { get; set; }

    private bool isPsychologist;
    private bool isAnimating;

    private void Awake() => ExperienceModeManager.OnExperienceModeSelected += ModeSelected;

    private void OnDestroy() => ExperienceModeManager.OnExperienceModeSelected -= ModeSelected;

    private void ModeSelected(ExperienceMode mode)
    {
        if (mode == ExperienceMode.Psychologist) isPsychologist = true;
    }

    private void Update()
    {
        if (isPsychologist && Object.HasStateAuthority && !isAnimating && hidePsychologistActionRef.action.WasPressedThisFrame()) ToggleHide();
    }

    private void ToggleHide()
    {
        isHidden = !isHidden;
        isAnimating = true;
        DOVirtual.DelayedCall(1, AnimatingStopped);

        foreach (var render in renderers)
        {
            if (isHidden)
            {
                render.material = handMatTransparent;
                render.material.DOFade(0.5f, 1);
            }
            else
            {
                render.material.DOFade(1, 1).OnComplete(() => render.material = handMatOpaque);
            }
        }

        RPC_UpdateNameAndMode();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = false)]
    private void RPC_UpdateNameAndMode()
    {
        foreach (var render in renderers)
        {
            if (isHidden)
            {
                render.material = handMatTransparent;
                render.material.DOFade(0, 1);
            }
            else
            {
                render.material.DOFade(1, 1).OnComplete(() => render.material = handMatOpaque);
            }
        }
    }

    private void AnimatingStopped() => isAnimating = false;
}
