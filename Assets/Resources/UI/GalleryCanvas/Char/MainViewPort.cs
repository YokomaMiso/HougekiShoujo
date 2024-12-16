using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainViewPort : MonoBehaviour
{
    public CharGalleryManager charGalleryManager;

    public Transform contentParent;
    public GameObject[] layoutPrefabs;
    public GameObject currentPage;

    private int charIntroCount=1;
    private int skethImageCount = 1;

    private bool isPlaying=false;
    private int maxPage = 3;

    private int voicePage;
    public void SetVoicePage(int voiceIndex){voicePage = voiceIndex;}

    private CharacterData currentCharData;

    public float typingSpeed = 0.05f;

    private int minVoiceIndex=0;
    private int maxVoiceIndex=11;
    public void ResetVoiceIndex()
    {
        minVoiceIndex=0;
        maxVoiceIndex = 11;
    }

    private AudioSource currentVoiceObject;

    private Animator currentAnimatorObject;

    private bool isPageMove = false;

    public bool GetPageMove() { return isPageMove; }

    void Update()
    {
        if (InputManager.GetKeyDown(BoolActions.WestButton))
        {
            if (!IsSecondPage(currentPage))
            {
                return;
            }
            if (!isPlaying)
            {
                StartCoroutine(PlayVoiceAndText(currentPage,voicePage, currentCharData));
            }
        }
    }

    public void SwitchPage(int page, CharacterData characterData)
    {
        if (currentPage != null)
        {
            Destroy(currentPage);
        }

        int currentPageStart = 1;
        int currentPageEnd;
        int pageTypeCount = -1;

        switch (page)
        {
            case 1:
                currentPage=GetPageFromPool(layoutPrefabs[0]);
                pageTypeCount = 0;
                UpdatePageContent(currentPage,characterData,pageTypeCount);
                if (GalleryManager.Instance.CurrentState == GalleryState.CharacterGallery)
                {
                    currentPage.transform.localPosition= new Vector3(297, -351, 0.0f);
                    return;
                }
                StartCoroutine(MovePage(currentPage.transform, new Vector3(297, -351, 0.0f), 0.5f));
                return;
            case 2:
                currentPage=GetPageFromPool(layoutPrefabs[1]);
                pageTypeCount = 1;
                UpdatePageContent(currentPage, characterData, pageTypeCount);
                currentCharData=characterData;
                return;
            case 3:
                currentPage=GetPageFromPool(layoutPrefabs[2]);
                pageTypeCount = 2;
                UpdatePageContent(currentPage, characterData, pageTypeCount);
                return;
            default:
                break;
        }
    }


    private GameObject GetPageFromPool(GameObject layoutPrefab)
    {
        GameObject page = Instantiate(layoutPrefab, contentParent);
        page.transform.localRotation = Quaternion.identity;
        return page;
    }


    public void SetActive(bool state)
    {
        if (currentPage != null)
        {
            currentPage.SetActive(state);
        }
    }

    private void UpdatePageContent(GameObject page, CharacterData characterData,int pageType)
    {
        switch (pageType)
        {
            case 0:
                UpdateCharIll(page, characterData);
                UpdateCharIntro(page, characterData);
                UpdateCharAnime(0, characterData);
                break;
            case 1:
                UpdateCharIll(page, characterData);
                UpdateVoiceText(page, characterData);
                voicePage = 0;
                break;
            case 2:
                UpdateCharIll(page, characterData);
                break;
            default:
                break;
        }
    }

    private void UpdateCharIll(GameObject page, CharacterData characterData)
    {
        if (page != null)
        {
            RawImage charImage = page.transform.Find("Photo/Mask/CharIll")?.GetComponent<RawImage>();
            if (charImage != null)
            {
                charImage.texture = characterData.charData.characterIllustration.texture;
            }
        }
    }

    private void UpdateCharIntro(GameObject page, CharacterData characterData)
    {
        if (page != null)
        {
            Text charNameText = page.transform.Find("CharName/Text")?.GetComponent<Text>();
            if (charNameText != null)
            {
                charNameText.text = characterData.charData.characterName;
            }

            Text SchoolNameText = page.transform.Find("School/Text")?.GetComponent<Text>();
            if (SchoolNameText != null)
            {
                SchoolNameText.text = characterData.charData.schoolText;
            }

            Text schoolYearText = page.transform.Find("SchoolYear/Text")?.GetComponent<Text>();
            if (schoolYearText != null)
            {
                schoolYearText.text = characterData.charData.schoolYear;
            }

            Text mainWeaponNameText = page.transform.Find("MainWeapon/Name")?.GetComponent<Text>();
            if (mainWeaponNameText != null)
            {
                mainWeaponNameText.text = characterData.weapon[0].GetWeaponName();
            }

            Text mainWeaponIntroText = page.transform.Find("MainWeapon/Intro")?.GetComponent<Text>();
            if (mainWeaponIntroText != null)
            {
                mainWeaponIntroText.text = characterData.weapon[0].GetWeaponText();
            }

            Text subWeaponNameText = page.transform.Find("SubWeapon/Name")?.GetComponent<Text>();
            if (subWeaponNameText != null)
            {
                subWeaponNameText.text = characterData.weapon[1].GetWeaponName();
            }

            Text subWeaponIntroText = page.transform.Find("SubWeapon/Intro")?.GetComponent<Text>();
            if (subWeaponIntroText != null)
            {
                subWeaponIntroText.text = characterData.weapon[1].GetWeaponText();
            }

            Text charSettingText = page.transform.Find("Setting/Intro")?.GetComponent<Text>();
            if (charSettingText != null)
            {
                charSettingText.text = characterData.charData.GetCharDesign();
            }

            Animator charAnimetion = page.transform.Find("Sprite")?.GetComponent<Animator>();
            if(charAnimetion!= null)
            {
                charAnimetion.runtimeAnimatorController= characterData.animatorControllers[0];
                currentAnimatorObject = charAnimetion;
            }

            RawImage mainWeaponIcon = page.transform.Find("MainWeapon/Icon")?.GetComponent<RawImage>();
            if (mainWeaponIcon != null)
            {
                mainWeaponIcon.texture = characterData.weapon[0].weaponIcon.texture;
            }

            RawImage subWeaponIcon = page.transform.Find("SubWeapon/Icon")?.GetComponent<RawImage>();
            if (subWeaponIcon != null)
            {
                subWeaponIcon.texture = characterData.weapon[1].weaponIcon.texture;
            }

            RawImage charEmote = page.transform.Find("Emote")?.GetComponent<RawImage>();
            if (charEmote != null)
            {
                charEmote.texture = characterData.charData.emoteIllustration.texture;
            }

        }
    }

    public void UpdateCharAnime(int animeIndex, CharacterData characterData)
    {
        if (currentAnimatorObject!=null)
        {
            currentAnimatorObject.runtimeAnimatorController = characterData.animatorControllers[animeIndex];
        }
    }

    private void UpdateVoiceText(GameObject page, CharacterData characterData)
    {
        int maxVoiceNum = characterData.CharVoice.Count;


        Transform voice = page.transform.Find("ScrollView/Viewport/Content/Voice_0");

        Transform voiceParent = page.transform.Find("ScrollView/Viewport/Content");
        if (voice == null)
        {
            Debug.LogError("Voice object not found!");
            return;
        }

        Text voiceText = voice.GetComponentInChildren<Text>();
        if (voiceText != null)
        {
            voiceText.text = $"{characterData.charData.characterName}            {characterData.CharVoice[0].GetVoiceText()}";
        }

        float spacing = 56.0f;
        for (int i = 1; i < maxVoiceNum; i++)
        {
            Transform existingVoice = page.transform.Find($"Voice_{i}");

            if (existingVoice != null)
            {
                Text existingText = existingVoice.GetComponentInChildren<Text>();
                if (existingText != null)
                {
                    existingText.text = characterData.CharVoice[i].GetVoiceText();
                }
            }
            else
            {
                GameObject newVoice = Instantiate(voice.gameObject, voiceParent);
                RectTransform rectTransform = newVoice.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(
                    voice.GetComponent<RectTransform>().anchoredPosition.x,
                    voice.GetComponent<RectTransform>().anchoredPosition.y - i * spacing);

                Text newVoiceText = newVoice.GetComponentInChildren<Text>();
                if (newVoiceText != null)
                {
                    newVoiceText.text = $"{characterData.charData.characterName}            {characterData.CharVoice[i].GetVoiceText()}";
                }
                newVoice.name = $"Voice_{i}";
            }
        }
    }




    bool IsSecondPage(GameObject page)
    {
        if (layoutPrefabs != null && layoutPrefabs.Length > 2)
        {
            return page != null && page.name.StartsWith(layoutPrefabs[1].name);
        }
        return false; 
    }

    private IEnumerator PlayVoiceAndText(GameObject gameObject, int VoiceCount, CharacterData characterData)
    {
        isPlaying = true;

        string fullText = characterData.CharVoice[VoiceCount].GetVoiceFullText();
        Transform VoiceText = gameObject.transform.Find("VoiceText");
        Transform voiceObject = gameObject.transform.Find("VoiceObject");
        Text voice = VoiceText?.GetComponent<Text>();
        AudioSource voiceClip = voiceObject?.GetComponent<AudioSource>();

        if (voiceClip != null)
        {
            currentVoiceObject = voiceClip;
            currentVoiceObject.clip = characterData.CharVoice[VoiceCount].voiceClip;
            currentVoiceObject.Play();

            float totalDuration = currentVoiceObject.clip.length;
            float timePerCharacter = totalDuration / fullText.Length;

            string currentText = "";

            foreach (char c in fullText)
            {
                if (voice == null || currentVoiceObject == null)
                {
                    isPlaying = false;
                    yield break;
                }

                currentText += c;
                voice.text = currentText;
                yield return new WaitForSeconds(Mathf.Min(typingSpeed, timePerCharacter));
            }

            if (currentVoiceObject != null)
            {
                while (currentVoiceObject != null && currentVoiceObject.isPlaying)
                {
                    yield return null; 
                }
            }


            isPlaying = false;
        }
        else
        {
            Debug.LogError("Voice object or AudioSource is missing!");
            isPlaying = false;
        }
    }

    public int GetMaxPage()
    {
        return maxPage;
    }


    //void OnDestroy()
    //{
    //    currentVoiceObject.Stop();
    //    StopAllCoroutines();
    //}

    public void DeletCharIll()
    {
        GameObject[] galleryObjects = GameObject.FindGameObjectsWithTag("GalleryObject");
        foreach (GameObject obj in galleryObjects)
        {
            Destroy(obj);
        }
    }

    public void EnterCharacterGallery(CharacterData characterData)
    {
        SwitchPage(1, characterData);
    }

    Transform FindChildByName(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }

            Transform result = FindChildByName(child, childName);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    private IEnumerator MovePage(Transform targetObject, Vector3 targetPosition, float duration)
    {
        isPageMove = true;
        float elapsedTime = 0f;
        Vector3 startingPosition = targetObject.localPosition;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            t = EaseInOutQuad(t);
            targetObject.localPosition = Vector3.Lerp(startingPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.localPosition = targetPosition;
        isPageMove = false;
    }

    private float EaseInOutQuad(float t)
    {
        if (t < 0.5f)
            return 2 * t * t;
        return 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
    }

    private float EaseInQuad(float t) => t * t;

    private float EaseOutQuad(float t) => 1 - (1 - t) * (1 - t);

    public void PageReturn()
    {
        StartCoroutine(PageReturnCoroutine());
    }

    private IEnumerator PageReturnCoroutine()
    {
        yield return MovePage(currentPage.transform, new Vector3(2405, -351, 0.0f), 0.5f);

        SetActive(false);
        DeletCharIll();
    }

    public void SetCusorPosition(int index, int maxNum)
    {
        if (!IsSecondPage(currentPage))
        {
            return;
        }

        Transform contentTransform = currentPage.transform.Find("ScrollView/Viewport/Content");
        Transform cursorTransform = currentPage.transform.Find("ScrollView/Viewport/Content/voicePlayer");
        ScrollRect scrollRect = currentPage.transform.Find("ScrollView").GetComponent<ScrollRect>();

        RectTransform contentRect = contentTransform.GetComponent<RectTransform>();
        RectTransform viewportRect = currentPage.transform.Find("ScrollView/Viewport").GetComponent<RectTransform>();

        float itemHeight = 56.0f;

        if (index < minVoiceIndex)
        {
            minVoiceIndex--;
            maxVoiceIndex--;

            contentRect.localPosition -= new Vector3(0, itemHeight, 0);
        }
        else if (index > maxVoiceIndex)
        {
            minVoiceIndex++;
            maxVoiceIndex++;

            contentRect.localPosition += new Vector3(0, itemHeight, 0);
        }

        Transform newTargetVoice = contentTransform.Find($"Voice_{index}");
        if (newTargetVoice == null)
        {
            Debug.LogError($"Voice_{index} not found");
            return;
        }

        cursorTransform.localPosition = new Vector3(cursorTransform.localPosition.x, newTargetVoice.localPosition.y, cursorTransform.localPosition.z);

        minVoiceIndex = Mathf.Clamp(minVoiceIndex, 0, maxNum - 1);
        maxVoiceIndex = Mathf.Clamp(maxVoiceIndex, 0, maxNum - 1);
    }






}