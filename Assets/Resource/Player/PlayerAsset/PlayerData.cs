using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum STATUS_ID { STR = 0, DEX, AGI, CON, AOE, STATUS_MAX_NUM };

[CreateAssetMenu(fileName = "PlayerData", menuName = "Create/PlayerData/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [Header("Status Variable")]
    [SerializeField, Header("Status Array")] int[] statusValue;
    public const int maxStatusCount = (int)STATUS_ID.STATUS_MAX_NUM;

    [Header("Shell Data")]
    [SerializeField, Header("Shells")] Shell[] shells;
    public const int maxShellCount = 3;

    [Header("Weapon Data")]
    [SerializeField, Header("Weapon")] Weapon weapon;

    public int SetStatus(int _value, STATUS_ID _num)
    {
        int num = (int)_num;

        if (num >= maxStatusCount) { return -1; }
        statusValue[num] = _value;
        return statusValue[num];
    }
    public int AddStatus(int _value, STATUS_ID _num)
    {
        int num = (int)_num;

        if (num >= maxStatusCount) { return -1; }
        statusValue[num] += _value;
        return statusValue[num];
    }
    public int GetStatus(STATUS_ID _num)
    {
        int num = (int)_num;
        return statusValue[num];
    }
    public int[] GetAllStatus()
    {
        return statusValue;
    }

    public Shell SetShell(Shell _shell,int _num)
    {
        if (_num >= maxShellCount) { return null; }
        shells[_num] = _shell;
        return shells[_num];
    }
    public Shell GetShell(int _num)
    {
        if (_num >= maxShellCount) { return null; }
        return shells[_num];
    }
    public Shell[] GetShells()
    {
        return shells;
    }

    public Weapon SetWeapon(Weapon _weapon)
    {
        weapon = _weapon;
        return weapon;
    }
    public Weapon GetWeapon() { return weapon; }
    
}
