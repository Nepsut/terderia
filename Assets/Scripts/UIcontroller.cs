using UnityEngine;
using UnityEngine.Rendering;

public class UIController : MonoBehaviour
{
    [SerializeField] private float duration = 1.0f;
    [SerializeField] Transform targetPosRight;

    Camera mainCamera = Camera.main;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void MoveCamRight()
    {
        transform.LeanMoveX(targetPosRight.position.x, duration);
    }
}

