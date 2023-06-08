using UnityEngine;

public class FadeEffectOffline : MonoBehaviour
{
    [SerializeField] private FadeEffectChannel fadeEffectChannel;

    private void Start() => fadeEffectChannel.FadeOut();
}
