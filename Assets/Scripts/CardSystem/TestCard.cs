using System.Linq;
using CardSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestCard : MonoBehaviour
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

    private int selfId = -1;
    private Card selfCard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selfId = Random.Range(1, CardManager.Cards.Count+1);
        selfCard = CardManager.Cards.Values.FirstOrDefault(card => card.index == selfId);
        titleText.text = selfCard.name;
        descriptionText.text = selfCard.description;
        UtilityManager.Instance.DoNextFrame(() =>
        {
            RectTransform textRect = descriptionText.GetComponent<RectTransform>();
            if (textRect.rect.width > LayoutUtility.GetPreferredWidth(textRect))
            {
                descriptionText.alignment = TextAlignmentOptions.Midline;
            }
        });
        cardImage.sprite = selfCard.sprite;
        damageText.text = selfCard.damageType.ToString().FirstCharacterToUpper();

        switch (selfCard.cardType)
        {
            case Card.CardType.speech:
                cardBack.sprite = CardManager.Instance.CardbaseSpeech;
                break;
            case Card.CardType.spell:
                cardBack.sprite = CardManager.Instance.CardbaseSpell;
                break;
            case Card.CardType.utility:
                cardBack.sprite = CardManager.Instance.CardbaseUtility;
                break;
            case Card.CardType.weapon:
                cardBack.sprite = CardManager.Instance.CardbaseWeapon;
                break;
            default:
                cardBack.gameObject.SetActive(false);
                break;
        }

        switch (selfCard.damageType)
        {
            case Card.DamageType.blunt:
                damageImage.sprite = CardManager.Instance.DamageTypeBlunt;
                break;
            case Card.DamageType.cutting:
                damageImage.sprite = CardManager.Instance.DamageTypeCutting;
                break;
            case Card.DamageType.earth:
                damageImage.sprite = CardManager.Instance.DamageTypeEarth;
                break;
            case Card.DamageType.fire:
                damageImage.sprite = CardManager.Instance.DamageTypeFire;
                break;
            case Card.DamageType.ice:
                damageImage.sprite = CardManager.Instance.DamageTypeIce;
                break;
            case Card.DamageType.lightning:
                damageImage.sprite = CardManager.Instance.DamageTypeLightning;
                break;
            case Card.DamageType.poison:
                damageImage.sprite = CardManager.Instance.DamageTypePoison;
                break;
            default:
                damageImage.gameObject.SetActive(false);
                break;
        }

        for (int i = -1; i < (int)selfCard.strength; i++)
        {
            if (i == -1) continue;
            StrDotHolder.GetChild(i).GetComponent<Image>().sprite = CardManager.Instance.FilledDot;
        }
        for (int i = -1; i < (int)selfCard.range; i++)
        {
            if (i == -1) continue;
            RangeDotHolder.GetChild(i).GetComponent<Image>().sprite = CardManager.Instance.FilledDot;
        }
        for (int i = -1; i < (int)selfCard.areaOfEffect; i++)
        {
            if (i == -1) continue;
            AoeDotHolder.GetChild(i).GetComponent<Image>().sprite = CardManager.Instance.FilledDot;
        }
    }
}
