using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackAreaMat", menuName = "Create/AttackAreaMatSelecter", order = 1)]
public class AttackAreaMatSelecter : ScriptableObject
{
    [SerializeField, Header("ブラストの攻撃範囲マテリアル")] public Material blastMat;
    [SerializeField, Header("キャノンの攻撃範囲マテリアル")] public Material canonMat;
    [SerializeField, Header("迫撃砲の攻撃範囲マテリアル")] public Material mortarMat;
}