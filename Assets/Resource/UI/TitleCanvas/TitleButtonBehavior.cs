using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleButtonBehavior : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;

    Image image;

    int state = 0;
    float timer;
    const float maxTime = 0.2f;

    public void SetState(int _num) { state = _num; }

    void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        switch (state)
        {
            case 0:
                if (timer <= 0) { return; }
                timer = Mathf.Clamp(timer - Time.deltaTime, 0, maxTime);
                Shrink();
                break;
            case 1:
                if (timer >= maxTime) { return; }
                timer = Mathf.Clamp(timer + Time.deltaTime, 0, maxTime);
                Expand();
                break;
        }

        image.sprite = sprites[state];
    }

    void Expand()
    {
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f, Mathf.Clamp01(timer / maxTime));
    }
    void Shrink()
    {
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f, Mathf.Clamp01(timer / maxTime));
    }
}
