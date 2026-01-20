using System.Linq;
using CardSystem;
using TMPro;
using UnityEngine;

public class TestCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private int selfId = -1;
    private Card selfCard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selfId = Random.Range(1, CardManager.Cards.Count+1);
        selfCard = CardManager.Cards.Values.FirstOrDefault(card => card.index == selfId);
        titleText.text = selfCard.name;
        descriptionText.text = selfCard.description;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
