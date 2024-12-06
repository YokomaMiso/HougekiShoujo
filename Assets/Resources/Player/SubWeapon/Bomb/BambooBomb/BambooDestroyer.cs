using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooDestroyer : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<BambooBehavior>())
        {
            BambooBehavior bamboo = other.GetComponent<BambooBehavior>();
            bamboo.DeleteBamboo();
        }
            
    }
}
