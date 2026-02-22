using UnityEngine;

[CreateAssetMenu(fileName = "SceneLoadObject", menuName = "Scriptable Objects/SceneLoadObject")]
public class SceneLoadObject : ScriptableObject
{
    [field: SerializeField] public SceneTransitionManager.Scene SceneToLoad { get; private set; }

    public void LoadSetScene()
    {
        SceneTransitionManager.Instance.StartTransition(SceneToLoad);
    }
}
