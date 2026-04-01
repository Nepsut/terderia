using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public class CardData
    {
        public readonly int index;
        public readonly string name;
        public readonly string id;
        public readonly string description;
        public readonly CardType cardType;
        public readonly SpellSchool spellSchool;
        public readonly DamageType damageType;
        public readonly Strength strength;
        public readonly Range range;
        public readonly AreaOfEffect areaOfEffect;
        public readonly List<string> otherTags;
        public readonly Sprite sprite;
        public readonly AudioClip audioClip;
        public bool HasAudio => audioClip != null;

        public CardData(int index, string name, string id, string description, CardType cardType,
        SpellSchool spellSchool = SpellSchool.none, DamageType damageType = DamageType.none, 
        Strength strength = Strength.none, Range range = Range.touch,
        AreaOfEffect areaOfEffect = AreaOfEffect.none, List<string> otherTags = null)
        {
            this.index = index;
            this.name = name;
            this.id = id;
            this.description = description;
            this.cardType = cardType;
            this.damageType = damageType;
            this.spellSchool = spellSchool;
            this.strength = strength;
            this.range = range;
            this.areaOfEffect = areaOfEffect;
            this.otherTags = otherTags ?? new();
            sprite = Resources.Load<Sprite>($"CardSystem/CardSprites/{id}");
            if (sprite == null) Debug.LogWarning($"Sprite for card {this.id} couldn't be found and wasn't set.");
            audioClip = Resources.Load<AudioClip>($"CardSystem/CardSFX/{id}");
            if (audioClip != null) return;
            
            if (GameManager.Instance.DebugModeOn)
                Debug.Log($"SFX for card {this.id} couldn't be found, setting best generic audio.");
            
            if (cardType == CardType.speech)
                audioClip = AudioManager.Instance.SpeechCardSfx;
            else if (cardType == CardType.utility)
                audioClip = AudioManager.Instance.UtilityCardSfx;
            else if (damageType == DamageType.fire)
                audioClip = AudioManager.Instance.FireCardSfx;
            else if (damageType == DamageType.ice)
                audioClip = AudioManager.Instance.IceCardSfx;
            else if (damageType == DamageType.lightning)
                audioClip = AudioManager.Instance.LightningCardSfx;
            else if (damageType == DamageType.earth)
                audioClip = AudioManager.Instance.EarthCardSfx;
            else if (damageType == DamageType.poison)
                audioClip = AudioManager.Instance.PoisonCardSfx;
            else if (damageType == DamageType.cutting)
                audioClip = AudioManager.Instance.CuttingCardSfx;
            else if (damageType == DamageType.blunt)
                audioClip = AudioManager.Instance.BluntCardSfx;

            if (audioClip == null)
            Debug.LogWarning($"Could not find a valid generic replacement SFX for card {this.id}!");
        }

        private void SetFittingSfx()
        {
        }

        public enum CardType
        {
            spell = 0,
            weapon,
            speech,
            utility
        }

        public enum SpellSchool
        {
            none = 0,
            elemental,
            trickery,
            conjuration,
            protection,
            swordcery
        }

        public enum DamageType
        {
            none = 0,
            fire,
            ice,
            lightning,
            earth,
            poison,
            cutting,
            blunt
        }

        public enum Strength
        {
            none = 0,
            low,
            medium,
            high
        }

        public enum Range
        {
            touch = 0,
            low,
            medium,
            high
        }

        public enum AreaOfEffect
        {
            none = 0,
            small,
            medium,
            large
        }
    }
}