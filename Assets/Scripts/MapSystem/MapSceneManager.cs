using UnityEngine;

public class MapSceneManager : MonoBehaviour
{
    public const string EventCanvasTag = "EventCanvas";
    private GameObject eventCanvas;

    private void Start()
    {
        eventCanvas = GameObject.FindWithTag(EventCanvasTag);
        if (eventCanvas != null) eventCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        if (eventCanvas != null) eventCanvas.SetActive(true);
    }
}
