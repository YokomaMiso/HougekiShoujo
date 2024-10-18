using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameCanvasBehavior : MonoBehaviour
{
    [SerializeField] KillLogCanvas killLogList;
    [SerializeField] SerifListBehavior serifList;

    void Awake() { Managers.instance.gameManager.ingameCanvas = this; }

    void OnDestroy() { Managers.instance.gameManager.ingameCanvas = null; }

    public void AddSerif(MachingRoomData.RoomData _roomData, RADIO_CHAT_ID _chatID)
    {
        serifList.AddSerif(_roomData, _chatID);
    }

    public void AddKillLog(Player _player)
    {
        killLogList.AddKillLog(_player);
    }
}
