using UnityEngine;

public class SubclassDataHolder : MonoBehaviour
{
    [field: SerializeField] public PlayerData.Subclass Subclass { get; private set; }
}
