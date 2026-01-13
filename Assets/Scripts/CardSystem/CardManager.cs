using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CardSystem
{
    public class CardManager : MonoBehaviour
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

        public Card[] Cards { get; private set; }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private async void Start()
        {
            string tsvAsString = cardTsvAsset.text;
            await Task.Run(() => AssignCardDataFromTsv(tsvAsString));
            
            foreach(Card card in Cards)
            {
                Debug.Log(card.name);
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
            List<Card> tempCards = new();

            for (int i = 1; i < cardDataAsStrings.Length; i++)
            {
                string[] cardValues = cardDataAsStrings[i].Split('\t');
                Card.SpellSchool cardSchool = cardValues[schoolIndex] != "" && cardValues[schoolIndex] != null ?
                    Enum.Parse<Card.SpellSchool>(cardValues[schoolIndex], ignoreCase: true) : Card.SpellSchool.none;
                Card.DamageType cardDamage = cardValues[damageIndex] != "" && cardValues[damageIndex] != null ?
                    Enum.Parse<Card.DamageType>(cardValues[damageIndex], ignoreCase: true) : Card.DamageType.none;
                if (!int.TryParse(cardValues[strengthIndex], out int cardStrength)) cardStrength = 0;
                if (!int.TryParse(cardValues[rangeIndex], out int cardRange)) cardRange = 0;
                Card.AreaOfEffect cardAoe = cardValues[aoeIndex] != "" && cardValues[aoeIndex] != null ?
                    Enum.Parse<Card.AreaOfEffect>(cardValues[aoeIndex], ignoreCase: true) : Card.AreaOfEffect.none;
                string[] cardTags;
                if (cardValues[tagsIndex] != "" && cardValues[tagsIndex] != null) cardTags = cardValues[tagsIndex].Split(',');
                else cardTags = null;

                tempCards.Add(new(name: cardValues[nameIndex], id: cardValues[idIndex], description: cardValues[descriptionIndex],
                    cardType: Enum.Parse<Card.CardType>(cardValues[typeIndex], ignoreCase: true), damageType: cardDamage, spellSchool: cardSchool,
                    strength: cardStrength, range: cardRange, areaOfEffect: cardAoe, otherTags: cardTags));
            }

            Cards = tempCards.ToArray();
        }
    }
}