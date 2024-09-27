using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverAnimation : MonoBehaviour
{
    int index = 0;
    int indexForwardCount = 0;
    const int indexForwardBorder= 3;

    [SerializeField] Sprite[] sprites;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }
    void FixedUpdate()
    {
        indexForwardCount++;
        if(indexForwardCount>= indexForwardBorder)
        {
            index = (index + 1) % sprites.Length;
            image.sprite = sprites[index];

            indexForwardCount = 0;
        }
    }
}
