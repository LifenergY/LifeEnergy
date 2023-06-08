using System;
using UnityEngine;

//REMARK: This asset should only exist once in the project!
// REMARK: Uncomment the line below, if you need to create it.
[CreateAssetMenu(fileName = "FadeEffectChannel", menuName = "Scriptable Fade Effect Channel")]
public class FadeEffectChannel : ScriptableObject
{
    public Action OnFadeIn;
    public void FadeIn() => OnFadeIn?.Invoke();

    public Action OnFadeOut;
    public void FadeOut() => OnFadeOut?.Invoke();
}
