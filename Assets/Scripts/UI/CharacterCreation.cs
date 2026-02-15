using System.Collections;
using CardSystem;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour
{
    [SerializeField] private RectTransform genderHolder, subclassHolder, weaponHolder;
    [SerializeField] private Button genderLeftButton, genderRightButton, subclassLeftButton,
        subclassRightButton, weaponLeftButton, weaponRightButton, continueButton;
    private const float scrollTime = 0.32f;
    private const float scrollOffset = 20f;
    private bool scrollingGender = false, scrollingSubclass = false, scrollingWeapon = false;

    private void ScrollGenderLeft()
    {
        RectTransform rectToMove = genderHolder.GetChild(genderHolder.childCount - 1).GetComponent<RectTransform>();
        StartCoroutine(HandleGenderScroll(rectToMove, right: false));
    }

    private void ScrollGenderRight()
    {
        RectTransform rectToMove = genderHolder.GetChild(0).GetComponent<RectTransform>();
        StartCoroutine(HandleGenderScroll(rectToMove, right: true));
    }

    private void ScrollSubclassLeft()
    {
        RectTransform rectToMove = subclassHolder.GetChild(subclassHolder.childCount - 1).GetComponent<RectTransform>();
        StartCoroutine(HandleSubclassScroll(rectToMove, right: false));
    }

    private void ScrollSubclassRight()
    {
        RectTransform rectToMove = subclassHolder.GetChild(0).GetComponent<RectTransform>();
        StartCoroutine(HandleSubclassScroll(rectToMove, right: true));
    }

    private void ScrollWeaponLeft()
    {
        RectTransform rectToMove = weaponHolder.GetChild(weaponHolder.childCount - 1).GetComponent<RectTransform>();
        StartCoroutine(HandleWeaponScroll(rectToMove, right: false));
    }

    private void ScrollWeaponRight()
    {
        RectTransform rectToMove = weaponHolder.GetChild(0).GetComponent<RectTransform>();
        StartCoroutine(HandleWeaponScroll(rectToMove, right: true));
    }

    private void HandleContinue()
    {
        PlayerData playerData = new()
        {
            gender = genderHolder.GetChild(genderHolder.childCount - 1).GetComponent<GenderDataHolder>().Gender,
            subclass = subclassHolder.GetChild(subclassHolder.childCount - 1).GetComponent<SubclassDataHolder>().Subclass,
            startingWeapon = weaponHolder.GetChild(weaponHolder.childCount - 1).GetComponent<WeaponDataHolder>().StartingWeapon
        };

        CardManager.UnlockCard(playerData.StartingWeaponAsString);
        GameManager.Instance.playerData = playerData;
    }

    private IEnumerator HandleGenderScroll(RectTransform rectToMove, bool right)
    {
        genderLeftButton.interactable = false;
        genderRightButton.interactable = false;
        continueButton.interactable = false;
        scrollingGender = true;
        WaitForSeconds waitTime = new(scrollTime);
        float oldX = rectToMove.anchoredPosition.x;
        float newX = right ? rectToMove.anchoredPosition.x + rectToMove.rect.width + scrollOffset :
            rectToMove.anchoredPosition.x - rectToMove.rect.width - scrollOffset;
        LeanTween.moveX(rectToMove, newX, scrollTime).setEaseOutQuart();
        yield return waitTime;
        if (right) rectToMove.SetAsLastSibling();
        else rectToMove.SetAsFirstSibling();
        LeanTween.moveX(rectToMove, oldX, scrollTime).setEaseOutQuart();
        yield return waitTime;
        genderLeftButton.interactable = true;
        genderRightButton.interactable = true;
        scrollingGender = false;
        if (!scrollingSubclass && !scrollingWeapon) continueButton.interactable = true;
    }

    private IEnumerator HandleSubclassScroll(RectTransform rectToMove, bool right)
    {
        subclassLeftButton.interactable = false;
        subclassRightButton.interactable = false;
        continueButton.interactable = false;
        scrollingSubclass = true;
        WaitForSeconds waitTime = new(scrollTime);
        float oldX = rectToMove.anchoredPosition.x;
        float newX = right ? rectToMove.anchoredPosition.x + rectToMove.rect.width + scrollOffset :
            rectToMove.anchoredPosition.x - rectToMove.rect.width - scrollOffset;
        LeanTween.moveX(rectToMove, newX, scrollTime).setEaseOutQuart();
        yield return waitTime;
        if (right) rectToMove.SetAsLastSibling();
        else rectToMove.SetAsFirstSibling();
        LeanTween.moveX(rectToMove, oldX, scrollTime).setEaseOutQuart();
        yield return waitTime;
        subclassLeftButton.interactable = true;
        subclassRightButton.interactable = true;
        scrollingSubclass = false;
        if (!scrollingGender && !scrollingWeapon) continueButton.interactable = true;
    }

    private IEnumerator HandleWeaponScroll(RectTransform rectToMove, bool right)
    {
        weaponLeftButton.interactable = false;
        weaponRightButton.interactable = false;
        continueButton.interactable = false;
        scrollingWeapon = true;
        WaitForSeconds waitTime = new(scrollTime);
        float oldX = rectToMove.anchoredPosition.x;
        float newX = right ? rectToMove.anchoredPosition.x + rectToMove.rect.width + scrollOffset :
            rectToMove.anchoredPosition.x - rectToMove.rect.width - scrollOffset;
        LeanTween.moveX(rectToMove, newX, scrollTime).setEaseOutQuart();
        yield return waitTime;
        if (right) rectToMove.SetAsLastSibling();
        else rectToMove.SetAsFirstSibling();
        LeanTween.moveX(rectToMove, oldX, scrollTime).setEaseOutQuart();
        yield return waitTime;
        weaponLeftButton.interactable = true;
        weaponRightButton.interactable = true;
        scrollingWeapon = false;
        if (!scrollingSubclass && !scrollingGender) continueButton.interactable = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        genderLeftButton.onClick.AddListener(ScrollGenderLeft);
        genderRightButton.onClick.AddListener(ScrollGenderRight);
        subclassLeftButton.onClick.AddListener(ScrollSubclassLeft);
        subclassRightButton.onClick.AddListener(ScrollSubclassRight);
        weaponLeftButton.onClick.AddListener(ScrollWeaponLeft);
        weaponRightButton.onClick.AddListener(ScrollWeaponRight);
        continueButton.onClick.AddListener(HandleContinue);
    }
}
