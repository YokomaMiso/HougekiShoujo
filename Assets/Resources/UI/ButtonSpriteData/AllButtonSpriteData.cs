using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllButtonSpriteData", menuName = "Create/ButtonSpriteData/AllButtonData", order = 1)]
public class AllButtonSpriteData : ScriptableObject
{
    [SerializeField, Header("Horizontal")] ButtonSpriteData[] buttonData;
    public ButtonSpriteData GetButtonSprite(int _num) 
    {
        int num = Mathf.Clamp(_num, 0, buttonData.Length);
        return buttonData[num]; 
    }
}
