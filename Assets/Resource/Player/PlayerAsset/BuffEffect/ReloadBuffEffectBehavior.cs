using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadBuffEffectBehavior : MonoBehaviour
{
    ParticleSystem vfx;
    void Start()
    {
        vfx = GetComponent<ParticleSystem>();
    }
    public void DisplayBuff(float _value)
    {
        if (vfx == null) { return; }

        if (_value == 1.0f) { vfx.Stop(); return; }

        if (!vfx.isPlaying) { vfx.Play(); }
    }
}
