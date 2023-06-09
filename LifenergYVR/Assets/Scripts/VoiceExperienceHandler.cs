using System;
using Meta.WitAi;
using UnityEngine;
using Oculus.Voice;
using Meta.WitAi.Json;
using Fusion;

public class VoiceExperienceHandler : MonoBehaviour
{
    [Header("Default States"), Multiline]
    [SerializeField] private string freshStateText = "Try pressing the Activate button and saying \"Make the cube red\"";

    [Header("UI")]
    [SerializeField] private string voiceOutput;
    [SerializeField] private bool showJson;

    [Header("Voice")]
    [SerializeField] private AppVoiceExperience appVoiceExperience;

    public static Action<string> OnCheckVoiceOutput;
    public static Action OnVoiceOutputError;

    private void OnEnable()
    {
        appVoiceExperience.VoiceEvents.OnRequestCreated.AddListener(OnRequestStarted);
        appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener(OnRequestTranscript);
        appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(OnRequestTranscript);
        appVoiceExperience.VoiceEvents.OnStartListening.AddListener(OnListenStart);
        appVoiceExperience.VoiceEvents.OnStoppedListening.AddListener(OnListenStop);
        appVoiceExperience.VoiceEvents.OnStoppedListeningDueToDeactivation.AddListener(OnListenForcedStop);
        appVoiceExperience.VoiceEvents.OnStoppedListeningDueToInactivity.AddListener(OnListenForcedStop);
        appVoiceExperience.VoiceEvents.OnResponse.AddListener(OnRequestResponse);
        appVoiceExperience.VoiceEvents.OnError.AddListener(OnRequestError);

        AnswersManager.OnStartVoiceExperience += StartVoiceExperience;
    }

    private void OnDisable()
    {
        appVoiceExperience.VoiceEvents.OnRequestCreated.RemoveListener(OnRequestStarted);
        appVoiceExperience.VoiceEvents.OnPartialTranscription.RemoveListener(OnRequestTranscript);
        appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(OnRequestTranscript);
        appVoiceExperience.VoiceEvents.OnStartListening.RemoveListener(OnListenStart);
        appVoiceExperience.VoiceEvents.OnStoppedListening.RemoveListener(OnListenStop);
        appVoiceExperience.VoiceEvents.OnStoppedListeningDueToDeactivation.RemoveListener(OnListenForcedStop);
        appVoiceExperience.VoiceEvents.OnStoppedListeningDueToInactivity.RemoveListener(OnListenForcedStop);
        appVoiceExperience.VoiceEvents.OnResponse.RemoveListener(OnRequestResponse);
        appVoiceExperience.VoiceEvents.OnError.RemoveListener(OnRequestError);

        AnswersManager.OnStartVoiceExperience -= StartVoiceExperience;
    }

    private void OnRequestStarted(WitRequest r)
    {
        if (showJson) r.onRawResponse = (response) => voiceOutput = response;
    }

    private void OnRequestTranscript(string transcript) => voiceOutput = transcript;

    private void OnListenStart() => voiceOutput = "Listening...";

    private void OnListenStop() => voiceOutput = "Processing...";

    private void OnListenForcedStop()
    {
        if (!showJson) voiceOutput = freshStateText;

        OnRequestComplete();
    }

    private void OnRequestResponse(WitResponseNode response)
    {
        if (!showJson)
        {
            if (!string.IsNullOrEmpty(response["text"]))
            {
                print("Voice Yes");
                voiceOutput = "Jogador: " + response["text"];

                RPC_VoiceOutPut(true, voiceOutput);
            }
            else
            {
                print("Voice No");
                // voiceOutput = freshStateText;
                RPC_VoiceOutPut(false, "");
            }
        }

        OnRequestComplete();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = true)]
    private void RPC_VoiceOutPut(bool successful, string output)
    {

        if (successful)
        
        {
            print("RPC-------------------True");
            OnCheckVoiceOutput?.Invoke(output);
        }
        else
        {
            print("RPC-------------------fa=lse");
            OnVoiceOutputError?.Invoke();
        }
    }

    private void OnRequestError(string error, string message)
    {
        if (!showJson) voiceOutput = "Error";

        OnRequestComplete();
    }

    private void OnRequestComplete() => StopVoiceExperience();

    public void StartVoiceExperience() => appVoiceExperience.Activate();

    private void StopVoiceExperience() => appVoiceExperience.Deactivate();
}
