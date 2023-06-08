using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Channels")]
    [SerializeField] private FadeEffectChannel fadeEffectChannel;

    [Header("Parameters")]
    [SerializeField] private int sceneIndex;

    private bool isLoading;

    public void LoadScene()
    {
        if (isLoading) return;

        LoadeScene();
    }

    private async void LoadeScene()
    {
        isLoading = true;

        fadeEffectChannel.FadeIn();
        var loading = SceneManager.LoadSceneAsync(sceneIndex);
        loading.allowSceneActivation = false;

        await Task.Delay(1750);

        loading.allowSceneActivation = true;

        isLoading = false;
    }
}
