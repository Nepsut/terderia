using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SpriteRenderer))]
public class MapMarker : MonoBehaviour
{
    [field: SerializeField] public List<MapNeighborContainer> NeighborMarkers { get; private set; }

    //TODO: MapEvent should be populated by MapTravelManager randomly
    [field: SerializeField] public SceneLoadObject MapEvent { get; private set; }
    private SpriteRenderer selfSpriteRenderer;
    public bool SceneIsSeen { get; private set; }
    private bool RandomSceneReceived = false;

    private void Start()
    {
        selfSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void SetRandomScene()
    {
        if (RandomSceneReceived) return;
        RandomSceneReceived = true;
    }

    public void SetAsVisited()
    {
        selfSpriteRenderer.enabled = false;
    }
}

[System.Serializable]
public class MapNeighborContainer
{
    public MapMarker neighborMarker;
    public SplineContainer splineFromNeighbor;
}