using UnityEngine;

public class GenderDataHolder : MonoBehaviour
{
    [field: SerializeField] public PlayerData.Gender Gender { get; private set; }
}
