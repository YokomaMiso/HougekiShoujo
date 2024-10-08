using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TitleVideoBehavior : MonoBehaviour
{
    [SerializeField] VideoClip startVideo;
    [SerializeField] VideoClip loopVideo;

    VideoPlayer vp;

    float timer;
    float lifeTime;

    void Start()
    {
        lifeTime = (float)startVideo.length;
        vp = GetComponent<VideoPlayer>();
        vp.clip = startVideo;
        vp.Play();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            vp.clip = loopVideo;
            vp.Play();
            transform.root.GetComponent<TitleCanvasBehavior>().ChangeTitleState(TITLE_STATE.SELECT);

            //int characterID = Random.Range(0,Managers.instance.gameManager.playerDatas.Length);
            int characterID = 0;
            SoundManager.PlayVoice(Managers.instance.gameManager.playerDatas[characterID].GetPlayerVoiceData().GetTitleCall());

            Destroy(this);
        }
    }
}
