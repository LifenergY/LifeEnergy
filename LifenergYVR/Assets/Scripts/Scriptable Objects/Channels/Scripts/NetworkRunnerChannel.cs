using Fusion;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NetworkRunnerChannel", menuName = "Scriptable Fl")]
public class NetworkRunnerChannel : ScriptableObject
{
    public Func<NetworkRunner> OnNetworkRunnerRequest;
    public Func<NetworkEvents> OnNetworkEventsRequest;

    public NetworkRunner RequestNetworkRunner() => OnNetworkRunnerRequest?.Invoke();
    public NetworkEvents RequestNetworkEvents() => OnNetworkEventsRequest?.Invoke();
}
