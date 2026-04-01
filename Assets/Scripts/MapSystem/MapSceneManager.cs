using UnityEngine;

public class MapSceneManager : MonoBehaviour
{
    [SerializeField] private AudioClip mapMusic;
    public const string EventCanvasTag = "EventCanvas";
    private GameObject eventCanvas;

    private void Start()
    {
        eventCanvas = GameObject.FindWithTag(EventCanvasTag);
        if (eventCanvas != null) eventCanvas.SetActive(false);
        AudioManager.Instance.ChangeMusic(mapMusic);
    }

    private void OnDestroy()
    {
        if (eventCanvas != null) eventCanvas.SetActive(true);
    }
}
