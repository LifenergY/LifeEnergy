using TMPro;
using UnityEngine;

public class ConfirmName : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;

    private void OnEnable()
    {
        var savedName = PlayerPrefsManager.GetPlayerName();
        if (savedName != null) nameText.text = savedName;
    }

    public void SaveNameText()
    {
        if (nameText.text.Length > 1) PlayerPrefsManager.SavePlayerName(nameText.text);
    }
}
