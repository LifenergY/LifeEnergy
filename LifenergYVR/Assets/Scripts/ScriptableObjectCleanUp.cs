using UnityEngine;

public class ScriptableObjectCleanUp : MonoBehaviour
{
    public ScriptableObject scriptableObjectInstance;

    private void OnApplicationQuit() => Cleanup();

    private void Cleanup()
    {
        if (scriptableObjectInstance is ICleanable cleanable) cleanable.Clean();
    }
}
