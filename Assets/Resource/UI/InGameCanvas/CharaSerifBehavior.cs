using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaSerifBehavior : MonoBehaviour
{
    const float lifeTime = 3;
    string[] serifs = new string[(int)RADIO_CHAT_ID.MAX_NUM] { "", "ピンチ！助けて！", "前に出ます！", "援護します！", "" };

    readonly static Vector3[] windowLocalPos = new Vector3[2] { new Vector3(196, 0, 0), new Vector3(-196, 0, 0) };
    readonly static Vector3[] windowAndTextLocalScale = new Vector3[2] { new Vector3(1, 1, 1), new Vector3(-1, 1, 1) };

    Text serifText;

    float timer = 0;

    public void SetSerif(int _teamNum, int _serifNum)
    {
        if (OSCManager.OSCinstance.roomData.myBannerNum % 2 != _teamNum % 2)
        {
            Destroy(gameObject);
            return;
        }

        serifText = transform.GetChild(0).GetComponent<Text>();

        transform.localPosition = windowLocalPos[_teamNum];
        transform.localScale = windowAndTextLocalScale[_teamNum];
        serifText.transform.localScale = windowAndTextLocalScale[_teamNum];

        serifText.text = serifs[_serifNum];
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime) { Destroy(gameObject); }
    }
}
