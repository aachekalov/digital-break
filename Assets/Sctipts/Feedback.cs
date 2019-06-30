using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Feedback : MonoBehaviour {
    public InputField input;
    public ScrollRect chat;
    //public RectTransform myMessage;
    //public RectTransform yourMessage;
    public RectTransform messageRow;

    public void OnSend()
    {
        RectTransform row = Instantiate(messageRow);
        row.SetParent(chat.content);
        Text messageText = row.GetChild(0).GetChild(0).GetComponent<Text>();
        messageText.text = input.text;
        input.text = string.Empty;
        Image messageImage = row.GetChild(0).GetComponent<Image>();
        messageImage.rectTransform.anchorMin = Vector2.one;
        messageImage.rectTransform.anchorMax = Vector2.one;
        messageImage.rectTransform.pivot = Vector2.one;
        messageImage.rectTransform.anchoredPosition = Vector2.zero;
        StartCoroutine(size(messageImage, row, messageText));
        Answer("Ваше обращение получено! Пожалуйста, ожидайте ответа");
    }

    private IEnumerator size(Image messageImage, RectTransform row, Text messageText)
    {
        yield return null;
        messageImage.rectTransform.sizeDelta = new Vector2(messageImage.rectTransform.sizeDelta.x, messageText.rectTransform.sizeDelta.y + 20);
        row.sizeDelta = new Vector2(row.sizeDelta.x, messageText.rectTransform.sizeDelta.y + 20);
    }

    public void Answer(string str)
    {
        RectTransform row = Instantiate(messageRow);
        row.SetParent(chat.content);
        Text messageText = row.GetChild(0).GetChild(0).GetComponent<Text>();
        messageText.text = str;
        Image messageImage = row.GetChild(0).GetComponent<Image>();
        messageImage.rectTransform.anchorMin = Vector2.up;
        messageImage.rectTransform.anchorMax = Vector2.up;
        messageImage.rectTransform.pivot = Vector2.up;
        messageImage.rectTransform.anchoredPosition = Vector2.zero;
    }
}
