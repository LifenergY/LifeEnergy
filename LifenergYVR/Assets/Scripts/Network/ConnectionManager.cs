using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is responsible for managing network connections in the game
public class ConnectionManager : MonoBehaviour
{
    // These variables hold channels, objects, parameters and references needed for network operations
    [Header("Channels")]
    // NetworkRunnerChannel allows us to communicate with the Fusion NetworkRunner
    [SerializeField] private NetworkRunnerChannel networkRunnerChannel;

    [Header("References")]
    // NetworkRunner is a primary entry point into Fusion. It manages scenes, connections, and game sessions
    [SerializeField] private NetworkRunner networkRunner;
    // NetworkEvents allows us to subscribe and listen to network events like server connection
    [SerializeField] private NetworkEvents networkEvents;
    // NetworkSceneManagerDefault handles the synchronization of scene changes over the network
    [SerializeField] private NetworkSceneManagerDefault networkSceneManagerDefault;

    [Header("Parameters")]
    // Name of the room to connect to
    [SerializeField] private string roomName;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Connect to room upon initialization
        ConnectToRoom();

        // Listen to the event of successful connection to the server
        // and execute RegisterNetworkObjects function when this event occurs
        networkEvents.OnConnectedToServer.AddListener(RegisterNetworkObjects);

        // Subscribe to the NetworkRunner and NetworkEvents requests
        networkRunnerChannel.OnNetworkRunnerRequest += OnNetworkRunnerRequest;
        networkRunnerChannel.OnNetworkEventsRequest += OnNetworkEventsRequest;
    }

    // OnDestroy is called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        // Unsubscribe from the event when the object is destroyed
        // This helps in avoiding memory leaks or other unexpected behaviour
        networkEvents.OnConnectedToServer.RemoveListener(RegisterNetworkObjects);

        // Unsubscribe from the NetworkRunner and NetworkEvents requests
        networkRunnerChannel.OnNetworkRunnerRequest -= OnNetworkRunnerRequest;
        networkRunnerChannel.OnNetworkEventsRequest -= OnNetworkEventsRequest;
    }

    // This function initiates a connection to a room using NetworkRunner
    private void ConnectToRoom()
    {
        networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,  // Shared game mode means all clients can interact with each other
            CustomLobbyName = roomName,  // Set room name
            SceneManager = networkSceneManagerDefault,  // Set network scene manager
        });
    }

    // This function registers the network objects for the given scene
    private void RegisterNetworkObjects(NetworkRunner runner)
    {
        // FindNetworkObjects finds all NetworkObjects in the given scene
        var networkObjects = networkSceneManagerDefault.FindNetworkObjects(SceneManager.GetActiveScene());
        // Register these network objects with the network runner
        networkRunner.RegisterSceneObjects(networkObjects);
    }

    // Function called when a request for NetworkRunner is received
    private NetworkRunner OnNetworkRunnerRequest()
    {
        return networkRunner;
    }

    // Function called when a request for NetworkEvents is received
    private NetworkEvents OnNetworkEventsRequest()
    {
        return networkEvents;
    }
}
