using System;
using System.Collections;

public class UtilityManager : MonoSingleton<UtilityManager>
{
    public void DoNextFrame(Action action)
    {
        if (action == null) return;
        StartCoroutine(DoNextFrameCoroutine(action));
    }
    
    private IEnumerator DoNextFrameCoroutine(Action action)
    {
        yield return null;
        action?.Invoke();
    }
}
