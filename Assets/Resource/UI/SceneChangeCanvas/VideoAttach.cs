using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoAttach : MonoBehaviour
{
    Material ri;
    [SerializeField] Texture gb;
    [SerializeField] Texture videoTexture;

    int count = 0;

    void Start()
    {
        ri = GetComponent<RawImage>().material;
        ri.SetTexture("_mainTex", gb);
    }

    void FixedUpdate()
    {
        count++;
        if (count > 1)
        {
            ri.SetTexture("_mainTex", videoTexture);
            Destroy(this);
        }
    }
}
