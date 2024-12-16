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
    WorldIllGallery,
    WorldTextGallery,
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
    public GameObject worldMain;
    public GameObject soundMain;

    private GameObject intantWorldObject;
    private Vector3 spawnWorldPosition;

    public SoundGalleryManeger soundsManeger;
    public MainViewPort mainViewPort;


    //入力処理用(GalleryMain)
    private bool canChangeIndex = true;
    private int inputIndex = -1;
    private List<Transform> childTransforms = new List<Transform>();
    private Vector3 selecterSize = new Vector3(1.1f, 1.1f, 1.1f);
    private Vector3 defaultSize = new Vector3(1f, 1f, 1f);
    private Vector3 currentSoundPosition;
    private Vector3 endSoundPosition = new Vector3(1010.0f,-841.0f,0.0f);

    //fade
    public float fadeDuration = 1f;
    private bool isFade=false;
    private bool isMove = false;
    public void SetMoveState(bool state) => isMove = state;


    private int nextState = -1;
    private static float Duration = 0.2f;

    //バター移動用変数
    private static Vector3 targetPositionCount = new Vector3(-108,70,0);
    private List<Vector3> currentPosition = new List<Vector3>();
    private Vector3[] worldCurrentPosition=new Vector3[2];
    private Vector3[] targetWorldPosition=new Vector3[2];
    private Vector3 currentPositionCount;

    private Vector3 rotationCenter = new Vector3(50.0f,545.0f,0.0f);
    private float rotationAngle=-25.0f;

    public void SetNextStage(int index)
    {
        nextState = index;
    }

    private void Start()
    {
        childTransforms.Clear();
        currentPosition.Clear();
        currentSoundPosition = soundMain.transform.localPosition;
        spawnWorldPosition = new Vector3(-2050.0f, 0.0f, 0);
        if (largeCategory != null)
        {
            for (int i = 0; i < largeCategory.transform.childCount; i++)
            {
                Transform child = largeCategory.transform.GetChild(i);
                childTransforms.Add(child);
                currentPosition.Add(child.localPosition);
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
                break;
            case GalleryState.CharacterSelect:
                charCategory.SetActive(true);
                worldCategory.SetActive(false);
                soundCategory.SetActive(false);
                break;
            case GalleryState.CharacterGallery:
                break;
            case GalleryState.WorldSelect:
                charCategory.SetActive(false);
                worldCategory.SetActive(true);
                soundCategory.SetActive(false);
                break;
            case GalleryState.WorldIllGallery:
                break;
            case GalleryState.WorldTextGallery:
                break;
            case GalleryState.SoundSelect:
                charCategory.SetActive(false);
                worldCategory.SetActive(false);
                soundCategory.SetActive(true);
                break;
            case GalleryState.SoundGallery:
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
                GalleryMainUpdate();
                HandleGalleryMainInput();
                break;
            case GalleryState.CharacterSelect:
                HandleGalleryMainInput();
                break;
            case GalleryState.CharacterGallery:
                break;
            case GalleryState.WorldSelect:
                HandleGalleryMainInput();
                break;
            case GalleryState.SoundSelect:
                HandleGalleryMainInput();
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
        if(isFade) { return; }
        if(isMove) { return; }
        Vector2 axisInput = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);
        if (axisInput.y < -0.5f && canChangeIndex)
        {
            inputIndex++;
            if (inputIndex >= childTransforms.Count) { inputIndex = 0; }
            GalleryMainSelect(inputIndex);
            canChangeIndex = false;
        }
        else if (axisInput.y > 0.5f && canChangeIndex)
        {
            inputIndex--;
            if (inputIndex < 0) { inputIndex = childTransforms.Count-1; }
            GalleryMainSelect(inputIndex);
            canChangeIndex = false;
        }
        else if (axisInput.y > -0.5f && axisInput.y < 0.5f)
        {
            canChangeIndex = true;
        }

        if (InputManager.GetKeyDown(BoolActions.SouthButton) && inputIndex >= 0)
        {
            if(CurrentState == GalleryState.GalleryMain)
            {
                nextState = inputIndex;
                if (nextState == 1) { StartCoroutine(HandleWorldResetSequentially()); }
                if (nextState == 2)
                {
                    if (soundsManeger != null)
                    {
                        soundsManeger.ResetIndex();
                        soundsManeger.SoundsSelecter(0);
                        StartCoroutine(HandleSoundResetSequentially());
                    }
                }
                else
                {
                    TransRotationIn();
                }
                
            }
        }

        if (InputManager.GetKeyDown(BoolActions.EastButton))
        {
            if (soundsManeger.audioSource.isPlaying) { return; }
            if (mainViewPort.GetPageMove()) { return; }
            
            charCategory.SetActive(false);
            nextState = -1;
            switch (CurrentState)
            {
                case GalleryState.CharacterSelect:
                    inputIndex = 0;
                    TransRotationOut();
                    break;
                case GalleryState.WorldSelect:
                    inputIndex = 1;
                    StartCoroutine(HandleWorldExitSequentially());
                    break;
                case GalleryState.SoundSelect:
                    inputIndex = 2;
                    StartCoroutine(HandleSoundExitSequentially());
                    break;
                default:
                    break;
            }
            SetCategoryChild(largeCategory);
            
        }

        if (!isFade) 
        {
            switch (nextState)
            {
                case -1:
                    SetState(GalleryState.GalleryMain);
                    break;
                case 0:
                    SetState(GalleryState.CharacterSelect);
                    childTransforms.Clear();
                    currentPosition.Clear();
                    break;
                case 1:
                    SetCategoryChild(worldCategory);
                    SetState(GalleryState.WorldSelect);
                    break;
                case 2:
                    SetState(GalleryState.SoundSelect);
                    SetCategoryChild(soundCategory);
                    break;
                default:
                    break;
            }
        }

    }


    private void GalleryMainUpdate()
    {
        BackToMainTitle();
    }

    private void GalleryMainSelect(int index)
    {

        for (int i = 0; i < childTransforms.Count; i++)
        {
            if (i == index)
            {
                Image image = childTransforms[i].GetComponent<Image>();
                if(image != null)
                {
                    Color color = image.color;
                    color.a = 1.0f;
                    image.color = color;
                }
            }
            else
            {
                Image image = childTransforms[i].GetComponent<Image>();
                if (image != null)
                {
                    Color color = image.color;
                    color.a = 0.0f;
                    image.color = color;
                }
            }
        }
    }

    private void BackToMainTitle()
    {
        if (InputManager.GetKeyDown(BoolActions.EastButton)&&!isFade&&!isMove)
        {
            Managers.instance.ChangeState(GAME_STATE.TITLE);
            Managers.instance.ChangeScene(GAME_STATE.TITLE);
            isFade = true;
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

    private void ButtonMove(Transform targetobject,Vector3 targetposition,float _duration)
    {
        StartCoroutine(MoveObject(targetobject, targetposition, _duration));
    }

    private void ButtonRotation(RectTransform targetobject,float angle, float _duration)
    {
        StartCoroutine(RotateUI(targetobject,angle, _duration));
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
        isFade = true;
        float elapsedTime = 0f;
        Vector3 startingPosition = targetObject.localPosition;

        while (elapsedTime < duration)
        {
            targetObject.localPosition = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.localPosition = targetPosition;
        isFade = false;
    }

    private IEnumerator RotateUI(RectTransform targetObject, float angle, float duration)
    {
        float elapsedTime = 0f;
        float currentAngle = 0f;
        isFade = true;
        Quaternion startRotation = targetObject.rotation;

        while (elapsedTime < duration)
        {
            float deltaAngle = Mathf.Lerp(0, angle, elapsedTime / duration) - currentAngle;
            currentAngle += deltaAngle;

            targetObject.rotation = startRotation * Quaternion.Euler(0, 0, currentAngle);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.rotation = startRotation * Quaternion.Euler(0, 0, angle);
        isFade = false;
    }


    private IEnumerator HandleWorldExitSequentially()
    {
        isMove = true;
        SetState(GalleryState.GalleryMain);
        if (intantWorldObject != null)
        {
            yield return StartCoroutine(MoveObject(intantWorldObject.transform, spawnWorldPosition, 0.5f));
        }

        yield return StartCoroutine(RotateUI(largeCategory.GetComponent<RectTransform>(), -rotationAngle, Duration));
        isMove = false;
        worldCategory.SetActive(false);
    }

    private IEnumerator HandleWorldResetSequentially()
    {
        isMove = true;
        worldCategory.SetActive(true);
        yield return StartCoroutine(RotateUI(largeCategory.GetComponent<RectTransform>(), rotationAngle, Duration));

        if (intantWorldObject == null)
        {
            intantWorldObject = Instantiate(worldMain, worldCategory.transform);
            intantWorldObject.transform.localPosition = spawnWorldPosition;
        }
        yield return StartCoroutine(MoveObject(intantWorldObject.transform, Vector3.zero, 0.5f));

        isMove = false;
    }

    private IEnumerator HandleSoundExitSequentially()
    {
        isMove = true;
        SetState(GalleryState.GalleryMain);
        if (soundMain != null)
        {
            yield return StartCoroutine(MoveObject(soundMain.transform, currentSoundPosition, 0.5f));
        }

        yield return StartCoroutine(RotateUI(largeCategory.GetComponent<RectTransform>(), -rotationAngle, Duration));
        isMove = false;
        soundCategory.SetActive(false);
    }

    private IEnumerator HandleSoundResetSequentially()
    {
        isMove = true;
        soundCategory.SetActive(true);
        yield return StartCoroutine(RotateUI(largeCategory.GetComponent<RectTransform>(), rotationAngle, Duration));

        yield return StartCoroutine(MoveObject(soundMain.transform, endSoundPosition, 0.5f));

        isMove = false;
    }

    private void TransFadeIn(int index) 
    {

         for (int i = 0; i < childTransforms.Count; i++)
         {
             if (i == index)
             {
                 currentPosition[i] = currentPositionCount;
                 ButtonMove(childTransforms[i], currentPosition[i],Duration);
                 continue;
             }
             ButtonFadeIn(childTransforms[i]);
         }
        
    }

    private void TransFadeOut(int index)
    {
        for (int i = 0; i < childTransforms.Count; i++)
        {
            if (i == index)
            {
                currentPositionCount = childTransforms[i].localPosition;
                ButtonMove(childTransforms[i], targetPositionCount,Duration);
                continue;
            }
            ButtonFadeOut(childTransforms[i]);
        }
    }

    public void TransRotationIn()
    {
        ButtonRotation(largeCategory.GetComponent<RectTransform>(),rotationAngle, Duration);
    }

    public void TransRotationOut()
    {
        ButtonRotation(largeCategory.GetComponent<RectTransform>(), -rotationAngle, Duration);
    }

    private void WorldFadeIn(float _duration)
    {
        if (intantWorldObject == null)
        {
            intantWorldObject = Instantiate(worldMain, worldCategory.transform);
            intantWorldObject.transform.localPosition = spawnWorldPosition;
        }
        StartCoroutine(MoveObject(intantWorldObject.transform, Vector3.zero, _duration));
    }

    private void WorldFadeOut( float _duration)
    {
        if (intantWorldObject != null)
        {
            StartCoroutine(MoveObject(intantWorldObject.transform, spawnWorldPosition, _duration));
        }
    }

    public bool GetFadeState()
    {
        return isFade;
    }

    private void SetCategoryChild(GameObject _CategoryObject)
    {
        childTransforms.Clear();
        currentPosition.Clear();
        if (_CategoryObject != null)
        {
            for (int i = 0; i < _CategoryObject.transform.childCount; i++)
            {
                Transform child = _CategoryObject.transform.GetChild(i);
                childTransforms.Add(child);
                currentPosition.Add(child.localPosition);
            }
        }
    }


    /*--------------------------------------------------------------------------------------------------------------
    * -------------------------------------------------------------------------------------------------------------
    * -------------------------------------------変数取得関連関数----------------------------------------------
    * -------------------------------------------------------------------------------------------------------------
    * -------------------------------------------------------------------------------------------------------------
    * -------------------------------------------------------------------------------------------------------------*/

    public int GetInputIndex()
    {
        return inputIndex;
    }
}
