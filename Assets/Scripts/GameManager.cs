using System;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private InputReader inputReader;
    public int playerHealthMax = 5;
    public int playerHealth = 5;

    public static event Action<int> OnPlayerHealthLoss;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputReader.EnableUiInputs();
    }

    private void OnApplicationQuit()
    {
        inputReader.DisableUiInputs();
    }
    
    public void DamagePlayer(int damageAmount)
    {
        playerHealth = math.clamp(playerHealth - damageAmount, 0, playerHealthMax);
        OnPlayerHealthLoss?.Invoke(damageAmount);
    }
}
