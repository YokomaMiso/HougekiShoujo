using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuddenDeathArea : MonoBehaviour
{
    Material areaMat;

    float timer = 0;
    const float maxTime = 15;

    const float minRate = 0.15f;

    void Start()
    {
        areaMat = GetComponent<MeshRenderer>().material;
        areaMat.SetFloat("_sizeRate", 1);
    }

    public void Init() { timer = 0; }

    void Update()
    {
        IngameData.GameData hostIngameData;
        if (Managers.instance.playerID == 0) { hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData; }
        else { hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData; }
        if (hostIngameData.roundTimer > 0) { return; }

        if (timer > maxTime) { return; }
        timer += Managers.instance.timeManager.GetDeltaTime();

        float timeRate = Mathf.Clamp01(timer / maxTime);
        float nowRate = (1.0f - timeRate) * (1.0f - minRate) + minRate;

        transform.GetChild(0).localScale = Vector3.one * nowRate;
        areaMat.SetFloat("_sizeRate", nowRate);
    }
}
