using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EMPExplosionBehavior : ExplosionBehavior
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == hitTag)
        {
            other.AddComponent<SpeedBuff>().SetRateAndTime(0.0f, 3.0f);
        }
    }
}
