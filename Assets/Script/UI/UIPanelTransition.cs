using UnityEngine;
using DG.Tweening;

public class UIPanelTransition : MonoBehaviour
{
    [SerializeField] Vector3 startScale = Vector3.zero;
    [SerializeField] private float duration;
    [SerializeField] private Ease easeType;
    private RectTransform rectTransform;    
    private Vector3 originalScale;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = transform.localScale;
    }
    public void Open()
    {
        gameObject.SetActive(true);

        transform.DOKill();

        transform.DOScale(originalScale, duration)
            .SetEase(easeType)
            .SetUpdate(true);
    }
    public void Close()
    {
        transform.DOKill();

        transform.DOScale(startScale, duration)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
}