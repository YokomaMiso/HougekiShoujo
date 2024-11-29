using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomGraffiti : MonoBehaviour
{
    [SerializeField] Sprite[] graffities;

    void Start()
    {
        transform.GetComponent<Image>().sprite = graffities[Random.Range(0, graffities.Length)];
        Destroy(this);
    }
}
