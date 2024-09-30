using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSFXData", menuName = "Create/PlayerData/PlayerSFXData", order = 1)]

public class PlayerSFXData : ScriptableObject
{
    [Header("Shell SFX")]
    [SerializeField, Header("Reload SFX")] AudioClip reloadSFX;
    [SerializeField, Header("Aim SFX")] AudioClip aimSFX;
    [SerializeField, Header("Fire SFX")] AudioClip fireSFX;
    [SerializeField, Header("Fly SFX")] AudioClip flySFX;
    [SerializeField, Header("Explosion SFX")] AudioClip explosionSFX;

    public AudioClip GetReloadSFX() { return reloadSFX; }
    public AudioClip GetAimSFX() { return aimSFX; }
    public AudioClip GetFireSFX() { return fireSFX; }
    public AudioClip GetFlySFX() { return flySFX; }
    public AudioClip GetExplosionSFX() { return explosionSFX; }
}
