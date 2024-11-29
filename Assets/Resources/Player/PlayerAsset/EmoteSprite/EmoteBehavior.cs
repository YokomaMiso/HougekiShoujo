using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteBehavior : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] AudioClip[] clips;
    int spriteNum = 0;

    SpriteRenderer image;
    const float maxHeight = 2.5f;
    const float lifeTime = 4.0f;
    const float fadeStart = 3.0f;
    const float arriveTargetPos = 0.5f;

    float timer;

    public void SetSpriteNum(RADIO_CHAT_ID _ID)
    {
        spriteNum = (_ID - RADIO_CHAT_ID.APOLOGIZE) % (int)RADIO_TYPE.EMOTE;
    }

    void Start()
    {
        image = transform.GetChild(0).GetComponent<SpriteRenderer>();
        image.sprite = sprites[spriteNum];
        SoundManager.PlaySFXForUI(clips[spriteNum],this.transform);
    }

    void Update()
    {
        timer += Time.deltaTime;
        ImageUpdate();
        if (timer > lifeTime) { Destroy(gameObject); }
    }

    void ImageUpdate()
    {
        if (timer < arriveTargetPos)
        {
            image.transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.up * maxHeight, timer / arriveTargetPos);
            image.color = Color.white * timer / arriveTargetPos;
        }
        else if (timer < fadeStart)
        {
            image.transform.localPosition = Vector3.up * maxHeight;
            image.color = Color.white;
        }
        else
        {
            image.color = Color.white * (1.0f - (timer - fadeStart));
        }
    }
}
