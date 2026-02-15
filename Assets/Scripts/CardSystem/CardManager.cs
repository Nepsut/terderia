using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardSystem
{
    public class CardManager : MonoSingleton<CardManager>
    {
        [SerializeField] private TextAsset cardTsvAsset;

        [Header("Card Base Sprite References")]
        [field: SerializeField] public Sprite CardbaseSpeech { get; private set; }
        [field: SerializeField] public Sprite CardbaseSpell { get; private set; }
        [field: SerializeField] public Sprite CardbaseUtility { get; private set; }
        [field: SerializeField] public Sprite CardbaseWeapon { get; private set; }
        [field: SerializeField] public Sprite CardbaseNull { get; private set; }

        [Header("Damage Type Sprite References")]
        [field: SerializeField] public Sprite DamageTypeBlunt { get; private set; }
        [field: SerializeField] public Sprite DamageTypeCutting { get; private set; }
        [field: SerializeField] public Sprite DamageTypeEarth { get; private set; }
        [field: SerializeField] public Sprite DamageTypeFire { get; private set; }
        [field: SerializeField] public Sprite DamageTypeIce { get; private set; }
        [field: SerializeField] public Sprite DamageTypeLightning { get; private set; }
        [field: SerializeField] public Sprite DamageTypePoison { get; private set; }
        [field: SerializeField] public Sprite DamageTypeNone { get; private set; }

        [Header("Card Variable References")]
        [field: SerializeField] public Sprite EmptyDot { get; private set; }
        [field: SerializeField] public Sprite FilledDot { get; private set; }

        //Cards TSV file headers, do not alter if unsure
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

        public const int MaxCardsInHand = 5;
        public const int MaxCardsInDeck = 20;
        private static List<CardData> _playerHand;
        public static List<CardData> PlayerHand
        {
            get
            {
                if (_playerHand != null) return _playerHand;
                InitializePlayerHand();
                return _playerHand;
            }

            private set
            {
                _playerHand = value;
            }
        }
        public static List<string> ActiveDeck { get; private set; }
        public static List<string> UnlockedCards { get; private set; }
        public static Dictionary<string, CardData> Cards { get; private set; }
        public static List<string> CardIds { get; private set; }
        public readonly string[] DefaultCards = 
        {
            "apple",
            "bonfire",
            "banter",
            "lockpick"
        };

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            AssignCardDataFromTsv(cardTsvAsset.text);
            
            foreach(var keyValuePair in Cards)
            {
                Debug.Log($"{keyValuePair.Value.name} - {keyValuePair.Value.description}");
            }

            UnlockedCards = DefaultCards.ToList();
            ActiveDeck = UnlockedCards;
        }

        public static void UnlockCard(string cardId)
        {
            if (!Cards.ContainsKey(cardId))
            {
                Debug.LogWarning($"Tried to unlock a card with a non-existent id: {cardId}");
                return;
            }
            else if (UnlockedCards.Contains(cardId))
            {
                Debug.Log($"Tried to unlock an already-unlocked card: {cardId}");
                return;
            }
            UnlockedCards.Add(cardId);
        }

        public static void InitializePlayerHand()
        {
            PlayerHand = new();

            if (ActiveDeck.Count <= MaxCardsInHand)
            {
                ActiveDeck.ForEach(card => PlayerHand.Add(Cards[card]));
                return;
            }

            while (PlayerHand.Count < MaxCardsInHand)
            {
                string idToAdd = CardIds[UnityEngine.Random.Range(0, CardIds.Count)];
                PlayerHand.Add(Cards[idToAdd]);
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
            CardIds = new();

            for (int i = 1; i < cardDataAsStrings.Length; i++)
            {
                string[] cardValues = cardDataAsStrings[i].Split('\t');
                string cardId = cardValues[idIndex].Replace("#", null);
                if (!Enum.TryParse<CardData.SpellSchool>(cardValues[schoolIndex], ignoreCase: true, out var cardSchool)) cardSchool = CardData.SpellSchool.none;
                if (!Enum.TryParse<CardData.DamageType>(cardValues[damageIndex], ignoreCase: true, out var cardDamage)) cardDamage = CardData.DamageType.none;
                if (!Enum.TryParse<CardData.Strength>(cardValues[strengthIndex], ignoreCase: true, out var cardStrength)) cardStrength = CardData.Strength.none;
                if (!Enum.TryParse<CardData.Range>(cardValues[rangeIndex], ignoreCase: true, out var cardRange)) cardRange = CardData.Range.touch;
                if (!Enum.TryParse<CardData.AreaOfEffect>(cardValues[aoeIndex], ignoreCase: true, out var cardAoe)) cardAoe = CardData.AreaOfEffect.none;
                string[] cardTagArray;
                if (cardValues[tagsIndex] != "" && cardValues[tagsIndex] != null)
                {
                    cardTagArray = cardValues[tagsIndex].Split(',');
                    for (int j = 0; j < cardTagArray.Length; j++)
                    {
                        cardTagArray[j] = cardTagArray[j].Replace("#", null);
                        cardTagArray[j] = cardTagArray[j].Replace(" ", null);
                    }
                }
                else cardTagArray = null;
                List<string> cardTags = cardTagArray == null ? new List<string>() : cardTagArray.ToList();

                Cards.Add(cardId, new(index: int.Parse(cardValues[indexIndex]) , name: cardValues[nameIndex],
                    id: cardId, description: cardValues[descriptionIndex],
                    cardType: Enum.Parse<CardData.CardType>(cardValues[typeIndex], ignoreCase: true),
                    spellSchool: cardSchool, damageType: cardDamage, strength: cardStrength, range: cardRange,
                    areaOfEffect: cardAoe, otherTags: cardTags));
                CardIds.Add(cardId);
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