using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class UIButtonSFX : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClick;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        AudioPlayer.Instance.PlaySFX(buttonClick);
    }
}