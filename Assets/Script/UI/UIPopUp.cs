using UnityEngine;
using DG.Tweening;

public class UIPopUp : MonoBehaviour
{
    [SerializeField] private float startScale;
    [SerializeField] private float duration;
    [SerializeField] private Ease easeType;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        rectTransform.DOKill();
        rectTransform.localScale = Vector3.one * startScale;
        rectTransform.DOScale(Vector3.one, duration).SetEase(easeType).SetUpdate(true);
    }
    public void Close()
    {
        rectTransform.DOScale(Vector3.one * startScale, duration)
            .SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}