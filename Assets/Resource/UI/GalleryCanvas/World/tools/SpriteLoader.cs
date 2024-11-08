using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SpriteImporter : MonoBehaviour
{
    [MenuItem("Tools/Create Sprite Collection from Texture")]
    public static void CreateSpriteCollectionFromTexture()
    {
        Texture2D selectedTexture = Selection.activeObject as Texture2D;

        string texturePath = AssetDatabase.GetAssetPath(selectedTexture);
        Object[] allSprites = AssetDatabase.LoadAllAssetsAtPath(texturePath);

        List<Sprite> spriteList = new List<Sprite>();

        foreach (Object obj in allSprites)
        {
            if (obj is Sprite)
            {
                spriteList.Add(obj as Sprite);
            }
        }


        WorldGalleryData spriteCollection = ScriptableObject.CreateInstance<WorldGalleryData>();
        spriteCollection.sprites = spriteList;

        string assetPathAndName = "Assets/Resource/UI/GalleryCanvas/World/Data" + selectedTexture.name + ".asset";
        AssetDatabase.CreateAsset(spriteCollection, assetPathAndName);
        AssetDatabase.SaveAssets();

    }
}
