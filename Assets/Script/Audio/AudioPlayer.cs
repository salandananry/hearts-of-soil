using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance;

    [SerializeField] private AudioSource musicSpeaker;
    [SerializeField] private AudioSource sfxSpeaker;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void PlayMusic(AudioClip clip)
    {
        if (musicSpeaker.clip == clip) return;

        musicSpeaker.clip = clip;
        musicSpeaker.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSpeaker.clip = clip;
        sfxSpeaker.Play();
    }
}
