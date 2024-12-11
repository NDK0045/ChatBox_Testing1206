using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class SaveChatLogButtonScript : MonoBehaviour
{
    public Button saveButton; // Reference to the save button
    public MessageContainer messageContainer; // Reference to the MessageContainer which holds the messages

    private void Awake()
    {
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveChatLog);
        }
    }

    private void SaveChatLog()
    {
        // Define the file path and name
        string filePath = Path.Combine(Application.persistentDataPath, "chat_log.txt");

        StringBuilder chatContent = new StringBuilder();

        // Iterate through all the messages in the message container
        foreach (var presenter in messageContainer.GetComponentsInChildren<MessagePresenter>())
        {
            chatContent.AppendLine($"{presenter.Message.Sender}: {presenter.Message.Content} ({presenter.Message.SendTime.ToString("HH:mm:ss")})");
        }

        // Write the chat log to the file
        try
        {
            File.WriteAllText(filePath, chatContent.ToString());
            Debug.Log($"Chat log saved to {filePath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to save chat log: {ex.Message}");
        }
    }
}

