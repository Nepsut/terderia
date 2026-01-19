using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoSingleton<UIController>
{
    [SerializeField] private InputActionReference mousePosInput;
    private static Vector2 _mousePos;
    public static Vector2 MousePosition => _mousePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mousePosInput.action.performed += context => _mousePos = context.ReadValue<Vector2>();
    }
}

