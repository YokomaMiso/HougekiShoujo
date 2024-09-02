using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Create/PlayerData/WeaponData", order = 1)]
public class Weapon : ScriptableObject
{
    [SerializeField, Header("Weapon Graphic")] Sprite weaponGraphic;
}
