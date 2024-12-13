using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioChatWindowForSmartPhone : MonoBehaviour
{
    public void SetRadioChatID(int _radioChatID)
    {
        myRadioChat.ReceiveChatButtonFromUI((RADIO_CHAT_ID)_radioChatID);
        WindowOut();
    }

    public void WindowInAndOut()
    {
        if (active) { WindowOut(); }
        else { WindowIn(); }
    }

    PlayerRadioChat myRadioChat;

    Vector3 defaultPos = Vector3.right * 1040;
    Vector3 addPos = Vector3.right * 820;

    bool active;
    bool moved;
    void WindowIn() { active = true; moved = false; }
    void WindowOut() { active = false; moved = false; }

    float timer;
    const float windowMoveTime = 0.3f;

    void Start()
    {
        if (!Managers.instance.GetSmartPhoneFlag()) { Destroy(gameObject); return; }

        myRadioChat = Managers.instance.gameManager.GetPlayer(Managers.instance.playerID).GetComponent<PlayerRadioChat>();
    }

    void Update()
    {
        if (moved) { return; }

        if (active)
        {
            if (timer < windowMoveTime)
            {
                timer += Time.deltaTime;
                if (timer >= windowMoveTime)
                {
                    timer = windowMoveTime;
                    moved = true;
                }
            }
            float nowRate = Mathf.Sqrt(Mathf.Clamp01(timer / windowMoveTime));
            transform.localPosition = Vector3.Lerp(defaultPos, addPos, nowRate);
        }
        else
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = 0;
                    moved = true;
                }
            }
            float nowRate = 1.0f - Mathf.Pow(Mathf.Clamp01(timer / windowMoveTime), 2);
            transform.localPosition = Vector3.Lerp(addPos, defaultPos, nowRate);
        }
    }
}
