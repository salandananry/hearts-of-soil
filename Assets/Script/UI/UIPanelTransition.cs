using UnityEngine;
using DG.Tweening;

public class UIPanelTransition : MonoBehaviour
{
    [SerializeField] private Vector3 originalScale = Vector3.one;
    [SerializeField] private float duration;
    [SerializeField] private Ease easeType;
    private Vector3 startScale = Vector3.zero;
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