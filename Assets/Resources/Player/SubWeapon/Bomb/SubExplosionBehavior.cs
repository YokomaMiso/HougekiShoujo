using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubExplosionBehavior : ExplosionBehavior
{
    protected bool[] hitedPlayer = new bool[6];

    public void ResetHitedPlayer(int _num)
    {
        hitedPlayer[_num] = false;
    }
}
