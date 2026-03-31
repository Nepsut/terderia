using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource, sfxSource;

    [Header("Generic SFX clips")]
    [SerializeField] private AudioClip damagePlayerClip;
    [SerializeField] private AudioClip healPlayerClip, impactClip;

    [Header("Card SFX clips")]
    [SerializeField] private AudioClip speechCardClip;
    [SerializeField] private AudioClip utilityCardClip, weaponCardClip,
        iceCardClip, fireCardClip, poisonCardClip, earthCardClip, lightningCardClip,
        cuttingCardClip, bluntCardClip, healingCardClip;
    

    private void Start()
    {
        GameManager.OnPlayerHealthLoss += _ => PlaySfx(damagePlayerClip);
        GameManager.OnPlayerHealthGain += _ => PlaySfx(healPlayerClip);
    }

    public void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;

        if (GameManager.Instance.DebugModeOn) Debug.Log($"Playing SFX with clip {clip.name}.");
        sfxSource.PlayOneShot(clip);
    }
}