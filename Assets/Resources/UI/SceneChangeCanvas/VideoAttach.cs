using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoAttach : MonoBehaviour
{
    RawImage ri;
    Material riMat;
    VideoPlayer vp;
    [SerializeField] Texture gb;
    [SerializeField] Texture videoTexture;

    float applyTimer = 0;
    const float applyTime = 1.0f;

    void Start()
    {
        vp = GetComponent<VideoPlayer>();

        vp.prepareCompleted += OnCompletePrepare;
        vp.Prepare();

        ri = GetComponent<RawImage>();
        riMat = ri.material;
        riMat.SetTexture("_mainTex", gb);
        riMat.SetFloat("_alpha", 0);
    }

    void Update()
    {
        if (!vp.isPrepared) { return; }

        applyTimer += Time.deltaTime;
        float alpha = Mathf.Clamp01(applyTimer / applyTime);
        riMat.SetFloat("_alpha", alpha);
    }

    void OnCompletePrepare(VideoPlayer _vp)
    {
        vp.frame = 1;
        vp.Play();
        riMat.SetTexture("_mainTex", videoTexture);
    }

    void OnDestroy()
    {
        riMat.SetTexture("_mainTex", gb);
        riMat.SetFloat("_alpha", 0);
    }
}
