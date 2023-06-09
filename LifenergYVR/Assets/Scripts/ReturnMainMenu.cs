using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnMainMenu : MonoBehaviour
{
    [SerializeField] private FadeEffectChannel effectChannel;
    [SerializeField] private AnswersPanelAnimations answersPanelAnimations;

    [SerializeField] private Button returnButton;

    private void Awake() => returnButton.onClick.AddListener(ReturnToMainMenu);

    private void OnDestroy() => returnButton.onClick.RemoveListener(ReturnToMainMenu);

    private void ReturnToMainMenu()
    {
        effectChannel.FadeIn();
        answersPanelAnimations.DisableUIAnimation();
        DOVirtual.DelayedCall(1.75f, LoadScene);
    }

    private void LoadScene() => SceneManager.LoadScene(0);
}
