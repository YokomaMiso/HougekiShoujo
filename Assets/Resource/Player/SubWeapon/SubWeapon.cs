using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SUB_TYPE { BUFF = 0, INSTALLATION, BLINK, BOMB };
public enum BUFF_TYPE { NONE = 0, SPEED, RELOAD };

[CreateAssetMenu(fileName = "SubWeaponData", menuName = "Create/PlayerData/SubWeaponData", order = 1)]
public class SubWeapon : ScriptableObject
{
    [SerializeField, Header("SubWeapon Icon")] Sprite subWeaponIcon;
    [SerializeField, Header("Sub Type")] SUB_TYPE subType;
    [SerializeField, Header("Buff Type")] BUFF_TYPE buffType;
    [SerializeField, Header("Installation")] GameObject installation;
    [SerializeField, Header("Explain"), TextArea(1, 3)] string subWeaponExplain;

    [SerializeField, Header("Installation Start Anim")] RuntimeAnimatorController installationStartAnim;
    [SerializeField, Header("Installation Loop Anim")] RuntimeAnimatorController installationLoopAnim;

    [SerializeField, Header("Reload Time")] float reloadTime;
    [SerializeField, Header("Speed Rate")] float speedRate;
    [SerializeField, Header("Life Time")] float lifeTime;

    public Sprite GetIcon() { return subWeaponIcon; }
    public SUB_TYPE GetSubType() { return subType; }
    public BUFF_TYPE GetBuffType() { return buffType; }
    public GameObject GetInstallation() { return installation; }
    public string GetSubWeaponExplain() { return subWeaponExplain; }

    public RuntimeAnimatorController GetInstallationStartAnim() { return installationStartAnim; }
    public RuntimeAnimatorController GetInstallationLoopAnim() { return installationLoopAnim; }

    public float GetReloadTime() { return reloadTime; }
    public float GetSpeedRate() { return speedRate; }
    public float GetLifeTime() { return lifeTime; }
}
