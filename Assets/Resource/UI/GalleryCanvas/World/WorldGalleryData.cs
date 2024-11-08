using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpriteCollection", menuName = "Gallery/WorldData")]
public class WorldGalleryData : ScriptableObject
{
    public List<Sprite> sprites;
}
