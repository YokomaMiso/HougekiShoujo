using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SUB_TYPE { BUFF = 0, INSTALLATION, BLINK };

[CreateAssetMenu(fileName = "SubWeaponData", menuName = "Create/PlayerData/SubWeaponData", order = 1)]
public class SubWeapon : ScriptableObject
{
    [SerializeField, Header("SubWeapon Icon")] Sprite subWeaponIcon;
    [SerializeField, Header("Sub Type")] SUB_TYPE subType;
    [SerializeField, Header("Installation")] GameObject Installation;
    [SerializeField, Header("Explain"), TextArea(1, 3)] string subWeaponExplain;

    [SerializeField, Header("Reload Time")] float reloadTime;
    [SerializeField, Header("Speed Rate")] float speedRate;
    [SerializeField, Header("Life Time")] float lifeTime;

    public Sprite GetIcon() { return subWeaponIcon; }
    public SUB_TYPE GetSubType() { return subType; }
    public GameObject GetMine() {  return Installation; }
    public string GetSubWeaponExplain() {  return subWeaponExplain; }
    public float GetReloadTime() {  return reloadTime; }
    public float GetSpeedRate() { return speedRate; }
    public float GetLifeTime() { return lifeTime; }
}
