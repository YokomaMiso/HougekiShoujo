using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoAttach : MonoBehaviour
{
    Material ri;
    VideoPlayer vp;
    [SerializeField] Texture gb;
    [SerializeField] Texture videoTexture;

    float applyTimer;
    const float applyTime = 0.1f;

    void Start()
    {
        ri = GetComponent<RawImage>().material;
        ri.SetTexture("_mainTex", gb);

        vp = GetComponent<VideoPlayer>();
        vp.frame = 0;
    }

    void Update()
    {
        if (applyTimer < applyTime)
        {
            applyTimer += Time.deltaTime;
            if (applyTimer >= applyTime)
            {
                applyTimer = applyTime;
                ri.SetTexture("_mainTex", videoTexture);
            }
        }
        vp.playbackSpeed = Time.deltaTime * Application.targetFrameRate;
    }
}
