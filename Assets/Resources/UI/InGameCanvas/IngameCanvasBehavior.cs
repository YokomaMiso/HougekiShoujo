using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameCanvasBehavior : MonoBehaviour
{
    [SerializeField] KillLogCanvas killLogList;
    [SerializeField] SerifListBehavior serifList;

    void Awake()
    {
        Managers.instance.gameManager.ingameCanvas = this;

        if (Managers.instance.unlockFlag[(int)UNLOCK_ITEM.UI_DELETE])
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DisplayShellIcon shellIcon = transform.GetChild(i).GetComponent<DisplayShellIcon>();
                if (shellIcon == null)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

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
