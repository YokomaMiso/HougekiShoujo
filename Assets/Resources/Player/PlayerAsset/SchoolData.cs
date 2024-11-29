using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SCHOOL_ID { DAITO = 0, KYUSHA, KANON, MAX_NUM };

[CreateAssetMenu(fileName = "SchoolData", menuName = "Create/PlayerData/SchoolData", order = 1)]
public class SchoolData : ScriptableObject
{
    [SerializeField, Header("School Name")] string schoolName;
    [SerializeField, Header("School Icon")] Sprite schoolIcon;

    public string GetSchoolName() { return schoolName; }
    public Sprite GetSchoolIcon() {  return schoolIcon; }
}
