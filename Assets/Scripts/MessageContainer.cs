using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MessageContainer : MonoBehaviour
{
  public Chat Chat;
  public RectTransform ContainerObject;
  public GameObject MessagePrefab;
  public GameObject ChatOwnerMessagePrefab;

  private readonly List<MessagePresenter> _presenters = new List<MessagePresenter>();

  private void OnDestroy()
  {
    foreach (MessagePresenter presenter in _presenters)
      presenter.OnMessageDelete -= DeleteMessage;
  }

  private void Reset() =>
    Chat = FindObjectOfType<Chat>();

  public void AddMessage(Message message)
  {
    MessagePresenter presenter = InstantiatePresenter(message);
    presenter.OnMessageDelete += DeleteMessage;
  }

    private MessagePresenter InstantiatePresenter(Message message)
    {
        // Instantiate the appropriate prefab based on the sender
        MessagePresenter presenter = message.Sender == Chat.Owner
            ? Instantiate(ChatOwnerMessagePrefab, ContainerObject).GetComponent<MessagePresenter>()  // User's message
            : Instantiate(MessagePrefab, ContainerObject).GetComponent<MessagePresenter>();  // AI's message

        // Position and alignment for User's message
        if (message.Sender == Chat.Owner)  // User's message (right-aligned)
        {
            presenter.transform.SetParent(ContainerObject, false);
            // Adjust to right alignment
            RectTransform rect = presenter.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(1, 1);  // Align to right
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(1, 1);  // Ensure pivot is on the right side
        }
        else  // AI's message (left-aligned)
        {
            presenter.transform.SetParent(ContainerObject, false);
            // Adjust to left alignment
            RectTransform rect = presenter.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);  // Align to left
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);  // Ensure pivot is on the left side
        }

        // Update the last message if needed
        MessagePresenter lastMessage = _presenters.LastOrDefault();
        if (lastMessage && lastMessage.Message.Sender == message.Sender)
            lastMessage.Redraw(asLast: false);

        presenter.Message = message;
        _presenters.Add(presenter);

        return presenter;
    }

    private void DeleteMessage(Message message)
  {
    MessagePresenter presenter = _presenters.FirstOrDefault(o => o.Message == message);
    if (!presenter)
      return;
    
    RedrawPreviousIfNeeded(presenter);
    DestroyMessagePresenter(presenter);
  }

  private void DestroyMessagePresenter(MessagePresenter presenter)
  {
    presenter.OnMessageDelete -= DeleteMessage;
    _presenters.Remove(presenter);
    Destroy(presenter.gameObject);
  }

  private void RedrawPreviousIfNeeded(MessagePresenter presenter)
  {
    var index = _presenters.IndexOf(presenter);

    MessagePresenter previous = ValidIndex(index - 1) ? _presenters[index - 1] : null;

    MessagePresenter next = ValidIndex(index + 1) ? _presenters[index + 1] : null;

    if (ShouldRedrawPrevious())
      previous.Redraw(asLast: true);

    bool ShouldRedrawPrevious() =>
      previous && (!next || next && next.Message.Sender != presenter.Message.Sender);
  }

  private bool ValidIndex(int index) => 
    index >= 0 && index < _presenters.Count;

    public void ClearMessageHistory()
    {
        // Clear the list of presenters
        _presenters.Clear();
    }

}