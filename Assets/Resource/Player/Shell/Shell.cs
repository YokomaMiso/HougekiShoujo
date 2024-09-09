using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SHELL_TYPE { BLAST = 0, CANON, MORTAR };

[CreateAssetMenu(fileName = "ShellData", menuName = "Create/PlayerData/ShellData", order = 1)]
public class Shell : ScriptableObject
{
    [SerializeField, Header("Shell Icon")] Sprite shellIcon;
    [SerializeField, Header("Shell Type")] SHELL_TYPE shellType;
    [SerializeField, Header("Projectile")] GameObject projectile;
    [SerializeField, Header("Explosion")] GameObject explosion;
    [SerializeField, Header("Explain"), TextArea(1, 3)] string shellExplain;

    [SerializeField, Header("Reload Time")] float reloadTime;
    [SerializeField, Header("Recoil Time")] float recoilTime;
    [SerializeField, Header("Aim Range")] float aimRange;

    public Sprite GetShellIcon() { return shellIcon; }
    public SHELL_TYPE GetShellType() { return shellType; }
    public GameObject GetProjectile() {  return projectile; }
    public GameObject GetExplosion() {  return explosion; }
    public string GetShellExplain() {  return shellExplain; }

    public float GetReloadTime() {  return reloadTime; }
    public float GetRecoilTime() {  return recoilTime; }
    public float GetAimRange() {  return aimRange; }
}
