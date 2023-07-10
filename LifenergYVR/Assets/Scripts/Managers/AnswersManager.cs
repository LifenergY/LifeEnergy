using Fusion;
using TMPro;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class AnswersManager : NetworkBehaviour
{
    [SerializeField] private DatabaseManager databaseManager;
    [SerializeField] private GameObject AnswerUI;
    [SerializeField] private GameObject finalUI;
    [SerializeField] private GameObject spawnManagerUI;

    [Header("Texts")]
    [SerializeField] private TMP_Text topText;
    [SerializeField] private TMP_Text subText;

    [Networked(OnChanged = nameof(OnTopTextChanged)), Capacity(200)]
    public string NetworkedTopText { get; set; }

    [Networked(OnChanged = nameof(OnSubTextChanged)), Capacity(200)]
    public string NetworkedSubText { get; set; }

    private static void OnTopTextChanged(Changed<AnswersManager> changed) => changed.Behaviour.topText.text = changed.Behaviour.NetworkedTopText;
    private static void OnSubTextChanged(Changed<AnswersManager> changed) => changed.Behaviour.subText.text = changed.Behaviour.NetworkedSubText;

    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button answerButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button fillJustifyButton;

    [SerializeField] private Button retryButton;
    [SerializeField] private Button displayAnswersButton;
    [SerializeField] private Button concluirButton;
    [SerializeField] private Button justifyButton;
    [SerializeField] private Button hierarchizyButton;

    [Header("Lists")]
    public List<Answer> answersList = new();
    public List<int> dropdownValues = new();

    [Header("Prefabs")]
    [SerializeField] private GameObject answerTextPrefab;
    [SerializeField] private GameObject answerDropDownPrefab;

    [Header("Canvas Transform")]
    [SerializeField] private Transform answerPanel;
    [SerializeField] private Transform dropdownPanel;

    private int currentAnswerIndex;
    [Networked] private NetworkBool Justifying { get; set; }

    private const string hierarquizarText = "Hierarquizar";
    private const string winMegasenaText = "What would you do if you won the lottery?";
    private const string answerRankJustifyText = "Reply, justify and prioritize";
    private const string answerQuestionsText = "answer";
    private const string justifyText = "Justify";
    private const string answerNextQuestionsText = "Answer Next Question";
    private const string pressButtonToAnswerText = "Pressione o botão para responder";
    private const string isWhatYouMeantText = "is what you wanted to say?";
    private const string voiceOutputErrorText = "Não entendi, pode repetir?";
    private const string justifyAnswerText = "Agora, justifique suas respostas";

    public static Action OnStartVoiceExperience;

    private float period = 0.35f;
    private Sequence loadingTextAnimation;

    private void LoadingTextAnimation()
    {
        if (!Object.HasStateAuthority) return;

        loadingTextAnimation = DOTween.Sequence();
        loadingTextAnimation.AppendCallback(() => NetworkedSubText = "Listening.");
        loadingTextAnimation.AppendInterval(period);
        loadingTextAnimation.AppendCallback(() => NetworkedSubText = " Listening..");
        loadingTextAnimation.AppendInterval(period);
        loadingTextAnimation.AppendCallback(() => NetworkedSubText = "  Listening...");
        loadingTextAnimation.AppendInterval(period);
        loadingTextAnimation.SetLoops(-1, LoopType.Restart);
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            NetworkedTopText = winMegasenaText;
            NetworkedSubText = answerRankJustifyText;
        }

        startButton.onClick.AddListener(SettingAnswers);
        answerButton.onClick.AddListener(StartVoiceExperience);
        displayAnswersButton.onClick.AddListener(DisplayAnswers);
        retryButton.onClick.AddListener(RetryAnswer);
        hierarchizyButton.onClick.AddListener(SetupHierarchyFunctions);
        concluirButton.onClick.AddListener(SendData);

        confirmButton.onClick.AddListener(SaveAnswer);
        fillJustifyButton.onClick.AddListener(FillJustifyText);

        VoiceExperienceHandler.OnCheckVoiceOutput += CheckVoiceOutput;
        VoiceExperienceHandler.OnVoiceOutputError += VoiceOutputError;
    }

    //public override void Despawned(NetworkRunner runner, bool hasState)
    //{
    //    base.Despawned(runner, hasState);
    //}

    private void SendData()
    {
        print("DataBase");
        if (Object.HasStateAuthority) databaseManager.CreateUser();
        //   DOVirtual.DelayedCall(1, ActivateLastUI);
        AnswerUI.GetComponent<AnswersPanelAnimations>().DisableUIAnimation();
    }

    // private void ActivateLastUI() => finalUI.SetActive(true);

    private void SettingAnswers()
    {
        print("Setting Answers");

        if (Object.HasStateAuthority) NetworkedTopText = answersList.Count == 0 ? answerQuestionsText : answerNextQuestionsText;
        if (Object.HasStateAuthority) NetworkedSubText = pressButtonToAnswerText;

        startButton.gameObject.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        fillJustifyButton.gameObject.SetActive(false);
        answerButton.gameObject.SetActive(true);
    }

    private void StartVoiceExperience()
    {
        print("StartVoiceExperience");
        if (Object.HasStateAuthority) NetworkedTopText = "";
        if (Object.HasStateAuthority) NetworkedSubText = "";

        LoadingTextAnimation();

        answerButton.gameObject.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        if (Object.HasStateAuthority) OnStartVoiceExperience?.Invoke();
    }

    private void CheckVoiceOutput(string voiceOutput)
    {
        print("CheckVoiceOutput");
        loadingTextAnimation.Kill();

        NetworkedTopText = isWhatYouMeantText;
        NetworkedSubText = voiceOutput;

        startButton.gameObject.SetActive(false);
        answerButton.gameObject.SetActive(false);
        fillJustifyButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(true);

        print("Justifying: " + Justifying);

        if (Justifying)
        {
            fillJustifyButton.gameObject.SetActive(true);
            // displayAnswersButton.gameObject.SetActive(true);
        }
        else
        {
            confirmButton.gameObject.SetActive(true);
            if (answersList.Count < 0) retryButton.gameObject.SetActive(false);
        }
    }

    private void RetryAnswer()
    {
        print("RetryAnswer");
        if (Object.HasStateAuthority) NetworkedTopText = "";
        if (Object.HasStateAuthority) NetworkedSubText = "";

        confirmButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        LoadingTextAnimation();
        OnStartVoiceExperience?.Invoke();
    }

    private void VoiceOutputError()
    {
        print("VoiceOutputError");
        loadingTextAnimation.Kill();

        if (Object.HasStateAuthority) NetworkedTopText = voiceOutputErrorText;
        if (Object.HasStateAuthority) NetworkedSubText = "";

        if (Justifying) displayAnswersButton.gameObject.SetActive(true);
        else startButton.gameObject.SetActive(true);
    }

    void JustifyEntry()
    {
        print("JustifyEntry");

        if (Object.HasStateAuthority)
        {
            NetworkedTopText = justifyAnswerText;
            NetworkedSubText = "";
        }

        Justifying = true;

        //  uiSpawnManager.SetActive(false);
        displayAnswersButton.gameObject.SetActive(true);
    }

    public void DisplayAnswers()
    {
        print("DisplayAnswers");
        displayAnswersButton.gameObject.SetActive(false);
        fillJustifyButton.gameObject.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        if (Object.HasStateAuthority) NetworkedSubText = "";

        if (currentAnswerIndex < answersList.Count)
        {
            Answer currentAnswer = answersList[currentAnswerIndex];
            NetworkedTopText = currentAnswer.answerText;
            print("                            " + currentAnswer.answerText);
            answerButton.gameObject.SetActive(true);
        }
        else
        {
            NetworkedTopText = "Muito bem! Você conseguiu!";
            hierarchizyButton.gameObject.SetActive(true);
        }
    }

    public void SaveAnswer()
    {
        print("SaveAnswer  AnswerListCount: " + answersList.Count);
        retryButton.gameObject.SetActive(false);
        if (Object.HasStateAuthority) NetworkedTopText = "";

        if (answersList.Count != 3)
        {
            Answer newAnswer = ScriptableObject.CreateInstance<Answer>();
            newAnswer.answerText = subText.text;
            answersList.Add(newAnswer);
            SettingAnswers();
        }

        if (answersList.Count != 3) return;

        answerButton.gameObject.SetActive(false);

        if (Object.HasStateAuthority) NetworkedSubText = "";

        currentAnswerIndex = 0;

        JustifyEntry();
    }

    public void FillJustifyText()
    {
        print("FillJustifyText");
        //  if (Object.HasStateAuthority) NetworkedTopText = "";

        // obtém a resposta correspondente ao índice atual
        Answer currentAnswer = answersList[currentAnswerIndex];

        // atualiza a propriedade JustifyText com o valor do campo de entrada de texto
        currentAnswer.justifyText = subText.text;

        // incrementa o índice para exibir a próxima resposta na próxima interação
        currentAnswerIndex++;
        print("CurrentAnswerIndex : ------" + currentAnswerIndex);
        // exibe o answerText da próxima resposta (ou encerra o jogo)
        DisplayAnswers();
    }

    void EndAnswerSystem()
    {
        print("end answers system");
        //ClearListeners();

        //globalAnswerPanel.SetActive(false);
        //playerRays.SetActive(false);
        //playerCanvas.SetActive(true);
        //mapTrails.SetActive(true);
        //questManager.NextQuest();
    }

    void SetupHierarchyFunctions()
    {
        print("SetupHierarchyFunctions");

        confirmButton.gameObject.SetActive(false);
        fillJustifyButton.gameObject.SetActive(false);
        hierarchizyButton.gameObject.SetActive(false);
        displayAnswersButton.gameObject.SetActive(false);
        concluirButton.gameObject.SetActive(true);

        if (Object.HasStateAuthority) NetworkedTopText = "";
        if (Object.HasStateAuthority) NetworkedSubText = "";

        int index = 150;

        foreach (Answer obj in answersList)
        {
            var instance = Instantiate(answerTextPrefab, answerPanel);
            instance.transform.localPosition = new Vector3(0, -1 * index, 0);
            instance.GetComponent<TMP_Text>().text = obj.answerText;

            //dropdowns

            GameObject secondInstance = Instantiate(answerDropDownPrefab, dropdownPanel);
            secondInstance.transform.localPosition = new Vector3(0, -1 * index, 0);
            secondInstance.GetComponent<TMP_Dropdown>().ClearOptions();
            secondInstance.GetComponent<TMP_Dropdown>().AddOptions(new List<string>()
            {
                "1 - Menos importante", "2 - Intermediário", "3 - Mais importante"
            });

            CheckDropdowns();

            secondInstance.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate
            {
                CheckDropdowns();
            });

            // Obtém a referência ao ScriptableObject correspondente à resposta atual
            Answer answer = obj;

            // Atualiza a propriedade priority do ScriptableObject com o texto da opção selecionada no dropdown
            secondInstance.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate
            {
                answer.priorityText = secondInstance.GetComponent<TMP_Dropdown>().
                    options[secondInstance.GetComponent<TMP_Dropdown>().value].text;
                Debug.Log(answer.priorityText);
            });

            index++;
        }

        //    mainButton.GetComponentInChildren<TMP_Text>().text = "Hierarquizar";
        //mainButton.onClick.AddListener(JustifyEntry);
    }

    void CheckDropdowns()
    {
        dropdownValues.Clear(); // Limpa a lista de valores de dropdown existentes
        foreach (TMP_Dropdown dropdown in transform.GetComponentsInChildren<TMP_Dropdown>())
        {
            dropdownValues.Add(dropdown.value); // Adiciona o valor selecionado na lista
        }
        // Aqui você pode fazer o que quiser com os valores dos dropdowns
        // Por exemplo, pode imprimir os valores no console:
        //       Debug.Log(string.Join(", ", dropdownValues));
    }
}
