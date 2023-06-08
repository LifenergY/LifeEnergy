using UnityEngine;

public interface ICleanable
{
    void Clean();
}

public class CleanableScriptableObject : ScriptableObject, ICleanable
{
    public void Clean() { }
}
