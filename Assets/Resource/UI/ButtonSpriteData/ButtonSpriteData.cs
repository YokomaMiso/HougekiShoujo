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

    public Sprite[] GetAllSprites()
    {
        Sprite[] sprites = new Sprite[5];
        sprites[0] = horizontal;
        sprites[1] = vertical;
        sprites[2] = submit;
        sprites[3] = cancel;
        sprites[4] = stageSelect;
        return sprites;
    }
    public Sprite GetSubmit() { return submit; }
    public Sprite GetCancel() { return cancel; }
    public Sprite GetShoulder() { return stageSelect; }

}
