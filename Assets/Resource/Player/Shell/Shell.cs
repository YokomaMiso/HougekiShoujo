using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SHELL_TYPE { BLAST = 0, CANON, MORTOR };

[CreateAssetMenu(fileName = "ShellData", menuName = "Create/PlayerData/ShellData", order = 1)]
public class Shell : ScriptableObject
{
    [SerializeField, Header("Shell Icon")] Sprite shellIcon;
    [SerializeField, Header("Shell Type")] SHELL_TYPE type;
    [SerializeField, Header("Projectile")] GameObject projectile;

    public Sprite GetSprite() { return shellIcon; }
    public SHELL_TYPE GetShellType() { return type; }
    public GameObject GetProjectile() {  return projectile; }
}
