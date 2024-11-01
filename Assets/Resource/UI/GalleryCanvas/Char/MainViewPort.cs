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

    public Vector2 AnimeSelecterSize = new Vector2(15, 15);
    public Vector2 AnimeSelecterOffset = new Vector2(15, 15);
    public Vector2 VoiceSelecterSize = new Vector2(15, 15);
    public Vector2 VoiceSelecterOffset = new Vector2(15, 15);

    public Transform contentParent;
    public GameObject characterImagePrefab;
    public GameObject characterAnimePrefab;

    private GameObject CurrentCharacter;
    private List<Image> generatedImages = new List<Image>();

    private Text currentText;
    public Font font;

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
        generatedImages.Clear();

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

        Vector3 currentPosition = new Vector3(newAnime.transform.localPosition.x + AnimeSelecterOffset.x,
                                              newAnime.transform.localPosition.y+AnimeSelecterOffset.y,
                                              newAnime.transform.localPosition.z);


        for(int i = 0; i < character.animatorControllers.Count; i++)
        {
            GameObject imageObject = new GameObject("AnimatorImage_" + i);
            imageObject.transform.SetParent(contentParent, false);
            imageObject.tag = "GalleryObject";

            RectTransform rectTransform = imageObject.AddComponent<RectTransform>();
            Image image = imageObject.AddComponent<Image>();

            rectTransform.sizeDelta = AnimeSelecterSize;
            rectTransform.localPosition = new Vector3(currentPosition.x + i * 50, currentPosition.y, currentPosition.z);
            rectTransform.localScale = Vector3.one;

            image.color = Color.white;

            generatedImages.Add(image);
        }

        GameObject VoiceObject = new GameObject("VoiceButton");
        VoiceObject.transform.SetParent(contentParent, false);
        VoiceObject.tag = "GalleryObject";

        RectTransform rectVoiceTransform = VoiceObject.AddComponent<RectTransform>();
        Image voiceImage = VoiceObject.AddComponent<Image>();

        rectVoiceTransform.sizeDelta = VoiceSelecterSize;
        rectVoiceTransform.localPosition = new Vector3(currentPosition.x+VoiceSelecterOffset.x,
                                                       currentPosition.y + VoiceSelecterOffset.y,
                                                       currentPosition.z);
        rectVoiceTransform.localScale = Vector3.one;
        voiceImage.color = Color.white;
        Color colorA = voiceImage.color;
        colorA.a = 0.2f;
        voiceImage.color = colorA;

        GameObject textObject = new GameObject("VoiceButtonText");
        textObject.transform.SetParent(VoiceObject.transform, false);

        RectTransform textRectTransform = textObject.AddComponent<RectTransform>();
        Text textComponent = textObject.AddComponent<Text>();

        currentText = textComponent;

        textRectTransform.sizeDelta = rectVoiceTransform.sizeDelta;
        textRectTransform.localPosition = Vector3.zero;
        textComponent.alignment = TextAnchor.MiddleCenter;

        textComponent.text = "×Ô¼º½B½é";
        textComponent.font = font;
        textComponent.fontSize = 32;
        textComponent.color = Color.black;

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

    public void ModifyGeneratedImages(int index)
    {
        for (int i = 0; i < generatedImages.Count; i++)
        {
            RectTransform rectTransform = generatedImages[i].GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = AnimeSelecterSize;
                if (i == index)
                {
                    rectTransform.sizeDelta *= 1.5f;
                }
            }
        }
    }

    public void ChangeText(string newText)
    {
        if (currentText != null)
        {
            currentText.text = newText;
        }
    }
}