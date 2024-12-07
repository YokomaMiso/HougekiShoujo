using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooBehavior : MonoBehaviour
{
    int num;
    public void SetNum(int _num) 
    {
        num = _num; 
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    }
    public void DeleteBamboo() { transform.parent.GetComponent<BambooWallBehavior>().DeleteBamboo(num); }

    public void DeleteCollision() 
    {
        BoxCollider[] col= transform.GetComponents<BoxCollider>();
        for(int i = 0; i < col.Length; i++) { col[i].enabled = false; }
    }
    public void EnableCollision()
    {
        BoxCollider[] col = transform.GetComponents<BoxCollider>();
        for (int i = 0; i < col.Length; i++) { col[i].enabled = true; }
    }
    public void EnableSprite() 
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
    }
}
