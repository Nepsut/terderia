using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, InputMap.IUIActions
{
    private InputMap inputMap;

    public bool InputsEnabled => inputMap.UI.enabled;

    private void OnEnable()
    {
        if (inputMap == null)
        {
            inputMap = new();
            inputMap.UI.AddCallbacks(this);
            inputMap.UI.Disable();
        }
    }

    private void OnDisable()
    {
        inputMap.UI.Disable();
    }

    public void EnableUiInputs()
    {
        inputMap?.UI.Enable();
    }

    public void DisableUiInputs()
    {
        inputMap?.UI.Disable();
    }

    public event Action OnCancelEvent;
    public event Action OnClickEvent;
    public event Action OnClickHoldEvent;
    public event Action OnClickReleaseEvent;
    public event Action OnMiddleClickEvent;
    public event Action OnRightClickEvent;
    public event Action OnRightClickReleaseEvent;
    public event Action<Vector2> OnNavigateEvent;
    public event Action<Vector2> OnPointEvent;
    public event Action<Vector2> OnScrollEvent;
    public event Action OnSubmitEvent;
    public event Action OnSubmitReleaseEvent;

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnCancelEvent?.Invoke();
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnClickEvent?.Invoke();
        }
        else if (context.phase == InputActionPhase.Performed)
        {
            OnClickHoldEvent?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            OnClickReleaseEvent?.Invoke();
        }
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnMiddleClickEvent?.Invoke();
        }
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        OnNavigateEvent?.Invoke(obj: context.ReadValue<Vector2>());
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        OnPointEvent?.Invoke(obj: context.ReadValue<Vector2>());
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnRightClickEvent?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            OnRightClickReleaseEvent?.Invoke();
        }
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        OnScrollEvent?.Invoke(obj: context.ReadValue<Vector2>());
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnSubmitEvent?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            OnSubmitReleaseEvent?.Invoke();
        }
    }
}
