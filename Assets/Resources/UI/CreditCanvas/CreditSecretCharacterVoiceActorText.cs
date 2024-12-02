using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditSecretCharacterVoiceActorText : MonoBehaviour
{
    void Start()
    {
        if (Managers.instance.unlockFlag[(int)UNLOCK_ITEM.TSUBASA])
        {
            GetComponent<Text>().text = "ç‡ëO éÈó¢àü\nãﬂâq óÉ";
        }
    }
}
