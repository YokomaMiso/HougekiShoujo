using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOneBehavior : MonoBehaviour
{
    [SerializeField] GameObject[] texts;

    void Start()
    {
        Instantiate(texts[(int)Managers.instance.nowLanguage],transform);
    }
}
