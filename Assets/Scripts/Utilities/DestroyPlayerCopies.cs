using UnityEngine;

public class DestroyPlayerCopies : MonoBehaviour
{
    private static DestroyPlayerCopies firstCopy;
    
    private void Awake()
    {
        if (firstCopy == null) firstCopy = this;
        else Destroy(gameObject);
    }
}