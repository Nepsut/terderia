using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class HeartHolder : MonoBehaviour
{
    [SerializeField] private AnimationClip heartSwingClip;
    [SerializeField] private AnimationClip heartGrowClip;
    [SerializeField] private AnimationClip heartBreakClip;
    private RectTransform selfRect;
    private Vector2 homePosition;
    private int Health => GameManager.Instance.playerHealth;
    private int MaxHealth => GameManager.Instance.playerHealthMax;
    private int activeHearts;
    private const float branchShakeFrequency = 0.1f;
    private const float branchShakeDuration = 0.4f;
    private const float branchShakeXOffset = -20f;
    private const float branchShakeYOffset = -10f;
    private Coroutine branchShakeCoroutine;
    private int branchShakeTween = -1;
    private const float heartDisableDelay = 0.1f;
    private bool heartAnimsInProgress = false;
    [SerializeField] private Image[] heartImages;
    [SerializeField] private Animator[] heartAnimators;

    public static event Action OnHealthAnimationDone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selfRect = GetComponent<RectTransform>();
        homePosition = selfRect.anchoredPosition;
        GameManager.OnPlayerHealthLoss += _ => ShakeBranch();
        GameManager.OnPlayerHealthLoss += HandleHealthLoss;
        GameManager.OnPlayerHealthGain += HandleHealthGain;
        activeHearts = heartAnimators.Length;
    }

    private void ShakeBranch()
    {
        if (heartAnimsInProgress) return;
        if (branchShakeCoroutine != null) StopCoroutine(branchShakeCoroutine);
        StartCoroutine(BranchShaker());
    }

    private IEnumerator BranchShaker()
    {
        Vector2 shakeAmount = new(branchShakeXOffset, branchShakeYOffset);
        branchShakeTween = LeanTween.move(selfRect, selfRect.anchoredPosition + shakeAmount, branchShakeFrequency)
                                    .setEaseInOutElastic()
                                    .setLoopPingPong().id;
        yield return new WaitForSeconds(branchShakeDuration);
        LeanTween.cancel(branchShakeTween);
        selfRect.anchoredPosition = homePosition;
        branchShakeTween = -1;
        branchShakeCoroutine = null;
    }

    private void HandleHealthLoss(int lostHealth)
    {
        if (Health == MaxHealth || heartAnimsInProgress) return;
        if (lostHealth > activeHearts) lostHealth = activeHearts;
        heartAnimsInProgress = true;

        for (int i = 0; i < activeHearts; i++)
        {
            heartAnimators[i].Play(heartSwingClip.name, -1, 0f);
        }

        StartCoroutine(HeartBreakHandler(lostHealth));
    }

    private IEnumerator HeartBreakHandler(int lostHealth)
    {
        yield return new WaitForSeconds(heartSwingClip.length);

        int firstHeartToBreak = activeHearts - lostHealth;

        for (int i = firstHeartToBreak; i < activeHearts; i++)
        {
            heartAnimators[i].Play(heartBreakClip.name, -1, 0f);
        }
        yield return new WaitForSeconds(heartBreakClip.length + heartDisableDelay);

        for (int i = firstHeartToBreak; i < activeHearts; i++)
        {
            heartImages[i].enabled = false;
        }
        activeHearts -= lostHealth;
        heartAnimsInProgress = false;
        OnHealthAnimationDone?.Invoke();
    }

    private void HandleHealthGain(int gainedHealth)
    {
        if (heartAnimsInProgress) return;
        if (gainedHealth + activeHearts > MaxHealth)
            gainedHealth = MaxHealth - activeHearts;

        for (int i = activeHearts; i < activeHearts + gainedHealth; i++)
        {
            heartImages[i].enabled = true;
            heartAnimators[i].Play(heartGrowClip.name, -1, 0f);
        }
        activeHearts += gainedHealth;
        StartCoroutine(HeartGrowHandler());
    }

    private IEnumerator HeartGrowHandler()
    {
        yield return new WaitForSeconds(heartGrowClip.length);
        heartAnimsInProgress = false;
    }
}
