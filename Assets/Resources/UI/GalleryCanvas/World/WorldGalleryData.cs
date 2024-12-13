using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpriteCollection", menuName = "Gallery/WorldData")]
public class WorldGalleryData : ScriptableObject
{
    //public List<Sprite> sprites;
    public List<BulidingTextPair> bulidTextPair;
}

[System.Serializable]
public class BulidingTextPair
{
    public Sprite sprite;
    [TextArea] public string bulidingText;
    public bool hasText;
}