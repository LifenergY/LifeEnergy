using UnityEngine;
using DG.Tweening;

public class AnswersPanelAnimations : MonoBehaviour
{
    [SerializeField] private Ease scaleEase;
    [SerializeField] private Ease moveEase;
    [SerializeField] private float speedScaleAnimation = 1f;
    [SerializeField] private float speedMoveAnimation = 1f;
    [SerializeField] private Transform startingPosition;

    private Vector3 originalScale;
    private Vector3 endPosition;

    private void Awake()
    {
        originalScale = transform.localScale;
        endPosition = transform.position;

        transform.localScale = Vector3.zero;
        transform.position = startingPosition.position;
    }

    public void DisableUIAnimation()
    {
        transform.DOScale(Vector3.zero, speedScaleAnimation).SetEase(scaleEase);
        transform.DOMove(startingPosition.position, speedMoveAnimation).SetEase(moveEase);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
        transform.position = startingPosition.position;
    }

    private void OnEnable()
    {
        transform.DOScale(originalScale, speedScaleAnimation).SetEase(scaleEase);
        transform.DOMove(endPosition, speedMoveAnimation).SetEase(moveEase);
    }
}
