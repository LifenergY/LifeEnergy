using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "PlayerVisualsChannel", menuName = "Scriptable Object/Avatar Channel")]
public class PlayerVisualsChannel : ScriptableObject
{
    public Action<PlayerInfo> OnRequestInfo;

    public void  SetPlayerInfo(PlayerInfo playerInfo) => OnRequestInfo?.Invoke(playerInfo);
}
