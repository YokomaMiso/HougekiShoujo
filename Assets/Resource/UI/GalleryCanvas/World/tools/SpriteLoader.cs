using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SpriteImporter : MonoBehaviour
{
    [MenuItem("Tools/Create Sprite Collection from Texture")]
    public static void CreateSpriteCollectionFromTexture()
    {
        Texture2D selectedTexture = Selection.activeObject as Texture2D;

        if (selectedTexture == null)
        {
            Debug.LogError("Please select a valid texture.");
            return;
        }

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
        spriteCollection.bulidTextPair = new List<BulidingTextPair>();

        foreach (Sprite sprite in spriteList)
        {
            BulidingTextPair newPair = new BulidingTextPair
            {
                sprite = sprite,
                bulidingText = "Default Text",
                hasText = true
            };
            spriteCollection.bulidTextPair.Add(newPair);
        }

        string assetPathAndName = "Assets/Resource/UI/GalleryCanvas/World/Data/" + selectedTexture.name + ".asset";
        AssetDatabase.CreateAsset(spriteCollection, assetPathAndName);
        AssetDatabase.SaveAssets();

        Debug.Log("Sprite collection created successfully at " + assetPathAndName);
    }
}
