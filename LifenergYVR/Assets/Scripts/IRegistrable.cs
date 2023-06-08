using UnityEngine;

public interface IRegistrable
{
    void RegisterEvent(int id);
}

public class RegisterEvents : MonoBehaviour, IRegistrable
{
    public void RegisterEvent(int id) { }
}

