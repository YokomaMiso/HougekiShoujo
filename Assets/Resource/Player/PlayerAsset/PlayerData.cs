using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Create/PlayerData/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [Header("Character Name")]
    [SerializeField, Header("Character Name")] string charaName;
    [Header("Animation")]
    [SerializeField, Header("Anim Data")] CharacterAnimData characterAnimData;
    [Header("Status Data")]
    [SerializeField, Header("Move Speed")] float moveSpeed = 5;
    [Header("Shell Data")]
    [SerializeField, Header("Shell")] Shell shell;
    [Header("Weapon Data")]
    [SerializeField, Header("Sub Weapon")] SubWeapon subWeapon;

    [Header("SFX")]
    [SerializeField, Header("Player SFX Data")] PlayerSFXData playerSFXData;

    public string GetCharaName() { return charaName; }
    public CharacterAnimData GetCharacterAnimData() {  return characterAnimData; }

    public float GetMoveSpeed() {  return moveSpeed; }

    public Shell GetShell() { return shell; }
    public SubWeapon GetSubWeapon() { return subWeapon; }

    public PlayerSFXData GetPlayerSFXData() { return playerSFXData; }
}
