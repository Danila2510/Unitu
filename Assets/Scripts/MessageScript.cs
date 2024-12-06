using System.Collections.Generic;
using UnityEngine;

public class MessageScript : MonoBehaviour
{
    private float timeout = 2.0f;
    private float leftTime;
    private GameObject content;
    private TMPro.TextMeshProUGUI messageTMP;
    private static MessageScript instance;
    private static Queue<Message> messageQueue = new Queue<Message>();

    void Start()
    {
        instance = this;

        content = transform.Find("Content").gameObject;
        messageTMP = transform.Find("Content/MessageText").GetComponent<TMPro.TextMeshProUGUI>();
        leftTime = 0;

        GameState.AddEventListener(OnGameEvent);
    }

    void Update()
    {
        if (leftTime > 0)
        {
            leftTime -= Time.deltaTime;
            if (leftTime <= 0)
            {
                messageQueue.Dequeue();
                content.SetActive(false);
            }
        }
        else if (messageQueue.Count > 0)
        {
            Message message = messageQueue.Peek();
            messageTMP.text = message.GetFormattedText();
            leftTime = message.Timeout ?? timeout;
            content.SetActive(true);
        }
    }

    private void OnGameEvent(string eventName, object data)
    {
        {
            if (data is GameEvents.IMessage m)
            {
                ShowMessage(m.message);
            }
        }
        
    }

    private void OnDestroy()
    {
        GameState.RemoveEventListener(OnGameEvent);
    }
    public static void ShowMessage(string messageText, string author = null, float? timeout = null)
    {
        foreach (var msg in messageQueue)
        {
            if (msg.Text == messageText && msg.Author == author)
            {
                Debug.Log($"Duplicate message ignored: '{messageText}'");
                return;
            }
        }

        messageQueue.Enqueue(new Message(messageText, author, timeout));
    }
    private class Message
    {
        public string Text { get; }
        public string Author { get; }
        public float? Timeout { get; }

        public Message(string text, string author = null, float? timeout = null)
        {
            Text = text;
            Author = author;
            Timeout = timeout;
        }
        public string GetFormattedText()
        {
            return Author != null ? $"{Author}: {Text}" : Text;
        }
    }
}
