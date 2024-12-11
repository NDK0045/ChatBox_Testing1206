using DG.Tweening;
using UnityEngine;

public class DeleteMessageMode : MonoBehaviour
{
    public MessageContainer Container;
    public GameObject DeleteMessageArea;
    public GameObject SendMessageArea;
    public float TransitionTime;

    public void SetActive(bool value)
    {
        var offset = value ? -100.0f : 0.0f;
        Container.ContainerObject.DOAnchorPosX(offset, TransitionTime);

        DeleteMessageArea.SetActive(value);
        SendMessageArea.SetActive(!value);
    }

    // New method to clear all messages in the chat
    public void ClearAllMessages()
    {
        // Clear all message presenters from the container
        foreach (Transform child in Container.ContainerObject)
        {
            Destroy(child.gameObject);
        }

        // Optionally reset the list of messages and any other state
        Container.ClearMessageHistory();
    }

    private void Reset() =>
        Container = FindObjectOfType<MessageContainer>();
}
