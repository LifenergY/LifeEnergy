using System;
using UnityEngine;
using Firebase.Database;
using System.Collections.Generic;

public class DatabaseManager : MonoBehaviour
{
    public AnswersManager nAnswerManager;

    private DatabaseReference dbReference;

    private void Awake() => nAnswerManager = FindObjectOfType<AnswersManager>();

    private void Start() => dbReference = FirebaseDatabase.DefaultInstance.RootReference.Child("Pacientes");

    public void CreateUser()
    {
        // Crie uma nova refer�ncia para o n� "Pacientes"
        DatabaseReference pacientesReference = FirebaseDatabase.DefaultInstance.GetReference("Pacientes");

        // Para cada valor diferente de PlayerPrefs, crie um novo n� de usu�rio em "Pacientes"
        foreach (string playerPrefValue in PlayerPrefs.GetString("PlayerName").Split(','))
        {
            // Usa o valor da PlayerPrefs para criar um novo nome de usu�rio exclusivo com a data atual
            string uniqueUsername = "Pacientes: " + PlayerPrefsManager.GetPlayerName() + " " + "Psicologo: " + "psicologoName" +
                                    DateTime.Now.ToString("D: dd-MM-yyyy  HH:mm:ss");

            // Usa o nome de usu�rio exclusivo para criar um novo n� de usu�rio em "Pacientes"
            DatabaseReference userReference = pacientesReference.Child(uniqueUsername);

            // Crie um novo n� dentro do n� do usu�rio para armazenar as respostas
            DatabaseReference answersReference = userReference.Child("Respostas");

            // Converte a lista de Scriptable Objects em um dicion�rio
            Dictionary<string, object> dict = new Dictionary<string, object>();
            for (int i = 0; i < nAnswerManager.answersList.Count; i++)
            {
                string answer = nAnswerManager.answersList[i].answerText.Replace("Jogador: ", "");
                string priority = nAnswerManager.answersList[i].priorityText;
                string justify = nAnswerManager.answersList[i].justifyText.Replace("Jogador: ", "");

                string formattedAnswer =
                    $"Resposta:{answer}{Environment.NewLine}Prioridade: {priority}{Environment.NewLine}Justificativa: {justify}{Environment.NewLine}{Environment.NewLine}";

                dict.Add(i.ToString(), formattedAnswer);
            }

            // Envia o dicion�rio para o Firebase
            answersReference.SetValueAsync(dict).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SetValueAsync was canceled.");
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError("SetValueAsync encountered an error: " + task.Exception);
                }
                else
                {
                    Debug.Log("Data uploaded successfully!");
                }
            });
        }
    }
}