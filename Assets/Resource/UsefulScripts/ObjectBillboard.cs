using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBillboard : MonoBehaviour
{
    public Vector3 FixedAngles = Vector3.zero;

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(FixedAngles);
    }
}
