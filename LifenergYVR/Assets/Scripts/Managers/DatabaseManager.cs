using System;
using UnityEngine;
using Firebase.Database;
using System.Collections.Generic;
using TMPro;
using Fusion;

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
            //Fix and get the name of the psychologist
            //TODO
            var tempName = GetTMPTextFromNetworkPlayers();
            string psychologistName="No Psychologist Present";
            if (tempName != null) psychologistName = tempName;


            // Usa o valor da PlayerPrefs para criar um novo nome de usu�rio exclusivo com a data atual
            string uniqueUsername = "Pacientes: " + PlayerPrefsManager.GetPlayerName() + " " + "Psicologo: " + psychologistName +
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
                    $"Resposta:{answer}{Environment.NewLine} \n Prioridade: {priority}{Environment.NewLine} \n Justificativa: {justify}{Environment.NewLine}{Environment.NewLine}";

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

        // Function to get TMP text from child of 'Network Player(Clone)' objects
        public string GetTMPTextFromNetworkPlayers()
        {
            // Find all 'Network Player(Clone)' objects in the scene
            GameObject[] networkPlayers = GameObject.FindGameObjectsWithTag("Player");

            // Iterate through each network player
            foreach (GameObject networkPlayer in networkPlayers)
            {
                // Check if the object does not have authority
                NetworkObject networkIdentity = networkPlayer.GetComponent<NetworkObject>();
                if (networkIdentity != null && !networkIdentity.HasStateAuthority)
                {
                    // Look for 'Name Panel' child
                    Transform namePanel = networkPlayer.transform.Find("Name Panel");
                    if (namePanel != null)
                    {
                        // Look for 'tmp' child and get TMP_Text component
                        TMP_Text tmpText = namePanel.GetChild(0).GetComponent<TMP_Text>();
                        if (tmpText != null)
                        {
                            // Return the text of the TMP_Text component
                            return tmpText.text;
                        }
                    }
                }
            }

            // If no such text is found, return null
            return null;
        }
    
}