using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackAreaMat", menuName = "Create/AttackAreaMatSelecter", order = 1)]
public class AttackAreaMatSelecter : ScriptableObject
{
    [SerializeField, Header("�u���X�g�̍U���͈̓}�e���A��")] public Material blastMat;
    [SerializeField, Header("�L���m���̍U���͈̓}�e���A��")] public Material canonMat;
    [SerializeField, Header("�����C�̍U���͈̓}�e���A��")] public Material mortarMat;
}