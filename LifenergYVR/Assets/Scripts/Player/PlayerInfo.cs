using Fusion;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    [Header("Channels")]
    [SerializeField] private PlayerVisualsChannel playerVisualsChannel;

    [Header("References")]
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    public Transform Head => head;
    public Transform LeftHand => leftHand;
    public Transform RightHand => rightHand;

    private void Start()
    {
        if (!Object.HasStateAuthority) return;

        playerVisualsChannel.SetPlayerInfo(this);

        leftHand.transform.GetChild(0).transform.SetPositionAndRotation(new Vector3(-0.16f, 0, -0.07f), Quaternion.Euler(new Vector3(-2.75f, -6.55f, 100f)));
        rightHand.transform.GetChild(0).transform.SetPositionAndRotation(new Vector3(0.16f, 0, -0.07f), Quaternion.Euler(new Vector3(-2.75f, 6.55f, -100f)));
    }
}
