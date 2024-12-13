using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InteractBGMObject : MonoBehaviour
{
    [SerializeField] AudioClip[] bgm;
    int prevNum = 0;
    int num = 0;

    float timer = 0;
    const float changeTime = 2.0f;

    /*
    float beat;
    float prevTime = 0;
    int prevSample = 0;
    const float bpm = 115.08f;
    */

    public GameObject GetObject(int _num)
    {
        if (_num < 0) { _num = 0; }
        return transform.GetChild(_num).gameObject;
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SoundObject>().ReceiveSound(bgm[i], SOUND_TYPE.BGM, true);
            if (0 == i)
            {
                transform.GetChild(i).GetComponent<AudioSource>().volume = SoundManager.masterVolume * SoundManager.bgmVolume;
            }
            else
            {
                transform.GetChild(i).GetComponent<AudioSource>().volume = 0.01f;
            }
        }


    }
    public void ChangeBGM(int _num)
    {
        switch (_num)
        {
            case 0:
            case 1:
            case 2:
                prevNum = num;
                num = _num;
                break;
            default:
                Destroy(gameObject);
                return;
        }

        transform.GetChild(num).GetComponent<AudioSource>().time = transform.GetChild(prevNum).GetComponent<AudioSource>().time;

        timer = 0;
    }

    void Update()
    {
        if (prevNum != num)
        {
            timer = 1;
            //timer += Time.deltaTime;
            float nowVolume = SoundManager.masterVolume * SoundManager.bgmVolume;
            float nowRate = Mathf.Clamp01(timer / changeTime);

            transform.GetChild(prevNum).GetComponent<AudioSource>().volume = Mathf.Lerp(nowVolume, 0.01f, Mathf.Pow(nowRate,2));
            transform.GetChild(num).GetComponent<AudioSource>().volume = Mathf.Lerp(0.01f, nowVolume, Mathf.Pow(nowRate, 2));

            if (nowRate >= 1.0f) { prevNum = num; }
        }
    }

    /*
    void Update()
    {
        if (prevNum == num) { return; }

        //beat = bgm[0].frequency * ((1f / Time.deltaTime) / bpm);
        beat = bgm[0].frequency;

        AudioSource prevSource = transform.GetChild(prevNum).GetComponent<AudioSource>();
        float rhythmTime = prevSource.time - prevTime;

        if ((prevSample + beat) % beat > (prevSource.timeSamples + beat) % beat)
        {
            float nowVolume = SoundManager.masterVolume * SoundManager.bgmVolume;
            transform.GetChild(prevNum).GetComponent<AudioSource>().volume = 0.01f;
            transform.GetChild(num).GetComponent<AudioSource>().volume = nowVolume;

            prevNum = num;
        }

        prevSample = prevSource.timeSamples;
    }
    */
}

