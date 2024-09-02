using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerID : MonoBehaviour
{
    int id;
    public int SetID(int _id) { id = _id; return id; }
    public int GetID() { return id; }
}
