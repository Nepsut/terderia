using System.Collections;
using CardSystem;
using UnityEngine;


/// <summary>
/// ALL FUNCTIONS IN THIS CLASS MUST BE NAMED EXACTLY THE SAME AS THE INK TAG
/// THAT IS USED TO CALL THEM, OTHERWISE EVERYTHING BREAKS AND I EXPLODE.
/// </summary>
public class EventFunctions : MonoBehaviour
{
    private EventVariables eventVariables => EventManager.Instance.EventVariables;
    private EventManager eventManager => EventManager.Instance;

    //Misc setting variables
    private const float eventEndTimeSeconds = 2f;

    public void TestFunction()
    {
        Debug.Log("Test function was called!");
        Debug.Log($"State of test variable: {eventManager.GetVariableState("g_test_seen")}");
    }

    //TEMPORARY, REMOVE LATER
    public void UnlockCabinCards()
    {
        CardManager.UnlockCard("punch", addToDeck: true);
        CardManager.UnlockCard("insult", addToDeck: true);
        CardManager.UnlockCard("static-shock", addToDeck: true);
        CardManager.UnlockCard("rope", addToDeck: true);
    }

    public void LoadMap1Scene()
    {
        StartCoroutine(MapLoadDelayHandler(SceneTransitionManager.Scene.Map1));
    }

    private IEnumerator MapLoadDelayHandler(SceneTransitionManager.Scene sceneToLoad)
    {
        yield return new WaitForSeconds(eventEndTimeSeconds);
        SceneTransitionManager.Instance.StartTransition(sceneToLoad);
    }
}
