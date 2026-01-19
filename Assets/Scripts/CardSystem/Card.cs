using System;
using UnityEngine;

namespace CardSystem
{
    public class Card
    {
        public readonly int index;
        public readonly string name;
        public readonly string id;
        public readonly string description;
        public readonly CardType cardType;
        public readonly DamageType damageType;
        public readonly SpellSchool spellSchool;
        public readonly Strength strength;
        public readonly Range range;
        public readonly AreaOfEffect areaOfEffect;
        public readonly string[] otherTags;
        public readonly Sprite sprite;

        public Card(int index, string name, string id, string description, CardType cardType, DamageType damageType = DamageType.none,
        SpellSchool spellSchool = SpellSchool.none, Strength strength = Strength.none, Range range = Range.self,
        AreaOfEffect areaOfEffect = AreaOfEffect.none, string[] otherTags = null)
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
            this.otherTags = otherTags ?? (new string[0]);
            sprite = Resources.Load<Sprite>($"Cards/{id}");
            if (sprite == null) Debug.LogWarning($"Sprite for card {this.id} couldn't be found and wasn't set.");
        }

        public enum CardType
        {
            spell = 0,
            weapon,
            speech,
            utility
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
            strong
        }

        public enum Range
        {
            self = 0,
            melee,
            low,
            medium,
            high
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

        public enum AreaOfEffect
        {
            none = 0,
            small,
            medium,
            large
        }
    }
}