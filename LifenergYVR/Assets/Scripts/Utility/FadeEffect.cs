using UnityEngine;
using DG.Tweening;

public class FadeEffect : MonoBehaviour
{
    [SerializeField] private FadeEffectChannel fadeEffectChannel;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private Ease fadeInEase = Ease.InExpo;
    [SerializeField] private Ease fadeOutEase = Ease.InOutExpo;

    [SerializeField] private float fadeInTime = 1.75f;
    [SerializeField] private float fadeOutTime = 1.25f;

    private Material fadeMaterial;

    private void Awake()
    {
        fadeMaterial = meshRenderer.materials[0];

        fadeEffectChannel.OnFadeIn += FadeIn;
        fadeEffectChannel.OnFadeOut += FadeOut;

        fadeMaterial.DOFade(1, 0);
    }
    
    private void OnDisable()
    {
        fadeEffectChannel.OnFadeIn -= FadeIn;
        fadeEffectChannel.OnFadeOut -= FadeOut;
    }
    
    private void FadeIn() => fadeMaterial.DOFade(1, fadeInTime).SetEase(fadeInEase);

    private void FadeOut() => fadeMaterial.DOFade(0, fadeOutTime).SetEase(fadeOutEase);
}
