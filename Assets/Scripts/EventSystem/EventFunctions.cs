using UnityEngine;


/// <summary>
/// ALL FUNCTIONS IN THIS CLASS MUST BE NAMED EXACTLY THE SAME AS THE INK TAG
/// THAT IS USED TO CALL THEM, OTHERWISE EVERYTHING BREAKS AND I EXPLODE.
/// </summary>
public class EventFunctions : MonoBehaviour
{
    private EventVariables eventVariables => EventManager.Instance.EventVariables;
    private EventManager eventManager => EventManager.Instance;

    public void TestFunction()
    {
        Debug.Log("Test function was called!");
        Debug.Log($"State of test variable: {eventManager.GetVariableState("g_test_seen")}");
    }
}
