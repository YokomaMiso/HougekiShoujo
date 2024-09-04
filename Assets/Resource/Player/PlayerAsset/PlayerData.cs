using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CHARACTER_ID { MAEDE = 0, AAA, III, UUU, EEE, MAX_NUM };

[CreateAssetMenu(fileName = "PlayerData", menuName = "Create/PlayerData/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [Header("Character ID")]
    [SerializeField, Header("Status Array")] CHARACTER_ID id;

    [Header("Status Data")]
    [SerializeField, Header("Move Speed")] float moveSpeed;

    [Header("Shell Data")]
    [SerializeField, Header("Shell")] Shell shell;

    [Header("Weapon Data")]
    [SerializeField, Header("Sub Weapon")] Weapon subWeapon;

    public int GetID() { return (int)id; }

    public Shell GetShell() { return shell; }
    public Weapon GetSubWeapon() { return subWeapon; }

}
