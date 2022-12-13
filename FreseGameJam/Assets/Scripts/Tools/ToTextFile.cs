using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ToTextFile : MonoBehaviour
{
    //public InputField inputFieldChat;
   [SerializeField] TMP_InputField inputFieldChat;


    void Start()
    {
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Chat_Logs/");

    }


    public void CreateTextFile()
    {

        if (inputFieldChat.text == "")
        {
            return;
        }

        string txtDocumentName = Application.streamingAssetsPath + "/Chat_Logs/" + "Chat" + ".txt";

        if (!File.Exists(txtDocumentName))
        {
            File.WriteAllText(txtDocumentName, "Title of my chat log \n\n");

        }

        File.AppendAllText(txtDocumentName, inputFieldChat.text + "/n");

        inputFieldChat.text = "";
    }


}
