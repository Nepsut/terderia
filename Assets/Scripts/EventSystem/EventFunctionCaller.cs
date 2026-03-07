using System.Collections;
using System.Collections.Generic;
using CardSystem;
using UnityEngine;


/// <summary>
/// ALL FUNCTIONS IN THIS CLASS MUST BE NAMED EXACTLY THE SAME AS THE INK TAG
/// THAT IS USED TO CALL THEM, OTHERWISE EVERYTHING BREAKS AND I EXPLODE.
/// </summary>
public class EventFunctionCaller : MonoBehaviour
{
    //Misc setting variables
    private const float eventEndTimeSeconds = 0.5f;

    private EventVariables eventVariables => EventManager.Instance.EventVariables;
    private EventManager eventManager => EventManager.Instance;

    public Dictionary<string, EventFunction> EventFunctionDict { get; private set; } = new()
    {
        {nameof(RewardCards), new RewardCards()},
        {nameof(LoadScene), new LoadScene()}
    };

    public bool FunctionExists(string funcName)
    {
        return EventFunctionDict.ContainsKey(funcName);
    }

    public bool TryCallEventFunction(string funcName, object[] args = null)
    {
        if (!EventFunctionDict.ContainsKey(funcName)) return false;

        return EventFunctionDict[funcName].TryExecute(args);
    }

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
        switch (GameManager.Instance.playerData.subclass)
        {
            case PlayerData.Subclass.alchemist:
                CardManager.UnlockCard("evil-bottle", addToDeck: true);
                break;
            case PlayerData.Subclass.trickster:
                CardManager.UnlockCard("smokescreen", addToDeck: true);
                break;
            case PlayerData.Subclass.chef:
                CardManager.UnlockCard("can-of-beans", addToDeck: true);
                break;
            case PlayerData.Subclass.elementalist:
                CardManager.UnlockCard("snowball", addToDeck: true);
                break;
        }
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
