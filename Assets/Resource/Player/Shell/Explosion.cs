using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionData", menuName = "Create/PlayerData/ExplosionData", order = 1)]

public class Explosion : ScriptableObject
{
    [SerializeField, Header("Explosion Body")] GameObject explosionBody;
    [SerializeField, Header("Explosion Radius")] float scale;
    [SerializeField, Header("Explosion Anim")] RuntimeAnimatorController explosionAnim;

    public GameObject GetBody() { return explosionBody; }
    public float GetScale() { return scale; }
    public RuntimeAnimatorController GetAnim() { return explosionAnim; }
}
