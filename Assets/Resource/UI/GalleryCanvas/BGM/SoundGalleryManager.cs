using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SoundGalleryManeger : MonoBehaviour
{
    public GallerySoundData soundsData;
    public GameObject soundsPrefab;

    public Transform contentParent;
    public Vector3 StartPosition = new Vector3(0, 0, 0);

    private bool RightStickcanChange=true;
    private int CurrentSoundsIndex = -1;

    int MaxSounds=0;
    
    private List<GameObject> voiceButtons = new List<GameObject>();

    public AudioSource audioSource;


    void Start()
    {
        if (soundsData != null)
        {
            MaxSounds = soundsData.sounds.Count;
            GenerateVoiceButton();
        }
    }

    public void GenerateVoiceButton()
    {

        float verticalSpacing = 150.0f;
        Vector3 startPos = StartPosition;

        for (int i = 0; i < MaxSounds; i++)
        {
            GameObject newButton = Instantiate(soundsPrefab, contentParent);

            RectTransform rectTransform = newButton.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = startPos - new Vector3(0, i * verticalSpacing, 0);
            }
            else
            {
                newButton.transform.localPosition = startPos - new Vector3(0, i * verticalSpacing, 0);
            }
            LoadSoundsText(newButton, i);
            newButton.name = $"VoiceButton_{i + 1}";

            voiceButtons.Add(newButton);
        }
    }

    void Update()
    {
        SoundsGalleryInput();
    }

    private void SoundsGalleryInput()
    {
        Vector2 Inout = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);
        if (GalleryManager.Instance.CurrentState == GalleryState.SoundSelect)
        {
            if (Inout.y > 0.5f && RightStickcanChange&&!audioSource.isPlaying)
            {
                CurrentSoundsIndex--;
                if (CurrentSoundsIndex < 0)
                {
                    CurrentSoundsIndex = MaxSounds-1;
                }
                SoundsSelecter(CurrentSoundsIndex);
                RightStickcanChange = false;
            }
            else if (Inout.y < -0.5f && RightStickcanChange&&!audioSource.isPlaying)
            {
                CurrentSoundsIndex++;
                if (CurrentSoundsIndex >= MaxSounds)
                {
                    CurrentSoundsIndex = 0;
                }
                SoundsSelecter(CurrentSoundsIndex);
                RightStickcanChange = false;
            }
            else if (Inout.y > -0.5f && Inout.y < 0.5f)
            {
                RightStickcanChange = true;
            }

            if (InputManager.GetKeyDown(BoolActions.SouthButton)&& !audioSource.isPlaying)
            {
                SetSounds();
            }
            if(InputManager.GetKeyDown(BoolActions.EastButton))
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Pause();
                }
            }
        }
    }

    private void SoundsSelecter(int index)
    {

        Color selectedColor = Color.yellow;
        Color defaultColor = Color.white;

        for (int i = 0; i < voiceButtons.Count; i++)
        {
            Image buttonImage = voiceButtons[i].GetComponentInChildren<Image>();
            if (buttonImage != null)
            {
                if (i == index)
                {
                    buttonImage.color = selectedColor;
                }
                else
                {
                    buttonImage.color = defaultColor;
                }
            } 
        }
    }
    private void LoadSoundsText(GameObject gameobject,int index)
    {
        if (gameobject != null)
        {
            Transform soundTransform = gameobject.transform.Find("BackGround/Text");
            if (soundTransform != null)
            {
                Text soundText = soundTransform.GetComponent<Text>();
                if (soundText != null)
                {
                    string newText = $"{index + 1}. {soundsData.sounds[index].Name} / " +
                        $"{soundsData.sounds[index].Author}  {soundsData.sounds[index].Time}";
                    soundText.text = newText;
                }
            }
        }
    }

    private void SetSounds()
    {
        if (audioSource != null)
        {
            audioSource.clip = soundsData.sounds[CurrentSoundsIndex].BgmClip;
            if (audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
    }

    public void DeleteSoundsImage()
    {
        foreach (Transform child in contentParent)
        {
            if (child.CompareTag("GalleryObject"))
            {
                Destroy(child.gameObject);
            }
        }
        voiceButtons.Clear();
    }
}
