using TMPro;
using UnityEngine;

public class InteractionHint : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hintText
        ;
    public void ShowHint(Vector3 position, string message)
    {
        hintText.gameObject.SetActive(true);
        hintText.rectTransform.position = position;
        hintText.text = message;
    }
    public void HideHint()
    {
        if (hintText == null && !hintText.gameObject.activeInHierarchy) return;
        hintText.gameObject.SetActive(false);
    }
}
