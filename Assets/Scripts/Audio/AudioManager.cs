using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioMixer masterVolume;

    [Header("Audio Settings Elements")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music Clips")]
    [field: SerializeField] public AudioClip MenuMusic { get; private set; }
    [field: SerializeField] public AudioClip Map1Music { get; private set; }
    [field: SerializeField] public AudioClip CaveMusic { get; private set; }

    [Header("Generic SFX Clips")]
    [SerializeField] private AudioClip damagePlayerSfx;
    [SerializeField] private AudioClip healPlayerSfx;
    [SerializeField] private AudioClip impactSfx;

    [Header("Generic Card SFX Clips")]
    [field: SerializeField] public AudioClip SpeechCardSfx { get; private set; }
    [field: SerializeField] public AudioClip UtilityCardSfx { get; private set; }
    [field: SerializeField] public AudioClip IceCardSfx { get; private set; }
    [field: SerializeField] public AudioClip FireCardSfx { get; private set; }
    [field: SerializeField] public AudioClip PoisonCardSfx { get; private set; }
    [field: SerializeField] public AudioClip EarthCardSfx { get; private set; }
    [field: SerializeField] public AudioClip LightningCardSfx { get; private set; }
    [field: SerializeField] public AudioClip CuttingCardSfx { get; private set; }
    [field: SerializeField] public AudioClip BluntCardSfx { get; private set; }
    [field: SerializeField] public AudioClip HealingCardSfx { get; private set; }

    private const float musicFadeOutTime = 0.64f;
    private const float musicFadeInTime = 0.64f;
    private int musicTransitionOutTween = -1;
    private int musicTransitionInTween = -1;
    

    private void Start()
    {
        GameManager.OnPlayerHealthLoss += _ => PlaySfx(damagePlayerSfx);
        GameManager.OnPlayerHealthGain += _ => PlaySfx(healPlayerSfx);
        // masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        // musicVolumeSlider.onValueChanged.AddListener(SetMasterVolume);

        musicSource.clip = MenuMusic;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Tried to call PlaySfx with null AudioClip!");
            return;
        }

        if (GameManager.Instance.DebugModeOn) Debug.Log($"Playing SFX with clip {clip.name}.");
        sfxSource.PlayOneShot(clip);
    }

    public void ChangeMusic(AudioClip newMusic)
    {
        if (newMusic == null)
        {
            Debug.LogWarning("Tried to call ChangeMusic with null AudioClip!");
            return;
        }
        if (newMusic == musicSource.clip)
        {
            if (GameManager.Instance.DebugModeOn)
                Debug.Log("Ignored ChangeMusic call made with already active music clip.");
            return;
        }

        float savedVolume = musicSource.volume;
        musicTransitionOutTween = LeanTween.value(gameObject, vol =>
        {
            musicSource.volume = vol;
        }, savedVolume, 0f, musicFadeOutTime).setOnComplete(() =>
        {
            musicTransitionOutTween = -1;
            musicSource.clip = newMusic;
            musicSource.Play();
            musicTransitionInTween = LeanTween.value(gameObject, vol =>
            {
                musicSource.volume = vol;
            }, 0f, savedVolume, musicFadeInTime).
            setOnComplete(() => musicTransitionOutTween = -1).id;
        }).id;
    }

    private void SetMasterVolume(float value)
    {
        masterVolume.SetFloat("Volume", Mathf.Log10(value / 100) * 20f);
    }

    private void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    private void SetSfxVolume(float value)
    {
        sfxSource.volume = value;
    }
}