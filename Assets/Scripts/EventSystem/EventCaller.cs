using UnityEngine;

public class EventCaller : MonoBehaviour
{
    [SerializeField] private TextAsset eventAsset;
    [SerializeField] private string[] targetsToActivateOnStart;

    private void Start()
    {
        SceneTransitionManager.OnSceneTransitionEnd += CallEventStart;

        if (targetsToActivateOnStart == null) return;

        foreach (string targetName in targetsToActivateOnStart)
        {
            EventCardTargetManager.EventCardTargets[targetName].SetActive(true);
        }
    }

    private void CallEventStart()
    {
        EventManager.Instance.EnterEvent(eventAsset);
        SceneTransitionManager.OnSceneTransitionEnd -= CallEventStart;
    }
}
