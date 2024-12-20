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
    float applyTime;

    void Awake()
    {
        ri = GetComponent<RawImage>().material;
        ri.SetTexture("_mainTex", gb);

        vp = GetComponent<VideoPlayer>();
        vp.frame = 0;

        float frameRate = 1.0f / Time.deltaTime;
        vp.playbackSpeed = 120.0f / frameRate;
        applyTime = vp.playbackSpeed * 0.1f;

        vp.prepareCompleted += OnCompletePrepare;

        vp.Prepare();
    }

    void Update()
    {
        float frameRate = 1.0f / Time.deltaTime;
        vp.playbackSpeed = 120.0f / frameRate;
    }

    void OnCompletePrepare(VideoPlayer _vp)
    {
        ri.SetTexture("_mainTex", videoTexture);
        vp.Play();
    }
}
