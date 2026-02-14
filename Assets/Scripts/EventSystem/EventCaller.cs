using UnityEngine;

public class EventCaller : MonoBehaviour
{
    [SerializeField] private TextAsset eventAsset;

    private void Start()
    {
        SceneTransitionManager.OnSceneTransitionEnd += CallEventStart;
    }

    private void CallEventStart()
    {
        EventManager.Instance.EnterEvent(eventAsset);
        SceneTransitionManager.OnSceneTransitionEnd -= CallEventStart;
    }
}
