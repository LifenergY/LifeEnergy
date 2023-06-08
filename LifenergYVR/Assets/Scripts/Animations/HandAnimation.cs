using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimation : NetworkBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference triggerActionRef;
    [SerializeField] private InputActionReference gripActionRef;

    [Header("References")]
    [SerializeField] private Animator handAnimator;

    [Networked(OnChanged = nameof(OnTriggerValueChanged))] private float triggerValue { get; set; }
    [Networked(OnChanged = nameof(OnGripValueChanged))] private float gripValue { get; set; }

    private static readonly int s_trigger = Animator.StringToHash("Trigger");
    private static readonly int s_grip = Animator.StringToHash("Grip");

    private static void OnTriggerValueChanged(Changed<HandAnimation> changed) => changed.Behaviour.handAnimator.SetFloat(s_trigger, changed.Behaviour.triggerValue);
    private static void OnGripValueChanged(Changed<HandAnimation> changed) => changed.Behaviour.handAnimator.SetFloat(s_grip, changed.Behaviour.gripValue);

    void Update()
    {
        if (!Object.HasStateAuthority) return;

        triggerValue = triggerActionRef.action.ReadValue<float>();
        gripValue = gripActionRef.action.ReadValue<float>();
    }
}
