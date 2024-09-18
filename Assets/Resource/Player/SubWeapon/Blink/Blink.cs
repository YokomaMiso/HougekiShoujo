using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    void Start()
    {
        Player player = GetComponent<Player>();
        if (!player) { Destroy(this); return; }

        float length = 5;

        Vector3 targetVec = player.GetInputVector();
        RaycastHit hit;
        RaycastHit hit2;

        Vector3 warpPos = transform.position + targetVec * length;
        if (Physics.Raycast(warpPos - targetVec, targetVec, out hit, 1))
        {
            if (hit.collider.tag == "Ground")
            {
                if (Physics.Raycast(transform.position, targetVec, out hit2, length))
                {
                    transform.position = hit2.point;
                }
            }
        }

        transform.position += targetVec * length;
        Destroy(this);
    }
}
