using UnityEngine;

public class EventCaller : MonoBehaviour
{
    [SerializeField] private TextAsset eventAsset;
    [Tooltip("Leave null to not change music for event")]
    [SerializeField] private AudioClip eventMusic;
    [SerializeField] private string[] targetsToActivateOnStart;

    private void Start()
    {
        SceneTransitionManager.OnSceneTransitionEnd += CallEventStart;

        if (eventMusic != null) AudioManager.Instance.ChangeMusic(eventMusic);

        if (targetsToActivateOnStart != null)
        {
            foreach (string targetName in targetsToActivateOnStart)
            {
                EventCardTargetManager.EventCardTargets[targetName].SetActive(true);
            }
        }
    }

    private void CallEventStart()
    {
        EventManager.Instance.EnterEvent(eventAsset);
        SceneTransitionManager.OnSceneTransitionEnd -= CallEventStart;
    }
}
