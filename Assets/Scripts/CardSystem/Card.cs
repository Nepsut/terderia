using System;
using System.Linq;
using CardSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DraggableObject))]
public class Card : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private Image cardBack;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image damageImage;
    [SerializeField] private Transform StrDotHolder;
    [SerializeField] private Transform RangeDotHolder;
    [SerializeField] private Transform AoeDotHolder;

    private DraggableObject selfDraggable;
    public CardData CardData { get; private set; }

    public static event Action<Card> OnCardDragStart;
    public static event Action<Card> OnCardDragEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selfDraggable = GetComponent<DraggableObject>();
        selfDraggable.OnDragStart += _ => OnCardDragStart?.Invoke(this);
        selfDraggable.OnDragEnd += _ => OnCardDragEnd?.Invoke(this);

        InitializeCard(UnityEngine.Random.Range(1, CardManager.Cards.Count+1));
    }

    public void InitializeCard(string uniqueTag)
    {
        if (!CardManager.Cards.ContainsKey(uniqueTag))
        {
            Debug.LogWarning($"Tried to initialize card with flawed tag: {uniqueTag}");
        }
        CardData = CardManager.Cards[uniqueTag];
        SetAppearance();
    }

    public void InitializeCard(int cardIndex)
    {
        CardData = CardManager.Cards.Values.FirstOrDefault(card => card.index == cardIndex);
        SetAppearance();
    }

    private void SetAppearance()
    {
        titleText.text = CardData.name;
        descriptionText.text = CardData.description;
        UtilityManager.Instance.DoNextFrame(() =>
        {
            RectTransform textRect = descriptionText.GetComponent<RectTransform>();
            if (textRect.rect.width > LayoutUtility.GetPreferredWidth(textRect))
            {
                descriptionText.alignment = TextAlignmentOptions.Midline;
            }
        });
        cardImage.sprite = CardData.sprite;
        damageText.text = CardData.damageType.ToString().FirstCharacterToUpper();

        cardBack.sprite = CardData.cardType switch
        {
            CardData.CardType.speech => CardManager.Instance.CardbaseSpeech,
            CardData.CardType.spell => CardManager.Instance.CardbaseSpell,
            CardData.CardType.utility => CardManager.Instance.CardbaseUtility,
            CardData.CardType.weapon => CardManager.Instance.CardbaseWeapon,
            _ => CardManager.Instance.CardbaseNull,
        };

        damageImage.sprite = CardData.damageType switch
        {
            CardData.DamageType.blunt => CardManager.Instance.DamageTypeBlunt,
            CardData.DamageType.cutting => CardManager.Instance.DamageTypeCutting,
            CardData.DamageType.earth => CardManager.Instance.DamageTypeEarth,
            CardData.DamageType.fire => CardManager.Instance.DamageTypeFire,
            CardData.DamageType.ice => CardManager.Instance.DamageTypeIce,
            CardData.DamageType.lightning => CardManager.Instance.DamageTypeLightning,
            CardData.DamageType.poison => CardManager.Instance.DamageTypePoison,
            _ => CardManager.Instance.DamageTypeNone,
        };

        for (int i = -1; i < (int)CardData.strength; i++)
        {
            if (i == -1) continue;
            StrDotHolder.GetChild(i).GetComponent<Image>().sprite = CardManager.Instance.FilledDot;
        }
        for (int i = -1; i < (int)CardData.range; i++)
        {
            if (i == -1) continue;
            RangeDotHolder.GetChild(i).GetComponent<Image>().sprite = CardManager.Instance.FilledDot;
        }
        for (int i = -1; i < (int)CardData.areaOfEffect; i++)
        {
            if (i == -1) continue;
            AoeDotHolder.GetChild(i).GetComponent<Image>().sprite = CardManager.Instance.FilledDot;
        }
    }
}