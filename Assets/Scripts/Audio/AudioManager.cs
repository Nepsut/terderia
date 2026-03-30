using CardSystem;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioClip speechCardClip, utilityCardClip, weaponCardClip,
        iceCardClip, fireCardClip, poisonCardClip, earthCardClip, lightningCardClip,
        cuttingCardClip, bluntCardClip, healingCardClip;
        

    private void Start()
    {
        EventManager.OnCardUsed += HandleCardSfx;
    }

    private void HandleCardSfx(PlayingCard card)
    {
        CardData data = card.CardData;

        //if data.HasAudio, use data.audioClip
        //if not, use a fitting clip from this script
    }
}