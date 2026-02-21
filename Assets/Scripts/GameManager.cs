using System;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private InputReader inputReader;
    public PlayerData playerData;
    public static int playerHealthMax = 5;
    public static int playerHealth = 5;

    [field: SerializeField] public bool DebugModeOn { get; private set; } = false;

    public static event Action<int> OnPlayerHealthLoss;
    public static event Action<int> OnPlayerHealthGain;

    private void Awake()
    {
        EventVariables.OnHealthLost += DamagePlayer;
        EventVariables.OnHealthGained += HealPlayer;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputReader.EnableUiInputs();
    }

    private void OnApplicationQuit()
    {
        inputReader.DisableUiInputs();
    }
    
    public static void DamagePlayer(int damageAmount)
    {
        if (damageAmount < 1)
        {
            Debug.LogWarning($"DamagePlayer should not be called with values below zero!");
        }
        playerHealth = math.clamp(playerHealth - damageAmount, 0, playerHealthMax);
        OnPlayerHealthLoss?.Invoke(damageAmount);
    }
    
    public static void HealPlayer(int healAmount)
    {
        if (healAmount < 1)
        {
            Debug.LogWarning($"HealPlayer should not be called with values below zero!");
        }
        playerHealth = math.clamp(playerHealth + healAmount, 0, playerHealthMax);
        OnPlayerHealthGain?.Invoke(healAmount);
    }
}
