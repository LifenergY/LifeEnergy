using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviour
{
    [Header("Channels")]
    [SerializeField] private NetworkRunnerChannel networkRunnerChannel;

    [Header("References")]
    [SerializeField] private NetworkRunner networkRunner;
    [SerializeField] private NetworkEvents networkEvents;
    [SerializeField] private NetworkSceneManagerDefault networkSceneManagerDefault;

    [Header("Parameters")]
    [SerializeField] private string roomName;

    private void Awake()
    {
        ConnectToRoom();

        networkEvents.OnConnectedToServer.AddListener(RegisterNetworkObjects);

        networkRunnerChannel.OnNetworkRunnerRequest += OnNetworkRunnerRequest;
        networkRunnerChannel.OnNetworkEventsRequest += OnNetworkEventsRequest;
    }

    private void OnDestroy()
    {
        networkEvents.OnConnectedToServer.RemoveListener(RegisterNetworkObjects);

        networkRunnerChannel.OnNetworkRunnerRequest -= OnNetworkRunnerRequest;
        networkRunnerChannel.OnNetworkEventsRequest -= OnNetworkEventsRequest;
    }
    private void ConnectToRoom()
    {
        networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            CustomLobbyName = roomName,
            SceneManager = networkSceneManagerDefault,
        });
    }

    private void RegisterNetworkObjects(NetworkRunner runner)
    {
        var networkObjects = networkSceneManagerDefault.FindNetworkObjects(SceneManager.GetActiveScene());
        networkRunner.RegisterSceneObjects(networkObjects);
    }

    private NetworkRunner OnNetworkRunnerRequest()
    {
        return networkRunner;
    }

    private NetworkEvents OnNetworkEventsRequest()
    {
        return networkEvents;
    }
}