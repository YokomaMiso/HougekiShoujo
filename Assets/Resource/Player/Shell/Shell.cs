using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SHELL_TYPE { BLAST = 0, CANON, MORTAR };

[CreateAssetMenu(fileName = "ShellData", menuName = "Create/PlayerData/ShellData", order = 1)]
public class Shell : ScriptableObject
{
    [SerializeField, Header("Shell Icon")] Sprite shellIcon;
    [SerializeField, Header("Shell Type")] SHELL_TYPE type;
    [SerializeField, Header("Projectile")] GameObject projectile;
    [SerializeField, Header("Explain"), TextArea(1, 2)] string shellExplain;

    [SerializeField, Header("Reload Time")] float reloadTime;

    public Sprite GetShellIcon() { return shellIcon; }
    public SHELL_TYPE GetShellType() { return type; }
    public GameObject GetProjectile() {  return projectile; }
    public string GetShellExplain() {  return shellExplain; }
}
