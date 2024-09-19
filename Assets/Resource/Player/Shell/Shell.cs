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
    [SerializeField, Header("Projectile Anim")] RuntimeAnimatorController projectileAnim;
    [SerializeField, Header("Explosion")] Explosion explosion;
    [SerializeField, Header("Explain"), TextArea(1, 3)] string shellExplain;

    [SerializeField, Header("Reload Time")] float reloadTime;
    [SerializeField, Header("Recoil Time")] float recoilTime;
    [SerializeField, Header("Projectile Speed")] float projectileSpeed;
    [SerializeField, Header("Aim Range")] float aimRange;
    [SerializeField, Header("Aim Degree")] float aimDegree;
    [SerializeField, Header("Life Time")] float lifeTime;

    public Sprite GetShellIcon() { return shellIcon; }
    public SHELL_TYPE GetShellType() { return shellType; }
    public GameObject GetProjectile() { return projectile; }
    public RuntimeAnimatorController GetAnim() { return projectileAnim; }

    public Explosion GetExplosion() { return explosion; }
    public string GetShellExplain() { return shellExplain; }

    public float GetReloadTime() { return reloadTime; }
    public float GetRecoilTime() { return recoilTime; }
    public float GetSpeed() { return projectileSpeed; }
    public float GetAimRange()
    {
        switch (shellType)
        {
            default:
                return aimRange;
            case SHELL_TYPE.CANON:
                return projectileSpeed * lifeTime * 2;
        }
    }
    public float GetAOEDegree() { return aimDegree; }
    public float GetLifeTime() { return lifeTime; }
}
