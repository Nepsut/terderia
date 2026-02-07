using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private InputReader inputReader;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputReader.EnableUiInputs();
    }

    private void OnApplicationQuit()
    {
        inputReader.DisableUiInputs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
