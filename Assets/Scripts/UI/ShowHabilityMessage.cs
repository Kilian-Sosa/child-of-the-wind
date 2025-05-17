using System.Collections;
using UnityEngine;
using TMPro;

public class ShowHabilityMessage : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public float typingSpeed = 0.05f;
    [SerializeField] GameObject _messsagePanel;

    public void ShowMessage(string message)
    {
        _messsagePanel.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(TypeMessage(message));
    }

    public void HideMessage()
    {
        _messsagePanel.SetActive(false);
        Time.timeScale = 1f;
        StopAllCoroutines();
        messageText.text = "";
    }

    private IEnumerator TypeMessage(string message)
    {
        messageText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            messageText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }
}
