using Fusion;
using TMPro;
using UnityEngine;

public class NameSync : NetworkBehaviour, IPlayerJoined
{
    [Header("Channels")]
    [SerializeField] private ExperienceModeChannel experienceModeChannel;

    [Header("Texts")]
    [SerializeField] private TMP_Text modeText;
    [SerializeField] private TMP_Text nameText;

    private void Awake() => ExperienceModeManager.OnExperienceModeSelected += PlayerReady;

    private void OnDestroy() => ExperienceModeManager.OnExperienceModeSelected -= PlayerReady;

    private void PlayerReady(ExperienceMode mode) => SendRPC();

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer) return;
        SendRPC();
    }

    private void SendRPC()
    {
        if (Object.HasStateAuthority) RPC_UpdateNameAndMode(PlayerPrefsManager.GetPlayerName(), experienceModeChannel.GetSelectedExperienceMode().ToString());
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = false)]
    private void RPC_UpdateNameAndMode(string name, string mode)
    {
        print("Name UYpdate:" + name + "   Title:   " + mode);
        nameText.text = name;
        modeText.text = mode;
    }
}
