using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public class CardManager : MonoSingleton<CardManager>
    {
        [SerializeField] private TextAsset cardTsvAsset;
        private const string indexHeader = "Index";
        private const string nameHeader = "Name";
        private const string idHeader = "Unique Tag";
        private const string descriptionHeader = "Description";
        private const string typeHeader = "Type";
        private const string schoolHeader = "Spell School";
        private const string damageHeader = "Damage Type";
        private const string strengthHeader = "Strength";
        private const string rangeHeader = "Range";
        private const string aoeHeader = "AoE";
        private const string tagsHeader = "Other Tags";
        private const string notesHeader = "Additional Notes";

        public static Dictionary<string, Card> Cards { get; private set; }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            AssignCardDataFromTsv(cardTsvAsset.text);
            
            foreach(var keyValuePair in Cards)
            {
                Debug.Log($"{keyValuePair.Value.name}\t{keyValuePair.Value.description}");
            }
        }

        private void AssignCardDataFromTsv(string tsvAsString)
        {
            string[] cardDataAsStrings = tsvAsString.Split('\n');
            string[] headers = cardDataAsStrings[0].Split('\t');
            int indexIndex = Array.IndexOf(headers, indexHeader);
            int nameIndex = Array.IndexOf(headers, nameHeader);
            int idIndex = Array.IndexOf(headers, idHeader);
            int descriptionIndex = Array.IndexOf(headers, descriptionHeader);
            int typeIndex = Array.IndexOf(headers, typeHeader);
            int schoolIndex = Array.IndexOf(headers, schoolHeader);
            int damageIndex = Array.IndexOf(headers, damageHeader);
            int strengthIndex = Array.IndexOf(headers, strengthHeader);
            int rangeIndex = Array.IndexOf(headers, rangeHeader);
            int aoeIndex = Array.IndexOf(headers, aoeHeader);
            int tagsIndex = Array.IndexOf(headers, tagsHeader);
            int notesIndex = Array.IndexOf(headers, notesHeader);

            Cards = new();

            for (int i = 1; i < cardDataAsStrings.Length; i++)
            {
                string[] cardValues = cardDataAsStrings[i].Split('\t');
                if (!Enum.TryParse<Card.SpellSchool>(cardValues[schoolIndex], ignoreCase: true, out var cardSchool)) cardSchool = Card.SpellSchool.none;
                if (!Enum.TryParse<Card.DamageType>(cardValues[damageIndex], ignoreCase: true, out var cardDamage)) cardDamage = Card.DamageType.none;
                if (!Enum.TryParse<Card.Strength>(cardValues[strengthIndex], ignoreCase: true, out var cardStrength)) cardStrength = Card.Strength.none;
                if (!Enum.TryParse<Card.Range>(cardValues[rangeIndex], ignoreCase: true, out var cardRange)) cardRange = Card.Range.self;
                if (!Enum.TryParse<Card.AreaOfEffect>(cardValues[aoeIndex], ignoreCase: true, out var cardAoe)) cardAoe = Card.AreaOfEffect.none;
                string[] cardTags;
                if (cardValues[tagsIndex] != "" && cardValues[tagsIndex] != null)
                {
                    cardTags = cardValues[tagsIndex].Split(',');
                    for (int j = 0; j < cardTags.Length; j++)
                    {
                        cardTags[j] = cardTags[j].Replace("#", null);
                        cardTags[j] = cardTags[j].Replace(" ", null);
                    }
                }
                else cardTags = null;

                Cards.Add(cardValues[idIndex], new(name: cardValues[nameIndex], id: cardValues[idIndex].Replace("#", null),
                    description: cardValues[descriptionIndex], cardType: Enum.Parse<Card.CardType>(cardValues[typeIndex],
                    ignoreCase: true), damageType: cardDamage, spellSchool: cardSchool, strength: cardStrength, range: cardRange,
                    areaOfEffect: cardAoe, otherTags: cardTags));
            }
        }

        public static Sprite GetCardSprite(string cardId)
        {
            if (Cards == null) return null;
            if (!Cards.ContainsKey(cardId))
            {
                Debug.LogError($"Card ID {cardId} couldn't be found in dictionary, did you make a typo?");
            }

            return Cards[cardId].sprite;
        }
    }
}