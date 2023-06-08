using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private static  string playerName = "PlayerName";

    public static void SavePlayerName(string name) => PlayerPrefs.SetString(playerName, name);

    public static string GetPlayerName() => PlayerPrefs.GetString(playerName);
}
