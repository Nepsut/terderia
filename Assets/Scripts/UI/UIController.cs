using UnityEngine;

public class UIController : MonoSingleton<UIController>
{
    [SerializeField] private InputReader inputReader;
    public InputReader MainInputReader => inputReader;
    private static Vector2 _mousePos;
    public static Vector2 MousePosition => _mousePos;
    public static bool IsMenuOpen { get; private set; } = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputReader.OnPointEvent += pos => _mousePos = pos;
    }
}