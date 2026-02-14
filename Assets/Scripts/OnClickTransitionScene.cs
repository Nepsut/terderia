using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OnClickTransitionScene : MonoBehaviour
{
    [SerializeField] private SceneTransitionManager.Scene scene;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneTransitionManager.Instance.StartTransition(scene));
    }
}
