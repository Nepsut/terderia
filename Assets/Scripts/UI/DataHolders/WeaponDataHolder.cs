using UnityEngine;

public class WeaponDataHolder : MonoBehaviour
{
    [field: SerializeField] public PlayerData.StartingWeapon StartingWeapon { get; private set; }
}
