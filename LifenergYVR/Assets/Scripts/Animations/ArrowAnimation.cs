using UnityEngine;
using System.Collections;

public class ArrowAnimation : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private float delayBetweenParticles = 0.35f;

    public void ActivateAnimation() => StartCoroutine(StartAnimation());
    public void DeactivateAnimation() => StartCoroutine(StopAnimation());

    private void Start() => StartCoroutine(StartAnimation());

    private IEnumerator StartAnimation()
    {
        foreach (var particle in particles)
        {
            yield return new WaitForSeconds(delayBetweenParticles);
            particle.Play();
        }
    }

    private IEnumerator StopAnimation()
    {
        foreach (var particle in particles)
        {
            yield return new WaitForSeconds(delayBetweenParticles);
            particle.Stop();
        }
    }
}
