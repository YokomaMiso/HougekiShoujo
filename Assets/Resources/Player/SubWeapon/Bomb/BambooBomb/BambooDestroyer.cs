using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooDestroyer : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        BambooBehavior bamboo;
        bamboo = other.transform.parent.GetComponent<BambooBehavior>();
        if (bamboo == null) { return; }

        bamboo.DeleteBamboo();
    }
}
