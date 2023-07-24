using Fusion;
using UnityEngine;

// This script is responsible for spawning the player's avatar (or gameobject) when the player connects to the server
public class NetworkPlayerSpawner : MonoBehaviour
{
    // Reference to the fade effect channel, which is presumably used to control some kind of fade effect when spawning the player
    [SerializeField] private FadeEffectChannel fadeEffectChannel;

    // Reference to the Player prefab, which is the object to be instantiated when the player connects
    [SerializeField] private GameObject PlayerPrefab;

    // Reference to the network events, which will be used to trigger the player spawn when the server is connected
    [SerializeField] private NetworkEvents networkEvents;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Add OnConnectedToServer function as a listener to the 'OnConnectedToServer' event
        networkEvents.OnConnectedToServer.AddListener(OnConnectedToServer);
    }

    // OnDestroy is called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        // Remove OnConnectedToServer function as a listener to the 'OnConnectedToServer' event
        networkEvents.OnConnectedToServer.RemoveListener(OnConnectedToServer);
    }

    // This function is called whenever the server is connected
    private void OnConnectedToServer(NetworkRunner runner)
    {
        // Spawn the player object at the server
        runner.Spawn(PlayerPrefab);

        // Trigger fade out effect
        fadeEffectChannel.FadeOut();
    }
}
