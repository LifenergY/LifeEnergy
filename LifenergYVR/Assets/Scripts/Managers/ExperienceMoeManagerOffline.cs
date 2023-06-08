using UnityEngine;
using UnityEngine.UI;

public class ExperienceMoeManagerOffline : MonoBehaviour
{
    [Header("Channels")]
    [SerializeField] private ExperienceModeChannel experienceModeChannel;

    [Header("Texts")]
    [SerializeField] private Text modeText;

    private void OnEnable() => modeText.text = experienceModeChannel.GetSelectedExperienceMode().ToString();
}
