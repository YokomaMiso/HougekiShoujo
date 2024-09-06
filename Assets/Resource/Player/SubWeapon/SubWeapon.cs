using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubWeaponData", menuName = "Create/PlayerData/SubWeaponData", order = 1)]

public class SubWeapon : ScriptableObject
{
    [SerializeField, Header("SubWeapon Icon")] Sprite subWeaponIcon;

    public Sprite GetIcon() { return subWeaponIcon; }
}
