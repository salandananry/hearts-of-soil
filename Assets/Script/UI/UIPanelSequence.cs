using System.Collections;
using UnityEngine;

public class UIPanelSequence : MonoBehaviour
{
    [SerializeField] private UIPanelTransition[] panels;

    private void OnEnable()
    {
        StartCoroutine(OpenSequence());
    }
    private void OnDisable()
    {
        foreach (var panel in panels)
        {
            panel.gameObject.SetActive(false);
        }
    }
    private IEnumerator OpenSequence()
    {
        yield return new WaitForSeconds(0.25f);
        foreach (var panel in panels)
        {
            panel.Open();
            yield return new WaitForSeconds(0.25f);
        }
    }
}