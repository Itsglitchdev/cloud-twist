using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;

    [Header("AudioClip")]
    [SerializeField] AudioClip cloudFlipSound;
    [SerializeField] AudioClip cloudMatchSound;
    [SerializeField] AudioClip cloudMismatchSound;
    [SerializeField] AudioClip gameWinSound;
    [SerializeField] AudioClip gameOverSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayCloudFlipSound() => audioSource.PlayOneShot(cloudFlipSound);
    public void PlayCloudMatchSound() => audioSource.PlayOneShot(cloudMatchSound);
    public void PlayCloudMismatchSound() => audioSource.PlayOneShot(cloudMismatchSound);
    public void PlayGameWinSound() => audioSource.PlayOneShot(gameWinSound);
    public void PlayGameOverSound() => audioSource.PlayOneShot(gameOverSound);
}