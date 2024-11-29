using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffectBehavior : MonoBehaviour
{
    enum BUFF_NUM { BUFF = 0, DEBUFF };
    [SerializeField] Sprite[] buffSprites;
    static readonly float[] rotX = new float[2] { -45, 180 - 45 };
    static readonly float[] posY = new float[2] { 1.0f, 2.5f };

    ParticleSystem vfx;

    void Start()
    {
        vfx = GetComponent<ParticleSystem>();
    }

    public void DisplayBuff(float _value)
    {
        if (vfx == null) { return; }

        if (_value == 1.0f) { vfx.Stop(); return; }
        
        int num = 0;
        if (_value < 1) { num = 1; }

        vfx.textureSheetAnimation.SetSprite(0, buffSprites[num]);
        var shape = vfx.shape;
        shape.rotation = new Vector3(rotX[num], 0, 0);
        shape.position = new Vector3(0, posY[num], -1);

        if (!vfx.isPlaying) { vfx.Play(); }
    }
}
