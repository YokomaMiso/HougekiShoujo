using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecterArrow : MonoBehaviour
{
    Vector2 defaultPos;
    [SerializeField] Vector2 addPos;

    bool add;
    float timer = moveTime;
    const float moveTime = 0.5f;

    public void SetAdd()
    {
        add = true;
        timer = moveTime;
        transform.localPosition = defaultPos + addPos;
    }
    public void Init()
    {
        add = false;
        timer = 0;
        transform.localPosition = defaultPos;
    }

    void Awake()
    {
        defaultPos = transform.localPosition;
    }

    void Update()
    {
        if (!add) { return; }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = moveTime;
            add = false;
            transform.localPosition = defaultPos;
            return;
        }

        float nowRate = (timer / moveTime);
        transform.localPosition = defaultPos + addPos * nowRate;
    }
}
