using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MapMarker : MonoBehaviour
{
    [field: SerializeField] public List<MapNeighborContainer> NeighborMarkers { get; private set; }

    //TODO: MapEvent should be populated by MapTravelManager randomly
    [field: SerializeField] public SceneLoadObject MapEvent { get; private set; }
    public bool SceneIsSeen { get; private set; }
    private bool RandomSceneReceived = false;
    
    public void SetRandomScene()
    {
        if (RandomSceneReceived) return;
        RandomSceneReceived = true;
    }
}

[System.Serializable]
public class MapNeighborContainer
{
    public MapMarker neighborMarker;
    public SplineContainer splineFromNeighbor;
}