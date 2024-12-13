using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BSD_", menuName = "Create/ButtonSpriteData/ButtonSpriteData", order = 1)]
public class ButtonSpriteData : ScriptableObject
{
    [SerializeField, Header("Horizontal")] Sprite horizontal;
    [SerializeField, Header("Vertical")] Sprite vertical;
    [SerializeField, Header("Submit")] Sprite submit;
    [SerializeField, Header("Cancel")] Sprite cancel;
    [SerializeField, Header("LeftShoulder")] Sprite stageSelect;
    [SerializeField, Header("LeftStick")] Sprite stickL;
    [SerializeField, Header("RightStick")] Sprite stickR;
    [SerializeField, Header("RightShoulder")] Sprite teamChat;

    public Sprite[] GetAllSprites()
    {
        Sprite[] sprites = new Sprite[8];
        sprites[0] = horizontal;
        sprites[1] = vertical;
        sprites[2] = submit;
        sprites[3] = cancel;
        sprites[4] = stageSelect;
        sprites[5] = stickL;
        sprites[6] = stickR;
        sprites[7] = teamChat;
        return sprites;
    }
    public Sprite GetSubmit() { return submit; }
    public Sprite GetCancel() { return cancel; }
    public Sprite GetShoulder() { return stageSelect; }
    public Sprite GetLeftStick() { return stickL; }
    public Sprite GetRightStick() { return stickR; }
    public Sprite GetTrigger() { return teamChat; }
}
