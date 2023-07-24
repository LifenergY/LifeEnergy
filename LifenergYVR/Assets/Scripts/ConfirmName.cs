using TMPro;
using UnityEngine;

// This script is responsible for confirming and saving player's name
public class ConfirmName : MonoBehaviour
{
    // The UI Text component that displays the player's name
    [SerializeField] private TMP_Text nameText;

    // OnEnable is called whenever the object becomes active
    private void OnEnable()
    {
        // Fetch saved player's name from PlayerPrefsManager
        var savedName = PlayerPrefsManager.GetPlayerName();

        // If there's a saved name, set the text field with it
        if (savedName != null) nameText.text = savedName;
    }

    // Function to save the player's name written in the text field
    public void SaveNameText()
    {
        // If the length of the name is greater than 1, save it
        // This prevents saving empty or single character names
        if (nameText.text.Length > 1) PlayerPrefsManager.SavePlayerName(nameText.text);
    }
}
