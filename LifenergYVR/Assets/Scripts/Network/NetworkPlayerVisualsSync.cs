using System.Threading;
using UnityEngine;

public class NetworkPlayerVisualsSync : MonoBehaviour
{
    [Header("Channels")]
    [SerializeField] private PlayerVisualsChannel playerVisualsChannel;

    [Header("References")]
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    [Header("Body Parameters")]
    [SerializeField] private Vector3 bodyPositionOffSet;
    [SerializeField] private float rotationSpeed = 0.04f;

    private Transform headVisuals;
    private Transform leftHandVisuals;
    private Transform rightHandVisuals;
    
    private bool hasVisuals;

    private void Awake() => playerVisualsChannel.OnRequestInfo += OnVisualsSpawned;

    private void OnDestroy() => playerVisualsChannel.OnRequestInfo -= OnVisualsSpawned;

    private void OnVisualsSpawned(PlayerInfo playerinfo)
    {
        headVisuals = playerinfo.Head;
        leftHandVisuals = playerinfo.LeftHand;
        rightHandVisuals = playerinfo.RightHand;

        hasVisuals = true;
    }

    private void LateUpdate()
    {
        if (!hasVisuals) return;

        // BodyPositionAndRotation();

        headVisuals.SetPositionAndRotation(head.position, head.rotation);
        leftHandVisuals.SetPositionAndRotation(leftHand.position, leftHand.rotation);
        rightHandVisuals.SetPositionAndRotation(rightHand.position, rightHand.rotation);
    }

    //private void BodyPositionAndRotation()
    //{
    //    Quaternion slerpRotation = Quaternion.Slerp(bodyAvatar.rotation, head.rotation, rotationSpeed);
    //    slerpRotation = Quaternion.Euler(new Vector3(0f, slerpRotation.eulerAngles.y, 0f));

    //    bodyAvatar.SetPositionAndRotation(head.position + bodyPositionOffSet, slerpRotation);
    //}
}
