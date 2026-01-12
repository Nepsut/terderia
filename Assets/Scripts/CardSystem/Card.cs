namespace CardSystem
{
    public class Card
    {
        public readonly string name;
        public readonly string id;
        public readonly string description;
        public readonly CardType cardType;
        public readonly DamageType damageType;
        public readonly SpellSchool spellSchool;
        public readonly int strength;
        public readonly int range;
        public readonly AreaOfEffect areaOfEffect;

        public Card(string name, string id, string description, CardType cardType, DamageType damageType, SpellSchool spellSchool,
        int strength = 0, int range = 0, AreaOfEffect areaOfEffect = AreaOfEffect.none)
        {
            this.name = name;
            this.id = id;
            this.description = description;
            this.cardType = cardType;
            this.damageType = damageType;
            this.spellSchool = spellSchool;
            this.strength = strength;
            this.range = range;
            this.areaOfEffect = areaOfEffect;
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
            cutting,
            blunt
        }

        public enum SpellSchool
        {
            none = 0,
            elemental,
            trickery,
            necromancy
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