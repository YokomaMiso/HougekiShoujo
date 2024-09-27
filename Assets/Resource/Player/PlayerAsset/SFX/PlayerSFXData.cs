using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSFXData", menuName = "Create/PlayerData/PlayerSFXData", order = 1)]

public class PlayerSFXData : ScriptableObject
{
    [Header("SFX")]
    [SerializeField, Header("Reload SFX")] AudioClip reloadSFX;

    public AudioClip GetReloadSFX() { return reloadSFX; }
}
