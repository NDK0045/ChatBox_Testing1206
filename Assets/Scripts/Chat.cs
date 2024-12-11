using System.Collections.Generic;
using UnityEngine;

public class Chat : MonoBehaviour
{
    [HideInInspector] public string Owner = "Egor_Zhuchkov"; // Default chat owner (user)
    public MessageContainer Container;

    public void ReceiveMessage(Message message) =>
        Container.AddMessage(message);

    private void Reset() =>
        Container = FindObjectOfType<MessageContainer>();
}
