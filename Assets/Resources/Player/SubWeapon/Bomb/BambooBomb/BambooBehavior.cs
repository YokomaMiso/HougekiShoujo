using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooBehavior : MonoBehaviour
{
    int num;
    public void SetNum(int _num) { num = _num; }
    public void DeleteBamboo() { transform.parent.GetComponent<BambooWallBehavior>().DeleteBamboo(num); }
}
