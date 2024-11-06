using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum GalleryState
{
    GalleryMain,
    CharacterSelect,
    CharacterGallery,
    WorldSelect,
    WorldGallery,
    SoundSelect,
    SoundGallery,
}

public class GalleryManager : MonoBehaviour
{
    private static GalleryManager _instance;
    public static GalleryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GalleryManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("GalleryStateManager");
                    _instance = obj.AddComponent<GalleryManager>();
                }
            }
            return _instance;
        }
    }

    public GalleryState CurrentState { get; private set; }

    public GameObject largeCategory;
    public GameObject charCategory;
    public GameObject worldCategory;
    public GameObject soundCategory;


    //入力処理用(GalleryMain)
    private bool canChangeIndex = true;
    private int inputIndex = -1;
    private Transform[] childTransforms;
    private Vector3 selecterSize = new Vector3(1.1f, 1.1f, 1.1f);
    private Vector3 defaultSize = new Vector3(1f, 1f, 1f);

    //fade
    public float fadeDuration = 1f;
    private bool isFade=false;
    private int nextState = -1;
    private static float Duration = 1.0f;

    //バター移動用変数
    private static Vector3 targetPosition = new Vector3(-108,70,0);
    private Vector3[] currentPosition = new Vector3[3];


    private void Start()
    {
        if (largeCategory != null)
        {
            childTransforms = new Transform[largeCategory.transform.childCount];
            for (int i = 0; i < childTransforms.Length; i++)
            {
                childTransforms[i] = largeCategory.transform.GetChild(i);
                currentPosition[i] = childTransforms[i].localPosition;
            }
        }
        SetState(GalleryState.GalleryMain);
    }

    public void SetState(GalleryState newState)
    {
        CurrentState = newState;
        UpdateGalleryUI();
    }

    private void UpdateGalleryUI()
    {
        switch (CurrentState)
        {
            case GalleryState.GalleryMain:
                charCategory.SetActive(false);
                worldCategory.SetActive(false);
                soundCategory.SetActive(false);
                break;
            case GalleryState.CharacterSelect:
                charCategory.SetActive(true);
                worldCategory.SetActive(false);
                soundCategory.SetActive(false);
                break;
            case GalleryState.CharacterGallery:
                break;
            case GalleryState.WorldSelect:
                break;
            case GalleryState.WorldGallery:
                largeCategory.SetActive(false);
                charCategory.SetActive(false);
                worldCategory.SetActive(true);
                soundCategory.SetActive(false);
                break;
            case GalleryState.SoundSelect:
                break;
            case GalleryState.SoundGallery:
                largeCategory.SetActive(false);
                charCategory.SetActive(false);
                worldCategory.SetActive(false);
                soundCategory.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case GalleryState.GalleryMain:
                HandleGalleryMainInput();
                GalleryMainUpdate();
                break;
            case GalleryState.CharacterSelect:
                HandleCharSelectInput();
                break;
            case GalleryState.CharacterGallery:
                HandleCharGalleryInput();
                break;
            case GalleryState.WorldSelect:
                HandleWorldSelectInput();
                break;
            case GalleryState.SoundSelect:
                HandleSoundSelectInput();
                break;
            default:
                break;
        }
    }













    /*--------------------------------------------------------------------------------------------------------------
     * -------------------------------------------------------------------------------------------------------------
     * -------------------------------------------アップデート関連関数----------------------------------------------
     * -------------------------------------------------------------------------------------------------------------
     * -------------------------------------------------------------------------------------------------------------
     * -------------------------------------------------------------------------------------------------------------*/
    
    private void HandleGalleryMainInput()
    {
        if (isFade) { return; }
        Vector2 axisInput = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);
        if (axisInput.y < -0.5f && canChangeIndex)
        {
            inputIndex++;
            if (inputIndex > childTransforms.Length) { inputIndex = 0; }
            GalleryMainSelect();
            canChangeIndex = false;
        }
        else if (axisInput.y > 0.5f && canChangeIndex)
        {
            inputIndex--;
            if (inputIndex < 0) { inputIndex = childTransforms.Length; }
            GalleryMainSelect();
            canChangeIndex = false;
        }
        else if (axisInput.y > -0.5f && axisInput.y < 0.5f)
        {
            canChangeIndex = true;
        }

        if (InputManager.GetKeyDown(BoolActions.SouthButton) && inputIndex >= 0)
        {
            nextState = inputIndex;
            TransFadeOut(inputIndex);
        }

        if (!isFade && nextState >= 0) 
        {
            switch (nextState)
            {
                case 0:
                    SetState(GalleryState.CharacterSelect);
                    break;
                case 1:
                    SetState(GalleryState.WorldSelect);
                    break;
                case 2:
                    SetState(GalleryState.SoundSelect);
                    break;
                default:
                    break;
            }
        }

    }

    private void HandleCharSelectInput()
    {
        BackToMainGallery();
    }

    private void HandleCharGalleryInput()
    {

    }


    private void GalleryMainUpdate()
    {
        BackToMainTitle();
    }

    private void HandleWorldSelectInput()
    {
        BackToMainGallery();
    }

    private void HandleSoundSelectInput()
    {
        BackToMainGallery();
    }

    private void BackToMainGallery()
    {
        if (InputManager.GetKeyDown(BoolActions.EastButton))
        {
            charCategory.SetActive(false);
            nextState = -1;
            TransFadeIn(inputIndex);
        }
        if (!isFade && nextState == -1)
        {
            SetState(GalleryState.GalleryMain);
        }
    }

    private void GalleryMainSelect()
    {

        for (int i = 0; i < childTransforms.Length; i++)
        {
            childTransforms[i].localScale = defaultSize;
        }

        if (inputIndex >= 0 && inputIndex < childTransforms.Length)
        {
            childTransforms[inputIndex].localScale = selecterSize;
        }
    }

    private void BackToMainTitle()
    {
        if (InputManager.GetKeyDown(BoolActions.EastButton)&&!isFade)
        {
            Managers.instance.ChangeState(GAME_STATE.TITLE);
            Managers.instance.ChangeScene(GAME_STATE.TITLE);
        }
    }









    /*--------------------------------------------------------------------------------------------------------------
     * -------------------------------------------------------------------------------------------------------------
     * -------------------------------------------Fade、バター操作関連関数----------------------------------------------
     * -------------------------------------------------------------------------------------------------------------
     * -------------------------------------------------------------------------------------------------------------
     * -------------------------------------------------------------------------------------------------------------*/

    private void ButtonFadeIn(Transform gameobject)
    {
        Image imageObject = gameobject.GetComponent<Image>();
        if (imageObject != null)
        {
            StartCoroutine(FadeIn(imageObject, Duration));
        }
    }

    private void ButtonFadeOut(Transform gameobject)
    {
        Image imageObject = gameobject.GetComponent<Image>();
        if (imageObject != null)
        {
            StartCoroutine(FadeOut(imageObject,Duration));
        }
    }

    private void ButtonMove(Transform targetobject,Vector3 targetposition)
    {
        StartCoroutine(MoveObject(targetobject, targetposition, Duration));
    }

    private IEnumerator FadeIn(Image image, float duration)
    {
        float elapsedTime = 0f;
        isFade = true;
        Color color = image.color;

        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(0, 1, elapsedTime / duration);
            image.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 1;
        image.color = color;
        isFade = false;
    }

    private IEnumerator FadeOut(Image image, float duration)
    {
        float elapsedTime = 0f;
        isFade = true;
        Color color = image.color;

        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(1, 0, elapsedTime / duration);
            image.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 0;
        image.color = color;
        isFade = false;
    }

    private IEnumerator MoveObject(Transform targetObject, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = targetObject.localPosition;

        while (elapsedTime < duration)
        {
            targetObject.localPosition = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.localPosition = targetPosition;
    }

    private void TransFadeIn(int index) 
    {
        for (int i = 0; i < childTransforms.Length; i++)
        {
            if (i == index)
            {
                ButtonMove(childTransforms[i], currentPosition[i]);
                continue;
            }
            ButtonFadeIn(childTransforms[i]);
        }
    }

    private void TransFadeOut(int index)
    {
        for (int i = 0; i < childTransforms.Length; i++)
        {
            if (i == index)
            {
                ButtonMove(childTransforms[i], targetPosition);
                continue;
            }
            ButtonFadeOut(childTransforms[i]);
        }
    }
    public bool GetFadeState()
    {
        return isFade;
    }
}
