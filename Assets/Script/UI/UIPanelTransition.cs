using UnityEngine;
using DG.Tweening;

public class UIPanelTransition : MonoBehaviour
{
    [SerializeField] private float startScale;
    [SerializeField] private float duration;
    [SerializeField] private Ease easeType;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void Open()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        rectTransform.DOKill();
        rectTransform.localScale = Vector3.one * startScale;
        rectTransform.DOScale(Vector3.one, duration)
            .SetEase(easeType)
            .SetUpdate(true);
    }
    public void Close()
    {
        rectTransform.DOKill();
        rectTransform.DOScale(Vector3.one * startScale, duration)
            .SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}