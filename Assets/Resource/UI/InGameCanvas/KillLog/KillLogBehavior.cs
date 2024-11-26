using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillLogBehavior : MonoBehaviour
{
    Image killPlayer;
    Image deadPlayer;

    readonly static Vector3 startPos = Vector3.right * 1400;
    readonly static Vector3 endPos = Vector3.right * 700;

    int logNum = 0;

    float timer;
    const float posTime = 0.25f;
    const float backTime = 5.25f;
    const float lifeTime = 5.75f;

    //readonly Color[] constColor = new Color[2] { new Color(0.1255f, 0.3137f, 0.8941f), new Color(1, 0.125f, 0.125f, 1) };
    //Color killColor;
    //Color deadColor;

    const float killVoiceTime = 0.5f;
    bool playKillVoice;
    AudioClip killVoice;
    Player killerPlayer;

    [SerializeField] Sprite[] killerSpriteBG;
    [SerializeField] Sprite[] deadManSpriteBG;

    public void SetText(Player _player)
    {
        killPlayer = transform.GetChild(0).GetComponent<Image>();
        deadPlayer = transform.GetChild(2).GetComponent<Image>();

        int killer = _player.GetKiller();
        int deadMan = _player.GetPlayerID();

        MachingRoomData.RoomData killerRoomData = OSCManager.OSCinstance.GetRoomData(killer);
        MachingRoomData.RoomData deadManRoomData = OSCManager.OSCinstance.GetRoomData(deadMan);

        killPlayer.sprite = killerSpriteBG[killerRoomData.myTeamNum];
        deadPlayer.sprite = deadManSpriteBG[deadManRoomData.myTeamNum];

        killPlayer.transform.GetChild(0).GetComponent<Text>().text = killerRoomData.playerName;
        deadPlayer.transform.GetChild(0).GetComponent<Text>().text = deadManRoomData.playerName;

        killPlayer.transform.GetChild(1).GetComponent<Image>().sprite = Managers.instance.gameManager.playerDatas[killerRoomData.selectedCharacterID].GetCharacterAnimData().GetCharaIcon();
        deadPlayer.transform.GetChild(1).GetComponent<Image>().sprite = Managers.instance.gameManager.playerDatas[deadManRoomData.selectedCharacterID].GetCharacterAnimData().GetCharaIcon();

        int killerTeam = killerRoomData.myTeamNum;
        int deadManTeam = deadManRoomData.myTeamNum;
        //killColor = constColor[killerTeam];
        //deadColor = constColor[deadManTeam];
        //killPlayer.color = killColor;
        //deadPlayer.color = deadColor;

        //Ž©Œˆ‚È‚ç
        if (killer == deadMan)
        {
            _player.PlayVoice(_player.GetPlayerData().GetPlayerVoiceData().GetDamage(), Camera.main.transform, 1);
        }
        //FF‚È‚ç
        else if (killerTeam == deadManTeam)
        {
            _player.PlayVoice(_player.GetPlayerData().GetPlayerVoiceData().GetDamageFF(), Camera.main.transform, 1);
            killerPlayer = Managers.instance.gameManager.GetPlayer(killer);
            killVoice = killerPlayer.GetPlayerData().GetPlayerVoiceData().GetFriendlyFire();
        }
        else
        {
            _player.PlayVoice(_player.GetPlayerData().GetPlayerVoiceData().GetDamage(), Camera.main.transform, 1);
            killerPlayer = Managers.instance.gameManager.GetPlayer(killer);
            killVoice = killerPlayer.GetPlayerData().GetPlayerVoiceData().GetKill();
        }

    }
    public void LogNumAdd() { logNum++; }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer < backTime)
        {
            float posValue = Mathf.Clamp01(timer / posTime);
            Vector3 addPos = Vector3.up * (logNum * -40);
            transform.localPosition = Vector3.Lerp(startPos + addPos, endPos + addPos, posValue);
        }
        else
        {
            float posValue = Mathf.Clamp01(timer - backTime / lifeTime - backTime);
            Vector3 addPos = Vector3.up * (logNum * -40);
            transform.localPosition = Vector3.Lerp(endPos + addPos, startPos + addPos, posValue);
        }
        //float colorValue = Mathf.Clamp01(1.0f - (timer - colorChangeTime));
        //Color multiplyRate = new Color(1, 1, 1, colorValue);
        //killPlayer.color = multiplyRate;
        //deadPlayer.color = multiplyRate;

        if (timer > lifeTime) { Destroy(gameObject); }


        if (killVoice == null) { return; }
        if (playKillVoice) { return; }
        if (timer > killVoiceTime)
        {
            killerPlayer.PlayVoice(killVoice, Camera.main.transform);
            playKillVoice = true;
        }
    }
}
