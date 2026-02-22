using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class MapTravelManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private SplineAnimate playerSplineAnimate;
    [SerializeField] private MapMarker startingMarker;
    [SerializeField] private GameObject mapObject;
    [SerializeField] private SceneTransitionManager.Scene selfScene;
    private SplineContainer activeSpline = null;
    private MapMarker currentMarker = null;
    private MapMarker travelingToMarker = null;
    private Vector2 MousePos => UIController.MousePosition;
    private static MapTravelManager firstCopy;

    //Map movement logic variables
    public bool TravelEnabled { get; private set; } = false;
    public bool PlayerMoving { get; private set; } = false;

    private void Awake()
    {
        if (firstCopy == null)
            firstCopy = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentMarker = startingMarker;
        playerSplineAnimate.transform.position = currentMarker.transform.position;
        inputReader.OnClickEvent += CheckIfShouldTravel;
        SceneTransitionManager.OnSceneLoadFinished += ResetMapState;
        SceneTransitionManager.OnSceneLoadStarted += HidePersistentMapItems;
        inputReader.EnableUiInputs();

        //TEMPORARY
        TravelEnabled = true;
    }

    private void ResetMapState(SceneTransitionManager.Scene loadedScene)
    {
        if (loadedScene != selfScene) return;
        mapObject.SetActive(true);
        playerSplineAnimate.gameObject.SetActive(true);
    }

    private void HidePersistentMapItems()
    {
        mapObject.SetActive(false);
        playerSplineAnimate.gameObject.SetActive(false);
    }

    private void CheckIfShouldTravel()
    {
        if (!TravelEnabled || PlayerMoving || UIController.IsMenuOpen) return;

        Debug.Log("Checking if travel should start");
        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(MousePos), Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log("Processing hit");
            if (hit.collider.TryGetComponent(out MapMarker hitMarker))
            {
                Debug.Log("Starting travel");
                HandleTravelStart(hitMarker);
                break;
            }
        }
    }

    private bool TryGetSplineToNeighbor(MapMarker neighborMarker, out SplineContainer container)
    {
        bool markerWasNeighbor = false;
        container = null;
        foreach (MapNeighborContainer neighbor in neighborMarker.NeighborMarkers)
        {
            if (neighbor.neighborMarker == currentMarker)
            {
                markerWasNeighbor = true;
                container = neighbor.splineFromNeighbor;
                break;
            }
        }
        return markerWasNeighbor;
    }

    private void HandleTravelStart(MapMarker destinationMarker)
    {
        if (!TryGetSplineToNeighbor(destinationMarker, out activeSpline))
        {
            if (GameManager.Instance.DebugModeOn)
                Debug.Log("Could not find path from current marker to clicked marker");
            return;
        }
        travelingToMarker = destinationMarker;
        playerSplineAnimate.Container = activeSpline;
        playerSplineAnimate.Completed += HandleTravelDone;
        playerSplineAnimate.Restart(true);
        // playerSplineAnimate.Play();
        PlayerMoving = true;
    }

    private void HandleTravelDone()
    {
        Debug.Log("Travel done");
        PlayerMoving = false;
        currentMarker = travelingToMarker;
        travelingToMarker = null;
        playerSplineAnimate.Completed -= HandleTravelDone;
        if (currentMarker.MapEvent != null) currentMarker.MapEvent.LoadSetScene();
    }
}