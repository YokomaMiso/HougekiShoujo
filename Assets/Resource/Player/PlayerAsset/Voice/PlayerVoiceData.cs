using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerVoiceData", menuName = "Create/PlayerData/PlayerVoiceData", order = 1)]
public class PlayerVoiceData : ScriptableObject
{
    [Header("Character Voice")]
    [SerializeField] AudioClip titleCall;
    [SerializeField] AudioClip selfPR;
    [SerializeField] AudioClip ready;
    [SerializeField] AudioClip gameStart;
    [SerializeField] AudioClip reload;
    [SerializeField] AudioClip useMain;
    [SerializeField] AudioClip useSub;
    [SerializeField] AudioClip kill;
    [SerializeField] AudioClip friendlyFire;
    [SerializeField] AudioClip damage;
    [SerializeField] AudioClip damageFF;
    [SerializeField] AudioClip damageTrap;
    [SerializeField] AudioClip thanks;
    [SerializeField] AudioClip roundWin;
    [SerializeField] AudioClip gameWin;

    public AudioClip GetTitleCall() { return titleCall; }
    public AudioClip GetSelfPR() { return selfPR; }
    public AudioClip GetReady() { return ready; }
    public AudioClip GetGameStart() { return gameStart; }
    public AudioClip GetReload() { return reload; }
    public AudioClip GetUseMain() { return useMain; }
    public AudioClip GetUseSub() { return useSub; }
    public AudioClip GetKill() { return kill; }
    public AudioClip GetFriendlyFire() { return friendlyFire; }
    public AudioClip GetDamage() { return damage; }
    public AudioClip GetDamageFF() { return damageFF; }
    public AudioClip GetDamageTrap() { return damageTrap; }
    public AudioClip GetThanks() { return thanks; }
    public AudioClip GetRoundWin() { return roundWin; }
    public AudioClip GetGameWin() { return gameWin; }



}
