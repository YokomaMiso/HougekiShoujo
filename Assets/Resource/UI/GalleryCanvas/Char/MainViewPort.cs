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

    private int maxPage;

    void Update()
    {
        if (InputManager.GetKeyDown(BoolActions.SouthButton))
        {
            if (!IsFourthPage(currentPage))
            {
                return;
            }
            PlayVoice(currentPage);
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

        if (page == 1)
        {
            currentPage = GetPageFromPool(layoutPrefabs[0]);
            pageTypeCount = 0;
            UpdatePageContent(currentPage, characterData, 0, pageTypeCount);
            return;
        }

        currentPageStart++;
        currentPageEnd = currentPageStart + (characterData.weapon?.Count ?? 0) - 1;
        if (page >= currentPageStart && page <= currentPageEnd)
        {
            currentPage = GetPageFromPool(layoutPrefabs[1]);
            pageTypeCount = 1;
            UpdatePageContent(currentPage, characterData, page, pageTypeCount);
            return;
        }

        currentPageStart = currentPageEnd + 1;
        currentPageEnd = currentPageStart + (characterData.animatorControllers?.Count ?? 0) - 1;
        if (page >= currentPageStart && page <= currentPageEnd)
        {
            currentPage = GetPageFromPool(layoutPrefabs[2]);
            pageTypeCount = 2;
            int animetionCount = page - currentPageStart;
            UpdatePageContent(currentPage, characterData, animetionCount, pageTypeCount);
            return;
        }

        currentPageStart = currentPageEnd + 1;
        currentPageEnd = currentPageStart;
        if (page == currentPageStart)
        {
            currentPage = GetPageFromPool(layoutPrefabs[3]);
            pageTypeCount = 3;
            return;
        }

        currentPageStart = currentPageEnd + 1;
        currentPageEnd = currentPageStart + (characterData.CharVoice?.Count ?? 0) - 1;
        if (page >= currentPageStart && page <= currentPageEnd)
        {
            currentPage = GetPageFromPool(layoutPrefabs[4]);
            pageTypeCount = 4;
            int voiceCount = page - currentPageStart;
            UpdatePageContent(currentPage, characterData, voiceCount, pageTypeCount);
            return;
        }

        Debug.LogError($"ÈëÁ¦¤µ¤ì¤¿¥Ú©`¥¸ {page} Ÿo„¿!");
    }


    private GameObject GetPageFromPool(GameObject layoutPrefab)
    {
        GameObject page = Instantiate(layoutPrefab, contentParent);
        Vector3 localPos = new Vector3(199,123,0);
        page.transform.localPosition = localPos;
        page.transform.localRotation = Quaternion.identity;
        page.transform.localScale = Vector3.one;
        return page;
    }


    public void EnterCharacterGallery(CharacterData characterData)
    {
        int animatorCount = characterData.animatorControllers != null ? characterData.animatorControllers.Count : 0;
        int charVoiceCount = characterData.CharVoice != null ? characterData.CharVoice.Count : 0;
        int weaponPageCount = characterData.weapon != null ? characterData.weapon.Count : 0;

        maxPage = charIntroCount + skethImageCount + animatorCount + charVoiceCount + weaponPageCount;

        SwitchPage(1, characterData);
    }

    public int GetMaxPage()
    {
        return maxPage;
    }

    public void SetActive(bool state)
    {
        if (currentPage != null)
        {
            currentPage.SetActive(state);
        }
    }

    private void UpdatePageContent(GameObject page, CharacterData characterData, int pageIndex,int pageType)
    {
        int weaponCount = pageIndex - 2;
        switch (pageType)
        {
            case 0:
                UpdateCharIntro(page, characterData);
                break;
            case 1:
                UpdateWeaponIntro(page, characterData, weaponCount);
                break;
            case 2:
                UpdateCharAnime(page, characterData, pageIndex);
                break;
            case 3:
                break;
            case 4:
                UpdateVoicePrefab(page, characterData, pageIndex);
                break;
            default:
                break;
        }
    }
    private void UpdateCharIntro(GameObject page,CharacterData characterData)
    {
        //Text
        Transform nameTransform = page.transform.Find("Name");
        if (nameTransform != null)
        {
            Text nameText = nameTransform.GetComponent<Text>();
            if (nameText != null)
            {
                nameText.text = characterData.charData.characterName;
            }
        }

        Transform schoolNameTransform = page.transform.Find("School");
        if (schoolNameTransform != null)
        {
            Text schoolNameText = schoolNameTransform.GetComponent<Text>();
            if (schoolNameText != null)
            {
                schoolNameText.text = characterData.charData.schoolText;
            }
        }

        Transform schoolYear = page.transform.Find("SchoolYear");
        if (schoolYear != null)
        {
            Text schoolYearText = schoolYear.GetComponent<Text>();
            if (schoolYearText != null)
            {
                schoolYearText.text = characterData.charData.schoolYear;
            }
        }

        Transform charIntro = page.transform.Find("Design");
        if (charIntro != null)
        {
            Text charIntroText = charIntro.GetComponent<Text>();
            if (charIntroText != null)
            {
                charIntroText.text = characterData.charData.charDesign;
            }
        }

        //Image
        Transform schoolIcon = page.transform.Find("SchoolIcon");
        if (schoolIcon != null)
        {
            Image schoolIconImage = schoolIcon.GetComponent<Image>();
            if (schoolIconImage != null)
            {
                schoolIconImage.sprite = characterData.charData.schoolIcon;
            }
        }

        Transform emote = page.transform.Find("emote");
        if (emote != null)
        {
            Image emoteImage = emote.GetComponent<Image>();
            if (emoteImage != null)
            {
                emoteImage.sprite = characterData.charData.emoteIllustration;
            }
        }
    }

    private void UpdateWeaponIntro(GameObject page, CharacterData characterData,int WeaponType)
    {
        //Text
        Transform weaponNameTransform = page.transform.Find("WeaponName");
        if (weaponNameTransform != null)
        {
            Text weaponNameText = weaponNameTransform.GetComponent<Text>();
            if (weaponNameText != null)
            {
                weaponNameText.text = characterData.weapon[WeaponType].weaponName;
            }
        }

        Transform weaponIntroTransform = page.transform.Find("WeaponIntro");
        if (weaponIntroTransform != null)
        {
            Text weaponIntroText = weaponIntroTransform.GetComponent<Text>();
            if (weaponIntroText != null)
            {
                weaponIntroText.text = characterData.weapon[WeaponType].weaponText;
            }
        }

        //image
        Transform weaponIconTransform = page.transform.Find("WeaponIcon");
        if (weaponIconTransform != null)
        {
            Image weaponIconImage = weaponIconTransform.GetComponent<Image>();
            if (weaponIconImage != null)
            {
                weaponIconImage.sprite = characterData.weapon[WeaponType].weaponIcon;
            }
        }
    }

    private void UpdateCharAnime(GameObject page, CharacterData characterData, int animeCount)
    {
        Transform CharSprite = page.transform.Find("CharSprite");
        if (CharSprite != null)
        {
            Animator charAnime = CharSprite.GetComponent<Animator>();
            if (charAnime != null)
            {
                charAnime.runtimeAnimatorController = characterData.animatorControllers[animeCount];
            }
        }
    }

    private void UpdateVoicePrefab(GameObject page, CharacterData characterData, int VoiceCount)
    {
        Transform VoiceName = page.transform.Find("VoiceName");
        if (VoiceName != null)
        {
            Text voiceNameText = VoiceName.GetComponent<Text>();
            if (voiceNameText != null)
            {
                voiceNameText.text = characterData.CharVoice[VoiceCount].voiceText;
            }
        }

        Transform voiceObject = page.transform.Find("VoiceObject");
        if (voiceObject != null)
        {
            AudioSource voiceClip = voiceObject.GetComponent<AudioSource>();
            if (voiceClip != null)
            {
                voiceClip.clip = characterData.CharVoice[VoiceCount].voiceClip;
            }
        }

        //Transform VoiceText = page.transform.Find("VoiceText");
        //if (VoiceText != null)
        //{
        //    Text voice = VoiceText.GetComponent<Text>();
        //    if (voice != null)
        //    {
        //        voice.text = characterData.CharVoice[VoiceCount];
        //    }
        //}
    }

    private void PlayVoice(GameObject voiceObject)
    {

        Transform voiceObj = voiceObject.transform.Find("VoiceObject");
        AudioSource voicePlayer = voiceObj.GetComponent<AudioSource>();
        if (voicePlayer != null&& voicePlayer.clip!=null)
        {
            voicePlayer.Play();
        }
    }

    bool IsFourthPage(GameObject page)
    {
        if (layoutPrefabs != null && layoutPrefabs.Length > 4)
        {
            return page != null && page.name.StartsWith(layoutPrefabs[4].name);
        }
        return false; 
    }

    public void GenerateIll(CharacterData characterData)
    {
        GameObject newObject = new GameObject("CharacterImage");
        newObject.tag = "GalleryObject";
        newObject.transform.SetParent(contentParent);
        RectTransform rectTransform = newObject.AddComponent<RectTransform>();
        Image image = newObject.AddComponent<Image>();
        image.sprite = characterData.charData.characterIllustration;
        rectTransform.sizeDelta = new Vector2(1920, 1080);
        rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.localPosition = new Vector3(-200, 0, 0);
        rectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        rectTransform.SetAsFirstSibling();
    }

    public void DeletCharIll()
    {
        GameObject[] galleryObjects = GameObject.FindGameObjectsWithTag("GalleryObject");
        foreach (GameObject obj in galleryObjects)
        {
            Destroy(obj);
        }
    }
}