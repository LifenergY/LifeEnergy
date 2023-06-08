using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Answer", menuName = "MeusAssets/Answer")]

[Serializable]
public class Answer : ScriptableObject
{
    public string answerText;
    [FormerlySerializedAs("priority")] public string priorityText;
    public string justifyText;

}