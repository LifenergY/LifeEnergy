using Fusion;
using UnityEngine;

public class NetworkPlayerSpawner : MonoBehaviour
{
    [SerializeField] private FadeEffectChannel fadeEffectChannel;

    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private NetworkEvents networkEvents;

    private void Awake() => networkEvents.OnConnectedToServer.AddListener(OnConnectedToServer);
    private void OnDestroy() => networkEvents.OnConnectedToServer.RemoveListener(OnConnectedToServer);

    private void OnConnectedToServer(NetworkRunner runner)
    {
        runner.Spawn(PlayerPrefab);
        fadeEffectChannel.FadeOut();
    }
}