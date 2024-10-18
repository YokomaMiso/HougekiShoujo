using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainViewPort : MonoBehaviour
{
    public CharGalleryManager charGalleryManager;

    public Vector3 ImagePosition = new Vector3(-25, 130, 0);
    public Vector2 ImageSize = new Vector2(3508, 4961);
    public Vector3 ImageScale = new Vector3(0.2f, 0.2f, 0.2f);

    public Vector3 CharPosition = new Vector3(-150, 100, 0);
    public Vector2 CharSize = new Vector2(200, 200);
    public Vector3 CharScale = new Vector3(0.2f, 0.2f, 0.2f);

    public Transform contentParent;
    public GameObject characterImagePrefab;
    public GameObject characterAnimePrefab;

    private GameObject CurrentCharacter;

    public void GenerateCharacterILL(CharacterData character)
    {
        GameObject newImage = Instantiate(characterImagePrefab, contentParent);
        newImage.tag = "GalleryObject";

        RectTransform imageRectTransform = newImage.GetComponent<RectTransform>();
        if (imageRectTransform != null)
        {
            imageRectTransform.localPosition = ImagePosition;
            imageRectTransform.sizeDelta = ImageSize;
            imageRectTransform.localScale = ImageScale;
        }

        Image buttonImage = newImage.GetComponentInChildren<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = character.characterIllustration;
        }
    }

    public void GenerateCharacterAnime(CharacterData character)
    {
        GameObject newAnime = Instantiate(characterAnimePrefab, contentParent);
        newAnime.tag = "GalleryObject";

        Transform charSpriteTransform = newAnime.transform.Find("CharSprite");

        RectTransform imageRectTransform = newAnime.GetComponent<RectTransform>();
        if (imageRectTransform != null)
        {
            imageRectTransform.localPosition = CharPosition;
            imageRectTransform.sizeDelta = CharSize;
            imageRectTransform.localScale = CharScale;
        }

        Animator animator = charSpriteTransform.GetComponent<Animator>();
        if (animator != null && character.animatorControllers.Count > 0)
        {
            animator.runtimeAnimatorController = character.animatorControllers[0];
        }
        CurrentCharacter = newAnime;
    }

    public void UpdateAnimatorController(CharacterData character,int currentAnimatorIndex)
    {
        Transform charSpriteTransform = CurrentCharacter.transform.Find("CharSprite");

        Animator animator = charSpriteTransform.GetComponent<Animator>();

        if (animator != null)
        {
            animator.runtimeAnimatorController = character.animatorControllers[currentAnimatorIndex];
            animator.Play(0);
        }
    }

    public void ClearCharacterILL()
    {
        foreach (Transform child in contentParent)
        {
            if (child.CompareTag("GalleryObject"))
            {
                Destroy(child.gameObject);
            }
        }
    }
}