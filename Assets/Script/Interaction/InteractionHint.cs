using TMPro;
using UnityEngine;

public class InteractionHint : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hintText;
    private void Start()
    {
        hintText.gameObject.SetActive(true);
        hintText.text = " ";
        hintText.gameObject.SetActive(false);
    }
    public void ShowHint(Vector3 position, string message)
    {
        hintText.gameObject.SetActive(true);
        hintText.rectTransform.position = position;
        hintText.text = message;
    }
    public void HideHint()
    {
        if (hintText == null) return;

        if (!hintText.gameObject.activeInHierarchy) return;

        hintText.gameObject.SetActive(false);
    }
}
