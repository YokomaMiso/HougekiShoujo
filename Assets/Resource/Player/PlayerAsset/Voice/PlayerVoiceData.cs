using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerVoiceData", menuName = "Create/PlayerData/PlayerVoiceData", order = 1)]
public class PlayerVoiceData : ScriptableObject
{
    [Header("Character Voice")]
    [SerializeField] AudioClip[] titleCall;
    [SerializeField] AudioClip[] selfPR;
    [SerializeField] AudioClip[] ready;
    [SerializeField] AudioClip[] gameStart;
    [SerializeField] AudioClip[] reload;
    [SerializeField] AudioClip[] useMain;
    [SerializeField] AudioClip[] useSub;
    [SerializeField] AudioClip[] kill;
    [SerializeField] AudioClip[] friendlyFire;
    [SerializeField] AudioClip[] damage;
    [SerializeField] AudioClip[] damageFF;
    [SerializeField] AudioClip[] damageTrap;
    [SerializeField] AudioClip[] thanks;
    [SerializeField] AudioClip[] roundWin;
    [SerializeField] AudioClip[] gameWin;

    public AudioClip GetTitleCall() { return titleCall[Random.Range(0,titleCall.Length)]; }
    public AudioClip GetSelfPR() { return selfPR[Random.Range(0, selfPR.Length)]; }
    public AudioClip GetReady() { return ready[Random.Range(0, ready.Length)]; }
    public AudioClip GetGameStart() { return gameStart[Random.Range(0, gameStart.Length)]; }
    public AudioClip GetReload() { return reload[Random.Range(0, reload.Length)]; }
    public AudioClip GetUseMain() { return useMain[Random.Range(0, useMain.Length)]; }
    public AudioClip GetUseSub() { return useSub[Random.Range(0, useSub.Length)]; }
    public AudioClip GetKill() { return kill[Random.Range(0, kill.Length)]; }
    public AudioClip GetFriendlyFire() { return friendlyFire[Random.Range(0,friendlyFire.Length)]; }
    public AudioClip GetDamage() { return damage[Random.Range(0,damage.Length)]; }
    public AudioClip GetDamageFF() { return damageFF[Random.Range(0, damageFF.Length)]; }
    public AudioClip GetDamageTrap() { return damageTrap[Random.Range(0, damageTrap.Length)]; }
    public AudioClip GetThanks() { return thanks[Random.Range(0, thanks.Length)]; }
    public AudioClip GetRoundWin() { return roundWin[Random.Range(0, roundWin.Length)]; }
    public AudioClip GetGameWin() { return gameWin[Random.Range(0, gameWin.Length)]; }

}
