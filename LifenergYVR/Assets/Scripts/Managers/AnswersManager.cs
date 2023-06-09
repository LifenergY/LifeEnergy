using Fusion;
using TMPro;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// wrong answer index timing reset
/// </summary>
public class AnswersManager : MonoBehaviour
{
    [SerializeField] private DatabaseManager databaseManager;
    [SerializeField] private GameObject AnswerUI;
    [SerializeField] private GameObject finalUI;

    [Header("Texts")]
    [SerializeField] private TMP_Text topText;
    [SerializeField] private TMP_Text subText;

    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button answerButton;
    [SerializeField] private Button confirmButton;
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

    [Networked] private int currentAnswerIndex { get; set; }
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
        loadingTextAnimation = DOTween.Sequence();
        loadingTextAnimation.AppendCallback(() => subText.text = "Listening.");
        loadingTextAnimation.AppendInterval(period);
        loadingTextAnimation.AppendCallback(() => subText.text = " Listening..");
        loadingTextAnimation.AppendInterval(period);
        loadingTextAnimation.AppendCallback(() => subText.text = "  Listening...");
        loadingTextAnimation.AppendInterval(period);
        loadingTextAnimation.SetLoops(-1, LoopType.Restart);
    }

    private void Awake()
    {
        subText.text = winMegasenaText;
        topText.text = answerRankJustifyText;

        startButton.onClick.AddListener(SettingAnswers);
        answerButton.onClick.AddListener(StartVoiceExperience);
        displayAnswersButton.onClick.AddListener(DisplayAnswers);
        retryButton.onClick.AddListener(RetryAnswer);
        hierarchizyButton.onClick.AddListener(SetupHierarchyFunctions);
        concluirButton.onClick.AddListener(SendData);
        confirmButton.onClick.AddListener(SaveAnswer);

        VoiceExperienceHandler.OnCheckVoiceOutput += CheckVoiceOutput;
        VoiceExperienceHandler.OnVoiceOutputError += VoiceOutputError;
    }
    private void SendData()
    {
        print("DataBase");
        databaseManager.CreateUser();
        DOVirtual.DelayedCall(1, ActivateLastUI);
        AnswerUI.GetComponent<AnswersPanelAnimations>().DisableUIAnimation();
    }

    private void ActivateLastUI() => finalUI.SetActive(true);

    private void SettingAnswers()
    {
        print("Setting Answers");

        topText.text = answersList.Count == 0 ? answerQuestionsText : answerNextQuestionsText;
        subText.text = pressButtonToAnswerText;

        startButton.gameObject.SetActive(false);
        confirmButton.gameObject.SetActive(false);

        answerButton.gameObject.SetActive(true);
    }

    private void StartVoiceExperience()
    {
        print("StartVoiceExperience");
        topText.text = "";
        subText.text = "";

        LoadingTextAnimation();

        answerButton.gameObject.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        OnStartVoiceExperience?.Invoke();
    }

    private void CheckVoiceOutput(string voiceOutput)
    {
        print("CheckVoiceOutput");
        loadingTextAnimation.Kill();

        topText.text = isWhatYouMeantText;
        subText.text = voiceOutput;

        startButton.gameObject.SetActive(false);
        answerButton.gameObject.SetActive(false);

        confirmButton.onClick.RemoveAllListeners();

        confirmButton.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        print("Justifying: " + Justifying);

        if (Justifying)
        {
            confirmButton.onClick.AddListener(FillJustifyText);
            // displayAnswersButton.gameObject.SetActive(true);
        }
        else
        {
            confirmButton.onClick.AddListener(SaveAnswer);
            if (answersList.Count < 0) retryButton.gameObject.SetActive(false);
        }
    }

    private void RetryAnswer()
    {
        print("RetryAnswer");
        topText.text = "";
        subText.text = "";

        confirmButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        LoadingTextAnimation();
        OnStartVoiceExperience?.Invoke();
    }

    private void VoiceOutputError()
    {
        print("VoiceOutputError");
        loadingTextAnimation.Kill();

        topText.text = voiceOutputErrorText;
        subText.text = "";

        if (Justifying) displayAnswersButton.gameObject.SetActive(true);
        else startButton.gameObject.SetActive(true);
    }

    void JustifyEntry()
    {
        print("JustifyEntry");
        topText.text = justifyAnswerText;
        subText.text = "";
        Justifying = true;

        //  uiSpawnManager.SetActive(false);
        displayAnswersButton.gameObject.SetActive(true);
    }

    public void DisplayAnswers()
    {
        print("DisplayAnswers");
        displayAnswersButton.gameObject.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        subText.text = "";

        if (currentAnswerIndex < answersList.Count)
        {
            Answer currentAnswer = answersList[currentAnswerIndex];
            topText.text = currentAnswer.answerText;
            answerButton.gameObject.SetActive(true);
        }
        else
        {
            topText.text = "Muito bem! Você conseguiu!";
            hierarchizyButton.gameObject.SetActive(true);
        }
    }

    public void SaveAnswer()
    {
        print("SaveAnswer");
        retryButton.gameObject.SetActive(false);

        if (answersList.Count != 3)
        {
            Answer newAnswer = ScriptableObject.CreateInstance<Answer>();
            newAnswer.answerText = subText.text;
            answersList.Add(newAnswer);
            SettingAnswers();
        }

        if (answersList.Count == 3)
        {
            answerButton.gameObject.SetActive(false);
            topText.text = "";
            subText.text = "";
            currentAnswerIndex = 0;
            //      uiSpawnManager.SetActive(true);
            JustifyEntry();
         //   SetupHierarchyFunctions();
        }
    }

    public void FillJustifyText()
    {
        print("FillJustifyText");
        // obtém a resposta correspondente ao índice atual
        Answer currentAnswer = answersList[currentAnswerIndex];

        // atualiza a propriedade JustifyText com o valor do campo de entrada de texto
        currentAnswer.justifyText = topText.text;

        // incrementa o índice para exibir a próxima resposta na próxima interação
        currentAnswerIndex++;

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
        hierarchizyButton.gameObject.SetActive(false);
        displayAnswersButton.gameObject.SetActive(false);
        concluirButton.gameObject.SetActive(true);
        topText.text = "";
        subText.text = "";

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
