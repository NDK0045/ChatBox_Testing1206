using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json; // Ensure you have the Newtonsoft.Json package installed
using System;
using System.Collections; // For IEnumerator
using TMPro; // For TextMeshPro fields
using UnityEngine;
using UnityEngine.InputSystem; // For InputAction
using UnityEngine.Networking; // For UnityWebRequest


public class MessageSender : MonoBehaviour
{
    public TMP_InputField MessageField;
    public Chat Chat;
    private const string ApiUrl = "https://api.openai.com/v1/chat/completions"; // Verify this is the correct endpoint
    private const string ApiKey = ""; // Replace with your OpenAI API key
    private void Update()
    {
        // Check if the Enter key is pressed and the message field is not empty
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(MessageField.text))
        {
            // Call the Send function
            Send();
        }
    }
    public void Send()
    {
        if (string.IsNullOrEmpty(MessageField.text))
            return;

        string userMessage = MessageField.text;
        MessageField.text = string.Empty;

        // Add the user message to the chat container
        var message = new Message("User", userMessage); // Assuming "User" is the sender's name
        Chat.ReceiveMessage(message);  // This calls AddMessage() in MessageContainer

        StartCoroutine(GetGPTResponse(userMessage));
    }

    private IEnumerator GetGPTResponse(string userMessage)
    {
        // Create the request payload
        var payload = new
        {
            model = "gpt-3.5-turbo", // Ensure you're using a valid model
            messages = new[]
            {
            new { role = "system", content = "You are a helpful assistant." },
            new { role = "user", content = userMessage }
        },
            max_tokens = 100,
            temperature = 0.7
        };

        string jsonPayload = JsonConvert.SerializeObject(payload);

        // Create the HTTP request
        UnityWebRequest request = new UnityWebRequest(ApiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {ApiKey}");

        // Send the request
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}\n{request.downloadHandler.text}");
        }
        else
        {
            Debug.Log($"Response: {request.downloadHandler.text}");
            ProcessResponse(request.downloadHandler.text);
        }
    }

    private void ProcessResponse(string jsonResponse)
    {
        // Parse the JSON response and extract the AI's message
        var response = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

        // Extract the AI's response (assuming the format returned by OpenAI)
        string aiMessage = response.choices[0].message.content;

        // Display the AI's response in the chat with a different sender label
        var message = new Message("AI", aiMessage); // This makes sure the sender is "AI"
        Chat.ReceiveMessage(message);
    }
    public void ResetChatSession()
    {
        // Reset any session variables related to the chat, such as conversation context or API state
        // For example, if you're using an API with conversation history, you might clear the context here.
        Debug.Log("AI conversation session reset.");
    }


}
