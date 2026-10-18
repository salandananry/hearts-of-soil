using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private float typingSpeed;

    private Coroutine typingCoroutine;
    private string currentText;

    public void ShowText(string text)
    {
        currentText = text;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textField.text = "";

        foreach (char character in currentText)
        {
            textField.text += character;
            yield return new WaitForSeconds(typingSpeed);
        }

        typingCoroutine = null;
    }

    public void SkipTyping()
    {
        if (typingCoroutine == null)
        {
            return;
        }

        StopCoroutine(typingCoroutine);
        textField.text = currentText;
        typingCoroutine = null;
    }
}