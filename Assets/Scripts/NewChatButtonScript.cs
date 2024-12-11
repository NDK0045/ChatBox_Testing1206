using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class NewChatButtonScript : MonoBehaviour
{
    public Button newChatButton; // Reference to the new chat button
    public MessageContainer messageContainer; // Reference to the message container
    public MessageSender messageSender; // Reference to the message sender (to reset AI connection)

    private void Awake()
    {
        if (newChatButton != null)
        {
            newChatButton.onClick.AddListener(StartNewChat);
        }
    }

    private void StartNewChat()
    {
        // Clear all previous messages
        messageContainer.ClearMessageHistory();

        // Optionally reset any other data in the message sender to start fresh
        messageSender.ResetChatSession();

        Debug.Log("New chat started!");
    }
}

